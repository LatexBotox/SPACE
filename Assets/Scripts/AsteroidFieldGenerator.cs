using UnityEngine;
using System.Collections;

public class AsteroidFieldGenerator : MonoBehaviour {

	public Destructables baseAsteroid;

	void Start () {
		float [,] noise = new float[10,10];

		float startX, startY;
		startX = 100;
		startY = 100;
		int i;
		for (i = 0; i < noise.GetLength(0);i++) {
			for (int j = 0; j < noise.GetLength(1);j++){
				noise[i,j] = Mathf.PerlinNoise((startX+j),(startY+i));
			}
		}

		i = 0;
		string noisePrint = "";
		foreach (float f in noise){
			noisePrint = noisePrint + ", " + f;
			i++;
			if( i == 10) {
				i = 0;
				noisePrint += "\n";
			}
		}
		print(noisePrint);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
