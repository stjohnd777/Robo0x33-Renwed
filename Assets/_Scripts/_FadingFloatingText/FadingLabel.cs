using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Assertions;

/**
 * FadingLabel is designed to imatate the Bordlands Effects,
 * The lable display text, move in a defined direction in x and y,
 * and fades out over a fixed length of time. This script live on a 
 * prefab container with a UI.Text field.
 * 
 */
public class FadingLabel : MonoBehaviour {

	public float FixedUpdateFadeOutSpeed = .95f;
	public float FixedUpdateDriftX = 1.00f;
	public float FixedUpdateDriftY = 1.01f;
	public float DeleteTime = 2.01f;
	public Color start;
	public Color end;


	private Text uitext;
	void Awake () {

		uitext = this.GetComponentInChildren<Text>();
		Assert.IsNotNull(uitext);

		//Destroy(this,2)
	}
	
	public void SetText (string text){

		uitext.text = text;

		Destroy(uitext,DeleteTime);
	}

	public float s = 1f;

	void FixedUpdate(){

		if (uitext.color.a > .01f) {

			uitext.color = new Color (uitext.color.r, uitext.color.g, uitext.color.b, FixedUpdateFadeOutSpeed * uitext.color.a);

			uitext.transform.position = new Vector2 (
				uitext.transform.position.x * FixedUpdateDriftX,
				uitext.transform.position.y * FixedUpdateDriftY);
		} else {
			Destroy (this);
		}

	}

}
