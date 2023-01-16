using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezes : MonoBehaviour, IFreezeUnFreeze {

    Rigidbody2D body2D;

    private void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
    }

    // do what needs to be done to freeze the player
    public void FreezeMotion()
    {
        body2D.velocity = Vector3.zero;
        body2D.isKinematic = true;
    }

    // do what needs to be done to unfreeze the player
    public void UnFreezeMotion()
    {
        body2D.isKinematic = false;
    }


}
