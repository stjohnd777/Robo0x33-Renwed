using UnityEngine;
using System.Collections;


/* 
 * By default, it was set to five. Now, if we were to move something closer to the camera, you'll see that it's incredibly large. And again, 
 * remember, this because all of our artwork is a one for one ratio to units in Unity. 
 * 
 * So each one of these units is now a pixel. Well, at different resolutions on different devices, we're not gonna be able to maintain a 
 * consistent look and feel. So, in order to fix that, we're gonna create a script that will allow us to automatically resize the camera based on the 
 * height of the screen that it's being played on. 
 * 
 * 
 * First, we're gonna wanna keep track of the pixels to unit ratio that we set up for our artwork. 
 * 
 * Next, we'll need another public static property to represent the scale.  
 */
public class PixelPerfectCamera : MonoBehaviour {

	// The art pixels to unit
	public static float pixelsToUnits = 32f;//1f; // pixels/meters
	 
	// The calculated scale 
	public static  float scale = 1f;
	 
	[Tooltip("Native Resoultion of our Game, 240x160 GameBoy Advanced")]
	public Vector2 nativeResolution = new Vector2(240,160); 

	void Awake () {

		// Refernece to the camera, script is on camera
		var camera = GetComponent<Camera>();

		// unity in 2d mode orthographic already set, not needed
		if ( camera.orthographic) {

			// 1920 / 240 = 8 scale by 8
			scale = Screen.height / nativeResolution.y; // pixels / intended pixels

			// 
			pixelsToUnits = pixelsToUnits * scale; // pixels/meter * number

			// the 0 possition middle scene Screen.height / 2f 
			// camera is in unity units 
			camera.orthographicSize = ( Screen.height / 2f ) / pixelsToUnits;  // pixels / (pixels/meters) = meters
		}
	}
	
 
}
