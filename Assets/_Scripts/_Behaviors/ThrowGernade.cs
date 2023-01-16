using UnityEngine;
using System.Collections;
using sillymutts;
public class ThrowGernade : MonoBehaviour {


	public GameObject gernade;

	public Vector3 forceThrow;

	public void Throw(){

		IFacingRight isRight = GetComponentInParent<IFacingRight> ();
		float dirX = 1;
		if (!isRight.IsFacingRight ()) {
			dirX = -1;
		}
		GameObject aGernade = GameObjectUtility.Instantiate (gernade, this.transform.position);
		aGernade.GetComponent<Rigidbody2D> ().AddForce (new Vector3(dirX*forceThrow.x,forceThrow.y,forceThrow.z));

	}

}
