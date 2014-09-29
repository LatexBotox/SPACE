using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AsteroidGenerator : MonoBehaviour {
	float octaves = 6;
	float threshold = 0.975f;
	float blend = 0.02f;
	float amplitude = 0.5f;
	float frequency = 1.2f;

	int seed = 0;
	public int levelSeed;

	Texture2D[] textures;
	Texture2D[] normals;

	Texture2D[,,,] textureArray;

	public MineralType[] minerals;
	public Asteroid baseAsteroid;
	public Mineral baseMineral;
	public Color baseColor;

	public SortedList<MineralType,float> mineralOccurance;
	float GenericAsteroidRate = 5;

	PerlinNoise lnoise;

	public int variants = 4;
	int sizes = 3;
	public int maxRes = 512;
	
	int chunkSize = 128;

	GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");


		float time = 0 - Time.realtimeSinceStartup;

		Random.seed = seed = (int)(seed==0?Time.realtimeSinceStartup:seed);

		mineralOccurance = new SortedList<MineralType, float>();

		foreach (MineralType m in minerals)
			mineralOccurance.Add(m,0.5f);

		textureArray = new Texture2D[sizes,2,variants,2];

		for (int size = 0; size < sizes;size++) {
			for (int variant = 0; variant < variants;variant++) {
				int res = maxRes/(int)Mathf.Pow (2, sizes-size-1);
				textureArray[size,1,variant,0] = new Texture2D(res, res);
				textureArray[size,1,variant,1] = new Texture2D(res, res);
				
				GenerateMineraledTexture (textureArray[size,1,variant,0],
				                       	 textureArray[size,1,variant,1],
				                         Random.Range (int.MinValue,int.MaxValue));
			}
		}

		for (int size = 0;size < sizes;size++) {
			for (int variant = 0;variant < variants;variant++) {
				int res = maxRes/(int)Mathf.Pow (2, sizes-size-1);
				textureArray[size,0,variant,0] = new Texture2D(res, res);
				textureArray[size,0,variant,1] = new Texture2D(res, res);

				GenerateGenericTexture (textureArray[size,0,variant,0],
				                        textureArray[size,0,variant,1],
				                        Random.Range (int.MinValue,int.MaxValue));
			}
		}

		time += Time.realtimeSinceStartup;
		print ("Generated Textures in: " + time);

		enabled = player;
	}

	public void GenerateChunk(Chunk chunk) {
		lnoise = new PerlinNoise(levelSeed);

		int chunkx, chunky;
		chunkx = chunk.chunkx;
		chunky = chunk.chunky;

		int id = 0;

		Random.seed = chunk.chunkSeed;

		for(int x = -chunkSize/128; x < chunkSize/128; x++) {
			for(int y = -chunkSize/128; y < chunkSize/128; y++) {
				float _x = x*64+chunkSize*chunkx;
				float _y = y*64+chunkSize*chunky;

				float ran = lnoise.FractalNoise2D(_x/256, _y/256, 3, 0.1f, 0.7f);

				if (ran > 0f) {
					transform.position = new Vector3(_x, _y, 1) +(Vector3)Random.insideUnitCircle*31;
					Asteroid a = GenerateRandomAsteroid (ran, chunk.chunkSeed);
					a.chunk = chunk;
					a.id = id;
					chunk.asteroids.Add(id++, a);
				}
			}
		}
	}

	public Asteroid GenerateRandomAsteroid(float ran, int seed) {
		Random.seed = seed;
		float mineralRange = GenericAsteroidRate;
		int size = Mathf.FloorToInt (Mathf.Lerp (1,sizes,ran*1.5f));
		

		foreach (KeyValuePair<MineralType, float> p in mineralOccurance)
			mineralRange += p.Value;

		float mineralf = Random.Range (0, mineralRange);
	
		float fa = 0f;
		foreach (KeyValuePair<MineralType, float> p in mineralOccurance) {
			fa += p.Value;
			if (fa > mineralf)
				return GenerateAsteroid (p.Key, size);
		}
		return GenerateAsteroid (MineralType.Blank, size);
	}

	public Asteroid GenerateAsteroid(MineralType mineral, int size) {
		Random.seed = seed = seed+1;
		int ran = Random.Range (0,variants-1);

		int mineralIndex = (mineral==MineralType.Blank?0:1);

		Asteroid clone = Instantiate (baseAsteroid, transform.position, baseAsteroid.transform.rotation) as Asteroid;

		clone.renderer.material.mainTexture = textureArray[Mathf.Clamp(size,0,sizes-1),mineralIndex,ran,0];
		textureArray[Mathf.Clamp(size,0,sizes-1),mineralIndex,ran,0].Apply();
		clone.renderer.material.SetColor ("_Color", baseColor*Random.Range (0.8f, 1.1f));
		clone.renderer.material.SetColor ("_OffColor", Asteroid.MineralToColor(mineral)*Random.Range (0.9f, 1.0f));
		clone.renderer.material.SetColor ("_SpecColor", Asteroid.MineralToColor(mineral)*Random.Range (1.1f, 1.3f));
		
		clone.renderer.material.SetTexture ("_BumpMap", textureArray[Mathf.Clamp(size,0,sizes-1),mineralIndex,ran,1]);
		textureArray[Mathf.Clamp(size,0,sizes-1),mineralIndex,ran,1].Apply ();
		clone.sizeClass = size;
		clone.gen = this;
		clone.mineral = mineral;

		clone.transform.localScale = Vector3.one*2*Random.Range (0.8f, 1.2f)*(Mathf.Pow (2,size));
		clone.rigidbody2D.mass = 2*Mathf.Pow (4,size);

		if (mineralIndex > 0)
			clone.tag = "valuable";

		DeformMesh (clone.GetComponent<MeshFilter>().mesh, clone.GetComponent<PolygonCollider2D>(), (sizes-size+1), Random.Range (int.MinValue, int.MaxValue));

		return clone;
	}

	public Mineral GenerateMineral(MineralType t) {

		Mineral clone = Instantiate(baseMineral, new Vector3(0,0,0), baseMineral.transform.rotation) as Mineral;
		DeformMesh(clone.GetComponent<MeshFilter>().mesh, clone.GetComponent<PolygonCollider2D>(), 1, Random.Range (int.MinValue, int.MaxValue));
		return clone;

	}

	void GenerateMineraledTexture(Texture2D tex, Texture2D normal, int seed) {
		float _x, _y, _z, noise;
		int width = tex.width;
		int height = tex.height;

		Color[] pix = new Color[width*height];
		Color[] pixn = new Color[width*height];

		noise = 0;
		
		Gradient[] gradients = MakeGradients();
		
		PerlinNoise pnoise = new PerlinNoise(seed);
		for(float y = 0; y < width;y++) {
			for(float x = 0; x < height;x++) {
				_x = (x/width-0.5f)*2;
				_y = (y/height-0.5f)*2;
				_z = Mathf.Sqrt (1 - Mathf.Pow (_x, 2) - Mathf.Pow (_y,2));

				noise = 0;
				if (_z>=0)
					noise = (pnoise.FractalNoise3D(_x, _y, _z, (int)octaves, frequency, amplitude));

				pix[(int)y*tex.width+(int)x] = gradients[0].Evaluate (1-Mathf.Abs (noise))*(1-Mathf.Abs (noise));
				pixn[(int)y*tex.width+(int)x] = gradients[1].Evaluate (1-Mathf.Abs (noise))*(1-Mathf.Abs (noise)*4);
			}
		}
		tex.SetPixels(pix);
		normal.SetPixels (HeightToNormal (pixn));
	}

	void GenerateGenericTexture(Texture2D tex, Texture2D normal, int seed) {
		float _x, _y, _z, noise;
		int width = tex.width;
		int height = tex.height;

		Color[] pix = new Color[width*height];
		Color[] pixn = new Color[width*height];
		
		noise = 0;
		
		Gradient[] gradients = MakeGenericGradients();

		PerlinNoise pnoise = new PerlinNoise(seed);
		for(float y = 0; y < width;y++) {
			for(float x = 0; x < height;x++) {
				_x = (x/width-0.5f)*2;
				_y = (y/height-0.5f)*2;
				_z = Mathf.Sqrt (1 - Mathf.Pow (_x, 2) - Mathf.Pow (_y,2));
				
				noise = 0;
				if (_z>=0)
					noise = (pnoise.FractalNoise3D(_x, _y, _z, (int)octaves, frequency, amplitude));
				
				pix[(int)y*tex.width+(int)x] = gradients[0].Evaluate (1-Mathf.Abs (noise))*(1-Mathf.Abs (noise));
				pixn[(int)y*tex.width+(int)x] = gradients[1].Evaluate (1-Mathf.Abs (noise))*(1-Mathf.Abs (noise)*4);
			}
		}
		tex.SetPixels(pix);
		normal.SetPixels (HeightToNormal (pixn));
	}

	Color[] HeightToNormal(Color[] normal) {
		Color[] nout = new Color[normal.Length];
		int side = (int)Mathf.Sqrt(normal.Length);

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

	void DeformMesh (Mesh mesh, PolygonCollider2D collider, int mult, int seed) {
		Vector3[] verts = mesh.vertices;
		Vector3[] normals = mesh.normals;
		
		PerlinNoise pnoise = new PerlinNoise(seed);

		Vector2[] colPoints = new Vector2[14];
		
		int j = 0;
		int k = 0;
		for(int i = 0; i < verts.Length;i++) {
			Vector3 v = normals[i];
			v.z = 0;
			verts[i] = verts[i]+v*(pnoise.FractalNoise3D(verts[i].x, verts[i].y, verts[i].z, (int)octaves/2, frequency*2, amplitude/2*mult));
			if (verts[i].z > -0.01) {
				if (k == 0)
					colPoints[j++%14] = verts[i];
				k = (k+1)%3;
			}
		}
		mesh.vertices = verts;
		collider.points = colPoints;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	Gradient[] MakeGradients() {

		Gradient[] gradients = new Gradient[2];
		gradients[0] = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[5];
		gck[0].color = Color.black;
		gck[0].time = 0;
		gck[1].color = Color.white;
		gck[1].time = threshold-blend;
		gck[2].color = Color.white*0.5f;
		gck[2].time = threshold;
		gck[3].color = Color.white*0.8f;
		gck[3].time = threshold+blend;
		gck[4].color = Color.white;
		gck[4].time = 1;
		GradientAlphaKey[] gak = new GradientAlphaKey[3];
		gak[0].alpha = 0.05f;
		gak[0].time = 0;
		gak[1].alpha = 0.05f;
		gak[1].time = threshold;
		gak[2].alpha = 1;
		gak[2].time = 1;
		gradients[0].SetKeys(gck, gak);
		
		gradients[1] = new Gradient();
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
		gradients[1].SetKeys (gck, gak);

		return gradients;
	}

	Gradient[] MakeGenericGradients() {
		Gradient[] gradients = new Gradient[2];
		gradients[0] = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[2];
		gck[0].color = Color.black;
		gck[0].time = 0;
		gck[1].color = Color.white;
		gck[1].time = 1;
		GradientAlphaKey[] gak = new GradientAlphaKey[2];
		gak[0].alpha = 0.05f;
		gak[0].time = 0;
		gak[1].alpha = 0.05f;
		gak[1].time = 1;
		gradients[0].SetKeys (gck, gak);

		gradients[1] = new Gradient();
		gck[0].color = Color.black;
		gck[0].time = 0;
		gck[1].color = Color.white;
		gck[1].time = 1;
		gradients[1].SetKeys (gck, gak);

		return gradients;
	}

	public void Save() {
		for (int size = 0; size < sizes;size++) {
			for (int variant = 0; variant < variants;variant++) {
				SaveTexture(textureArray[size,1,variant,0], "mineralTex" + levelSeed + "v" + variant + "s" + size );
				SaveTexture(textureArray[size,1,variant,1], "mineralBump" + levelSeed + "v" + variant + "s" + size );
				
				SaveTexture(textureArray[size,0,variant,0], "blankTex" + levelSeed + "v" + variant + "s" + size );
				SaveTexture(textureArray[size,0,variant,1], "blankBump" + levelSeed + "v" + variant + "s" + size );
			}
		}
	}  
	
	void SaveTexture(Texture2D tex, string fileName) {
		byte[] bytes = tex.EncodeToPNG();
		FileStream file = File.Open(Application.dataPath + "/GeneratedTextures/" + fileName + ".png", FileMode.Create);
		
		BinaryWriter bw = new BinaryWriter(file);
		bw.Write(bytes);
		file.Close();
	}
}
