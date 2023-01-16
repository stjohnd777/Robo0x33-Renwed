using UnityEngine;
using System.Collections;

public class csParticleMove : MonoBehaviour
{
    public float speed = 0.1f;

    public Vector3 dir = Vector3.right;
	void Update () {
        transform.Translate(dir * speed);
	}
}
