using UnityEngine;
using System.Collections;

/*
 * This script lives on an empty game object, and the location of
 * this empty game object the script lives on is the spawn point.
 * 
 */
public class SpawnerRandomInterval : MonoBehaviour {

	[Tooltip("Spawner is active or not")]
	public bool active = true;

	[Tooltip("Object to Spawn")]
	public GameObject  prefab;

	[Tooltip("Delay between spawning, dynamically set a randon b/t delayRange")]
	float delay = 2.0f;

	[Tooltip("The randon range min/max")]
	public Vector2 delayRange = new Vector2(1, 2);

	// Use this for initialization
	void Start () {
		ResetDelay ();
		StartCoroutine (Generator ());
	}

	IEnumerator Generator(){

		yield return new WaitForSeconds (delay);

		if (active) {
			var newTransform = transform;

			Instantiate(prefab, newTransform.position, Quaternion.identity);

			ResetDelay();
		}

		StartCoroutine (Generator ());

	}

	void ResetDelay(){
		delay = Random.Range (delayRange.x, delayRange.y);
	}

}
