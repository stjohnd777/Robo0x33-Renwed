using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class CameraBoundsChecker : MonoBehaviour {

	Camera camera;

	Camera2DFollow cameraFollow;

	BoxCollider2D boxCollider;

	float orthoSize;
	float aspect;

 
	void Awake () {

		camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();

		cameraFollow = camera.GetComponent<Camera2DFollow> ();

		boxCollider = GetComponent<BoxCollider2D> ();
	}
	

	void FixedUpdate () {

		orthoSize = camera.orthographicSize;
		aspect = camera.aspect;
		boxCollider.size = new Vector2 ( 2*orthoSize * aspect , 2*orthoSize);
		//boxCollider.size = new Vector2 (800 , 600);
	}

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Boundry")
        {
			cameraFollow.enabled = true;
        }
    }

	void OnTriggerEnterStay(Collider2D collider)
	{

		if (collider.gameObject.tag == "Boundry")
		{
			cameraFollow.enabled = true;
		}
	}

    void OnTriggerExit2D(Collider2D collider){
		if (collider.gameObject.tag == "Boundry") {
			cameraFollow.enabled = false;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1f,0f,0f,.3f);
		Gizmos.DrawCube(camera.transform.position, new Vector3( 2*orthoSize * aspect ,2*orthoSize, 1));
	}
}
