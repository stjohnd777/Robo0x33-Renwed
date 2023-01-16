using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEvent : MonoBehaviour {

    public delegate void   ApplyExplosionDamage(GameObject explosionAt);
    public static   event  ApplyExplosionDamage OnApplyExplosionDamage;

 
}
