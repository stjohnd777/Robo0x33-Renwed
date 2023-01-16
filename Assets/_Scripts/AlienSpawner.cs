using UnityEngine;
using System.Collections;
using sillymutts;
public class AlienSpawner : MonoBehaviour
{

	[Tooltip ("Is the Spawner Active")]
	public bool active = true;

	[Tooltip ("GameObject to Spawn")]
	public GameObject aGameObject;

	[Tooltip ("Maximum Nunber of Entities to Spawn")]
	public int maxNumberToSpawn = 100;

	[Tooltip ("Random Range between spawns ")]
	public Vector2 delayRangeBetweenSpawns = new Vector2 (1, 2);

	[Tooltip ("Random Range on the x value waypoint ")]
	public Vector2 waypointRangeX = new Vector2 (50, 100);

	[Tooltip ("Random waypoint speed")]
	public Vector2 waypointSpeed = new Vector2 (50, 100);

	GameObject player;
	int counter = 0;
	int sign = 1;

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		StartCoroutine (Generator ());
	}
		

	IEnumerator Generator ()
	{
		float randDelay = Random.Range (delayRangeBetweenSpawns.x, delayRangeBetweenSpawns.y);
		yield return new WaitForSeconds (randDelay);
		if (counter > maxNumberToSpawn) {
			active = false;
		}
		if (active) {
			Spawn ();
			counter++;
			StartCoroutine (Generator ());
		}
	}

	void Spawn ()
	{
		//GameObject newAlien = Instantiate(aGameObject, new Vector3(transform.position.x ,transform.position.y,transform.position.z ) , transform.rotation) as GameObject;
		
		GameObject newAlien = GameObjectUtility.Instantiate (aGameObject, transform.position);// , transform.rotation)  ;//as GameObject;
		newAlien.transform.position = transform.position;
 
		ActionWaypointMovement wps = newAlien.GetComponentInChildren<ActionWaypointMovement> ();
		wps.moveSpeed = Random.Range (waypointSpeed.x, waypointSpeed.y);

		GameObject[] waypoints = wps.waypoints;
	
		foreach (GameObject wp in waypoints) {
			float rx =Random.Range (waypointRangeX.x, waypointRangeX.y);
			Vector3 v = wp.transform.position + new Vector3 (sign * rx, 0, 0);
			wp.transform.position = v;
			sign = sign * -1;
		}

		//Vector2 w = new Vector3 (player.transform.position.x, wps.waypoints [0].transform.position.y, player.transform.position.z);
		//wps.waypoints [0].transform.position = w;

		counter++;
	}
}
