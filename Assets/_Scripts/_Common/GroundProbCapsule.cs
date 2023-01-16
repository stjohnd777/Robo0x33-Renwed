using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundProbCapsule : MonoBehaviour, IsGrounded {

    // LayerMask to determine what is considered ground for the player
    [Tooltip("Determins what layers are considered ground.")]
    public LayerMask whatIsGround;

    // Transform just below feet for checking if player is grounded
    [Tooltip("Center Possition for Cicrle Probe.")]
    public CapsuleCollider2D target  ;

    [Tooltip("Ground Probe Circle Radius.")]
    public float radius = 20;

    [Tooltip("Public variable for other script to read.")]
    public bool IsGrounded;


    // Update is called once per frame
    void Update()
    {
        IsGrounded = target.IsTouchingLayers(whatIsGround);


        //IsGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);
    }

    public bool IsOnGround()
    {
        IsGrounded = target.IsTouchingLayers(whatIsGround);
        return IsGrounded;
    }
}
