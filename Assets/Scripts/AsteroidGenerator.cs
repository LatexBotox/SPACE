﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidGenerator : MonoBehaviour {
	float octaves = 6;
	float threshold = 0.975f;
	float blend = 0.02f;
	float amplitude = 0.5f;
	float frequency = 1.2f;

	int seed = 0;

	Texture2D[] textures;
	Texture2D[] normals;

	Texture2D[,,,] textureArray;

	public Asteroid.Mineral[] minerals;
	public Asteroid baseAsteroid;
	public Color baseColor;

	public SortedList<Asteroid.Mineral,float> mineralOccurance;
	float GenericAsteroidRate = 5;

	PerlinNoise pnoise;

	public int variants = 4;
	int sizes = 3;
	public int maxRes = 512;

	int[,] chunks;
	int chunkSize = 128;

	GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");


		float time = 0 - Time.realtimeSinceStartup;

		Random.seed = seed = (int)(seed==0?Time.realtimeSinceStartup:seed);

		mineralOccurance = new SortedList<Asteroid.Mineral, float>();

		foreach (Asteroid.Mineral m in minerals)
			mineralOccurance.Add(m,0.5f);

		textureArray = new Texture2D[sizes,mineralOccurance.Count+1,variants,2];

		for (int size = 0;size < sizes;size++) {
			for (int mineral = 1;mineral < minerals.Length+1;mineral++) {
				for (int variant = 0;variant < variants;variant++) {
					int res = maxRes/(int)Mathf.Pow (2, sizes-size-1);
					textureArray[size,mineral,variant,0] = new Texture2D(res, res);
					textureArray[size,mineral,variant,1] = new Texture2D(res, res);

					GenerateSpecificTexture (textureArray[size,mineral,variant,0],
					                 				 textureArray[size,mineral,variant,1],
					                				 baseColor*Random.Range (0.7f, 1.3f),
					                 				 Asteroid.MineralToColor(minerals[mineral-1]),
					                         Random.Range (int.MinValue,int.MaxValue));
				}
			}
		}

		for (int size = 0;size < sizes;size++) {
			for (int variant = 0;variant < variants;variant++) {
				int res = maxRes/(int)Mathf.Pow (2, sizes-size-1);
				textureArray[size,0,variant,0] = new Texture2D(res, res);
				textureArray[size,0,variant,1] = new Texture2D(res, res);

				GenerateGenericTexture (textureArray[size,0,variant,0],
				                        textureArray[size,0,variant,1],
				                        baseColor*Random.Range (0.7f, 1.3f),
				                        Random.Range (int.MinValue,int.MaxValue));
			}
		}



		time += Time.realtimeSinceStartup;
		print ("Generated Textures in: " + time);
		pnoise = new PerlinNoise(Random.Range (int.MinValue, int.MaxValue));
		chunks = new int[64,64];
		enabled = player;
	}

	void FixedUpdate() {
		if (player && chunks[(int)player.transform.position.x/chunkSize+chunks.GetLength (0)/2,
		           					 (int)player.transform.position.y/chunkSize+chunks.GetLength (1)/2] != 2) {
			StartCoroutine("GenerateAdjacentChunks");
		}
	}

	IEnumerator GenerateAdjacentChunks() {
		int chunkx = (int)player.transform.position.x/chunkSize+chunks.GetLength (0)/2;
		int chunky = (int)player.transform.position.y/chunkSize+chunks.GetLength (1)/2;
		chunks[chunkx, chunky] = 2;
		for (int x = Mathf.Clamp (chunkx-5, 0, chunks.GetLength (0)-1);
		     x <= Mathf.Clamp (chunkx+5, 0, chunks.GetLength (0)-1); x++) {
			for (int y = Mathf.Clamp (chunky-5, 0, chunks.GetLength (0)-1); 
			     y <= Mathf.Clamp (chunky+5, 0, chunks.GetLength (1)-1); y++) {
				if (chunks[x,y] == 0) {
					GenerateChunk (x, y);
					chunks[x,y] = 1;
					yield return new WaitForFixedUpdate();
				}
			}
		}
		chunks[chunkx, chunky] = 2;
	}

	void GenerateChunk(int chunkx, int chunky) {
		print ("Generated chunk!");
		for(int x = -chunkSize/128; x < chunkSize/128; x++) {
			for(int y = -chunkSize/128; y < chunkSize/128; y++) {
				float _x = x*64+chunkSize*(chunkx-chunks.GetLength(0)/2);
				float _y = y*64+chunkSize*(chunky-chunks.GetLength(1)/2);

				float ran = pnoise.FractalNoise2D(_x/256, _y/256, 3, 0.1f, 0.7f);

				if (ran > 0f) {
					transform.position = new Vector3(_x, _y, 1)+(Vector3)Random.insideUnitCircle*31;
					GenerateRandomAsteroid (ran);
				}
			}
		}
	}

	public Asteroid GenerateRandomAsteroid(float ran) {
		float mineralRange = GenericAsteroidRate;
		int size = Mathf.FloorToInt (Mathf.Lerp (1,sizes,ran*1.5f));
		

		foreach (KeyValuePair<Asteroid.Mineral, float> p in mineralOccurance)
			mineralRange += p.Value;

		float mineralf = Random.Range (0, mineralRange);
	
		float fa = 0f;
		foreach (KeyValuePair<Asteroid.Mineral, float> p in mineralOccurance) {
			fa += p.Value;
			if (fa > mineralf)
				return GenerateAsteroid (p.Key, size);
		}
		return GenerateAsteroid (Asteroid.Mineral.Blank, size);
	}

	public Asteroid GenerateAsteroid(Asteroid.Mineral mineral, int size) {
		Random.seed = seed = seed+1;
		int ran = Random.Range (0,variants-1);

		int mineralIndex = mineralOccurance.IndexOfKey (mineral)+1;

		Asteroid clone = Instantiate (baseAsteroid, transform.position, baseAsteroid.transform.rotation) as Asteroid;

		clone.renderer.material.mainTexture = textureArray[Mathf.Clamp(size,0,sizes-1),mineralIndex,ran,0];
		textureArray[Mathf.Clamp(size,0,sizes-1),mineralIndex,ran,0].Apply();
		
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


	void GenerateSpecificTexture(Texture2D tex, Texture2D normal, Color asteroidColor, Color mineralColor, int seed) {
		float _x, _y, _z, noise;
		int width = tex.width;
		int height = tex.height;

		Color[] pix = new Color[width*height];
		Color[] pixn = new Color[width*height];

		noise = 0;
		
		Gradient[] gradients = MakeGradients(asteroidColor, mineralColor);
		
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

	void GenerateGenericTexture(Texture2D tex, Texture2D normal, Color asteroidColor, int seed) {
		float _x, _y, _z, noise;
		int width = tex.width;
		int height = tex.height;

		Color[] pix = new Color[width*height];
		Color[] pixn = new Color[width*height];
		
		noise = 0;
		
		Gradient[] gradients = MakeGenericGradients(asteroidColor);

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

	Gradient[] MakeGradients(Color asteroidColor, Color mineralColor) {

		Gradient[] gradients = new Gradient[2];
		gradients[0] = new Gradient();
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
		GradientAlphaKey[] gak = new GradientAlphaKey[3];
		gak[0].alpha = 0.15f;
		gak[0].time = 0;
		gak[1].alpha = 0.15f;
		gak[1].time = threshold+0.01f;
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

	Gradient[] MakeGenericGradients(Color asteroidColor) {
		Gradient[] gradients = new Gradient[2];
		gradients[0] = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[2];
		gck[0].color = Color.black;
		gck[0].time = 0;
		gck[1].color = asteroidColor;
		gck[1].time = 1;
		GradientAlphaKey[] gak = new GradientAlphaKey[2];
		gak[0].alpha = 0.15f;
		gak[0].time = 0;
		gak[1].alpha = 0.15f;
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
}