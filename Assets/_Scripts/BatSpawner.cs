using UnityEngine;
using System.Collections;

using sillymutts;
public class BatSpawner : MonoBehaviour {

	[Tooltip ("GameObject to Spawn")]
	public GameObject aBatObject;

	[Tooltip ("Number of Bats in the Cluster to Spawn")]
	public int numberOfBatsInCluster = 10;

	[Tooltip ("Number of Waypoint in Fly Radius") ]
	public int numberWaypoint = 10;

	[Tooltip ("Bat Fly Radius") ]
	public float radiusWaypoint = 10f;
	 
	[Tooltip ("Random Range Speed Movement Between waypoints")]
	public Vector2 SpeedRange = new Vector2 (10, 40);

	void Start () {
		for ( int i =0 ; i < numberOfBatsInCluster ; i++){
			Spawn();
		}
	}
 
	int sign = 1;	
	void Spawn(){
 
		//GameObject newBat = Instantiate(aBatObject, this.transform.position , transform.rotation) as GameObject
		GameObject newBat = GameObjectUtility.Instantiate(aBatObject, this.transform.position);
		newBat.transform.rotation =  transform.rotation;

		ActionWaypointMovement wps =  newBat.GetComponentInChildren<ActionWaypointMovement>();
		wps.moveSpeed = Random.Range(SpeedRange.x,SpeedRange.y);

		wps.waypoints = new GameObject[numberWaypoint];

		int indx = 0;
		foreach ( GameObject wp in wps.waypoints){
			if (wps.waypoints[indx]!=null){
				Destroy(wps.waypoints[indx]);
			}
			GameObject go = new GameObject();
			go.name = "wp"+indx;
			go.transform.parent = newBat.transform;
			go.transform.position = this.transform.position;
			float rx = Random.Range(0,radiusWaypoint);
			float ry = Random.Range(0,radiusWaypoint);
			Vector3 v = go.transform.position + new Vector3(sign * rx,sign * ry,0);
			go.transform.position =v ;
			wps.waypoints[indx] = go;
			sign = sign*-1;
			indx ++;
		}
	}
}
