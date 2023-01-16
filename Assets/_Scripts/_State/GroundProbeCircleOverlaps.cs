using UnityEngine;
using System.Collections;


public interface IsGrounded {
	 bool IsOnGround ();
}
/*
 * This script determines is the hosting game object
 * is grounded or not by checking for overlap of a 
 * circle of 'radius' centered at 'groundCheck.position'
 * with the layers set in the layer mask
 * 
 * 
 */ 
public class GroundProbeCircleOverlaps : MonoBehaviour, IsGrounded {


	// LayerMask to determine what is considered ground for the player
	[Tooltip("Determins what layers are considered ground.")]
	public LayerMask whatIsGround;

	// Transform just below feet for checking if player is grounded
	[Tooltip("Center Possition for Cicrle Probe.")]
	public Transform groundCheck;

	[Tooltip("Ground Probe Circle Radius.")]
	public float radius = 20;

	[Tooltip("Public variable for other script to read.")]
	public bool IsGrounded;


	// Update is called once per frame
	void Update () {
	
		IsGrounded = Physics2D.OverlapCircle(groundCheck.position,radius,whatIsGround);  
	}

	public bool IsOnGround(){
		IsGrounded = Physics2D.OverlapCircle(groundCheck.position,radius,whatIsGround);  
		return IsGrounded;
	}


	void OnDrawGizmos(){
		
		Gizmos.DrawWireSphere (groundCheck.position,radius);

	}

}
