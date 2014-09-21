using UnityEngine;
using System.Collections;

public class NoiseTexture : MonoBehaviour {
	float octaves = 6;
	float threshold = 0.975f;
	float blend = 0.02f;
	float amplitude = 0.5f;


	public float frequency = 1.2f;
	public int side = 256;
	public int seed = 0;
	int oldSeed = 0;

	Vector3[] origverts;
	Vector3[] orignormals;

	public Color mineralColor, asteroidColor;

	Gradient gradient;
	Gradient heightGradient;

	Texture2D tex, normal;
	Color[] pix, pixn;
	// Use this for initialization
	void Start () {
		tex = new Texture2D(side, side);
		normal = new Texture2D(side, side);
		pix = new Color[tex.width*tex.height];
		pixn = new Color[tex.width*tex.height];
		renderer.material.mainTexture = tex;
		renderer.material.SetTexture("_BumpMap", normal); 

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		origverts = mesh.vertices;
		orignormals = mesh.normals;
	}

	void CalcNoise() {
		float _x, _y, _z, noise;
		float width = tex.width;
		float height = tex.height;
		MakeGradients();

		PerlinNoise pnoise = new PerlinNoise(seed);
		for(float y = 0; y < width;y++) {
			for(float x = 0; x < height;x++) {
				_x = (x/width-0.5f)*2;
				_y = (y/height-0.5f)*2;
				_z = Mathf.Sqrt (1 - Mathf.Pow (_x, 2) - Mathf.Pow (_y,2));

				noise = (pnoise.FractalNoise3D(_x, _y, _z, (int)octaves, frequency, amplitude));

				pix[(int)y*tex.width+(int)x] = gradient.Evaluate (1-Mathf.Abs (noise))*(1-Mathf.Abs (noise));
				pixn[(int)y*tex.width+(int)x] = heightGradient.Evaluate (1-Mathf.Abs (noise))*(1-Mathf.Abs (noise)*4);
			}
		}
		tex.SetPixels(pix);
		tex.Apply ();
		normal.SetPixels (HeightToNormal (pixn));
		normal.Apply ();
	}

	Color[] HeightToNormal(Color[] normal) {
		Color[] nout = new Color[normal.Length];
		for (int x = 1; x < side - 1; x++ )
			for (int y = 1; y < side - 1; y++)
		{
			//using Sobel operator
			float tl, t, tr, l, right, bl, bot, br;
			tl = Intensity(normal[side*(y-1)+x-1]);
			t = Intensity(normal[side*(y-1)+x]);
			tr = Intensity(normal[side*(y-1)+x+1]);
			right = Intensity(normal[side*y+x+1]);
			br = Intensity(normal[side*(y+1)+x+1]);
			bot = Intensity(normal[side*(y+1)+x]);
			bl = Intensity(normal[side*(y+1)+x-1]);
			l = Intensity(normal[side*y+x-1]);
			
			//Sobel filter
			float dX = (tl + 2.0f * l + bl) - (tr + 2.0f * right + br);
			float dY = (tl + 2.0f * t + tr) - (bl + 2.0f * bot + br);
			float dZ = 1.0f;
			
			Vector3 vc = new Vector3(dX, dY, dZ);
			vc.Normalize();
			
			nout[y*side+x] = new Color((vc.x + 1f) / 2f, (vc.y + 1f) / 2f, (vc.z + 1f) / 2f, (vc.x + 1f) / 2f);
		}
		return nout;
	}

	public float Intensity(Color color) {
		return (0.299f * color.r + 0.587f * color.g + 0.114f * color.b);
	}
	
	// Update is called once per frame
	void Update () {
		if (oldSeed != seed) {
			DeformMesh();
			CalcNoise ();
			oldSeed = seed;
		}
	}

	void DeformMesh () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
//		float maxz = mesh.bounds.max.z;
		Vector3[] verts = (Vector3[])origverts.Clone();
		Vector3[] normals = (Vector3[])orignormals.Clone();
		PolygonCollider2D collider = GetComponent<PolygonCollider2D>();

		PerlinNoise pnoise = new PerlinNoise(seed);

		float colRes = 16;

		Vector2[] colPoints = new Vector2[16];

		int j = 0;
		int k = 0;
		for(int i = 0; i < verts.Length;i++) {
			Vector3 v = normals[i];
			v.z = 0;
			verts[i] = verts[i]+v*(pnoise.FractalNoise3D(verts[i].x, verts[i].y, verts[i].z, (int)octaves/2, frequency, amplitude/2));
			if (verts[i].z > -0.01) {
				if (k == 0)
					colPoints[++j%16] = verts[i];
				k = (k+1)%3;
			}
		}
		mesh.vertices = verts;
		collider.points = colPoints;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	void MakeGradients() {
		gradient = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[5];
		gck[0].color = Color.black;
		gck[0].time = 0;
		gck[1].color = asteroidColor;
		gck[1].time = threshold-blend;
		gck[2].color = asteroidColor*0.2f;
		gck[2].time = threshold;
		gck[3].color = mineralColor*0.8f;
		gck[3].time = threshold+blend;
		gck[4].color = mineralColor;
		gck[4].time = 1;
		GradientAlphaKey[] gak = new GradientAlphaKey[2];
		gak[0].alpha = 1;
		gak[0].time = 0;
		gak[1].alpha = 1;
		gak[1].time = 1;
		gradient.SetKeys(gck, gak);
		
		heightGradient = new Gradient();
		gck[0].color = new Color(0.3f,0.3f,0.3f);
		gck[0].time = 0;
		gck[1].color = new Color(1,1,1);
		gck[1].time = threshold-blend;
		gck[2].color = new Color(0.2f,0.2f,0.2f);
		gck[2].time = threshold;
		gck[3].color = new Color(0.1f,0.1f,0.1f);
		gck[3].time = threshold+blend;
		gck[4].color = new Color(0,0,0);
		gck[4].time = 1;
		heightGradient.SetKeys (gck, gak);
	}
}
