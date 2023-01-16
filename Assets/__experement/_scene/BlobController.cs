using System;
using UnityEngine;


public class BlobController : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 100f; // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 800f; // Amount of force added when the player jumps.

    [Range(0, 1)] [SerializeField]
    private float m_CrouchSpeed = .36f; // Amount of maxSpeed applied to crouching movement. 1 = 100%

    [SerializeField] private bool m_AirControl = false; // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround; // A mask determining what is ground to the character

    [SerializeField] private Collider2D[] colliders;

    [SerializeField]  private Transform[] m_GroundChecks; // A position marking where to check if the player is grounded.
    
    [SerializeField]  float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [SerializeField]  bool m_Grounded; // Whether or not the player is grounded.
    
    private Rigidbody2D m_Rigidbody2D;
    
    private GameObject[] orbs;
    private void Awake()
    {
        // Setting up references.

        orbs = GameObject.FindGameObjectsWithTag("OrbPE");
        
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public bool IsGrounded()
    {
        return true;
        
        // foreach (GameObject go in orbs)
        // {
        //     m_Grounded = Physics2D.OverlapCircle(go.transform.position, k_GroundedRadius, m_WhatIsGround);
        //     if (m_Grounded)
        //     {
        //         break;
        //     }
        // }
        // return m_Grounded;
    }


    private void FixedUpdate()
    {
        
        // foreach (var orb in orbs)
        // {
        //     orb.SetActive(false);
        // }
        Move();
        
        // if (m_Grounded)
        // {
        //     foreach (var orb in orbs)
        //     {
        //         orb.SetActive(false);
        //     }
        //     Move();
        // }
        // else
        // {
        //     foreach (var orb in orbs)
        //     {
        //         orb.SetActive(true);
        //     }
        // }

        //m_Grounded = false;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            m_Grounded = true;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        bool isStayed = false;
        if (other.gameObject.tag == "Ground")
        {
            m_Grounded = true;
            isStayed = true;
        }

        if (isStayed == false)
        {
            m_Grounded = false;
        }
    }

    void Move()
    {
        var move = Input.GetAxis("Horizontal");
        var up = Input.GetAxis("Vertical");

        // float move, bool crouch, bool jump

        // If crouching, check to see if the character can stand up
        // if (!crouch && m_Anim.GetBool("Crouch"))
        // {
        //     // If the character has a ceiling preventing them from standing up, keep them crouching
        //     if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
        //     {
        //         crouch = true;
        //     }
        // }

        // Set whether or not the character is crouching in the animator
        // m_Anim.SetBool("Crouch", crouch);


        if (move != 0)
        {
            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            m_Rigidbody2D.rotation = move * m_MaxSpeed;
            
            foreach (var orb in orbs)
            {
                orb.SetActive(true);
            }
        }
        else
        {
            foreach (var orb in orbs)
            {
                orb.SetActive(false);
            }
        }


        if (up != 0)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce * up));
        }
    }
}