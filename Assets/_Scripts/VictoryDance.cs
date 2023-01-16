using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryDance : MonoBehaviour, IVictory {

    public AudioClip victorySFX;

    public string victoryAmination;

    private Rigidbody2D body2D;

    private AudioSource _audio;

    private Animator animator;
    public void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

    }
 
    public void Victory()
    {

        PlaySound(victorySFX);

        FreezeMotion();

        animator.SetTrigger(victoryAmination);

 
    }

    public void FreezeMotion()
    {
        body2D.velocity = Vector3.zero;
        //playerCanMove = false;
        body2D.isKinematic = true;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip)
        {
            _audio.PlayOneShot(clip);
        }
    }
}
