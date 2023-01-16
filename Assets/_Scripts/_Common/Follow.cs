using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

	// The player
	public GameObject targetYouAreFollowing;

	// offset value
	private Vector3 offset;

	public Transform XeqMinLine;
	public Transform XeqMaxLine;

	public Transform YeqMinLine;
	public Transform YeqMaxLine;

	Camera camera;

	public int screenHieght;
	public int screenWidth;
	public float orthoSize;
	public int pixelHieght;
	public int pixelWidth;
	public float aspect;
	public Vector2 sizeView ;
	public 

	void Start()
	{
		camera = GetComponent<Camera> ();
		offset = transform.position - targetYouAreFollowing.transform.position;
	}

	//  
	void LateUpdate()
	{

		 screenHieght  = Screen.height;
		 screenWidth = Screen.width;
		 orthoSize = camera.orthographicSize;
		 pixelHieght = camera.pixelHeight;
		 pixelWidth = camera.pixelWidth;
		 aspect = camera.aspect;
		 sizeView =  new Vector2 ( 2*orthoSize * aspect , 2*orthoSize);


		if (targetYouAreFollowing != null)
		{

			transform.position = targetYouAreFollowing.transform.position + offset;
		
			 
			float x = transform.position.x;
			float y = transform.position.y;
			float z = transform.position.z;

			 
			if (x < ( XeqMinLine.position.x + sizeView.x /2) ) {
				x = XeqMinLine.position.x;
			}
			if (targetYouAreFollowing.transform.position.x > (XeqMaxLine.position.x - sizeView.x /2)) {
				x = XeqMinLine.position.x;
			}

			if (targetYouAreFollowing.transform.position.y <( YeqMinLine.position.x +  sizeView.y /2 )) {
				y = XeqMinLine.position.x;
			}
			if (targetYouAreFollowing.transform.position.y > ( YeqMaxLine.position.x -  sizeView.y /2) ) {
				y = XeqMinLine.position.x;
			}
 

			transform.position = new Vector3 (x, y, z);
		}
	}

}
