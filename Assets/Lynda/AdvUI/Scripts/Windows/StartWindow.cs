using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : GenericWindow
{
	public Button continueButton;

	public override void Open ()
	{
		var canContinue = true;

		continueButton.gameObject.SetActive (canContinue);

		if (continueButton.gameObject.activeSelf) {
			firstSelected = continueButton.gameObject;
		}


		base.Open ();
	}

	public override void NewGame(){
		windowManager.Open (1);
	}

	public void Continue(){
		Debug.Log ("Continue Pressed");
	}

	public void Options(){
		Debug.Log ("Options Pressed");
	}
	
 
}
