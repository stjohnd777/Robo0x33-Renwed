using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using sillymutts;

public class Dies : MonoBehaviour , IDie {


    public string DeathAnimationTrigger = "Die";

    public float waitOnDie = 2.0f;

    IFreezeUnFreeze freeze;

    Animator animator;

    public void Awake()
    {
        freeze = GetComponent<IFreezeUnFreeze>();

        animator = GetComponent<Animator>();

    }

    public void Die()
    {
        StartCoroutine(KillPlayer());
    }

    // coroutine to kill the player
    IEnumerator KillPlayer()
    {
        if (freeze != null)
        {
            // freeze the player
            freeze.FreezeMotion();
        }
      

        if (animator != null)
        {
            animator.SetTrigger(DeathAnimationTrigger);
        }

 
        yield return new WaitForSeconds(waitOnDie);

        GameObjectUtility.ClearPools();

		if (GameManager.getInstance())
        {
            // if the gameManager is available, tell it to reset the game
			GameManager.getInstance().PlayerHasDied();
        }
        else
        {
            // otherwise, just reload the current level
            Application.LoadLevel(Application.loadedLevelName);
        }
        
    }


}
