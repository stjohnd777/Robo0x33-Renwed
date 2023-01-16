using UnityEngine;
using System.Collections;

public class ZoomInOutCamera : MonoBehaviour {

	Camera camera ;


	public Vector2 Range;

	// Use this for initialization
	private void Start()
	{
		camera = GetComponent<Camera> ();
	}

	// Update is called once per frame
	private void Update()
	{
        
		if (camera != null &&camera.orthographic) {
			float factor = 10;
			float delta = Input.GetAxis ("Mouse ScrollWheel");
			camera.orthographicSize = camera.orthographicSize + factor * delta;
		}

	}

}
