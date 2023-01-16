using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

	public Vector2  orgin = Vector2.zero;

	public GameObject[] gameObjects;

	public int PixelsWidthImage = 256; // pixels x
	public int PixelHeightImage = 256; // pixels y
	public int PixelsToUnit = 64; // pixels / unit

	public int HorizontalTiles = 25;
	public int VerticalTiles = 25;

	public int Key = 1;

	public GameObject SelectARandom(int x, int y){

		var prefab =  gameObjects [RandomHelper.Range (x,y, Key, gameObjects.Length)];
		var go = GameObject.Instantiate (prefab);
		return go;
	}

	void Start () {
 
		var offset = new Vector3 (  
			-HorizontalTiles / 2  *  (PixelsWidthImage / PixelsToUnit)    ,  
			-VerticalTiles   / 2  * ( PixelHeightImage / PixelsToUnit)    , 
			0);

		for (int x = 0; x < HorizontalTiles; x++) {
			
			for (int y =0; y < VerticalTiles; y++) {
			
				var gameObject = SelectARandom (x, y);

				gameObject.transform.position = new Vector3 ( (x + orgin.x )* ( PixelsWidthImage/PixelsToUnit ) ,  (y + orgin.y)  * (PixelHeightImage/PixelsToUnit) , 0) + offset;

				gameObject.name = gameObject.name + "." + gameObject.transform.position;

				gameObject.transform.parent = transform;

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
