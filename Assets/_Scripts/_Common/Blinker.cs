using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour {


    public GameObject target;

    public float timeInSeconds;

    public bool defaultActivationState = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Awake () {

        StartCoroutine(toggle(timeInSeconds));
	}

    IEnumerator toggle(float delay)
    {
       yield  return new WaitForSeconds(delay);

        defaultActivationState = !defaultActivationState;

        target.SetActive(!defaultActivationState);

        StartCoroutine(toggle(timeInSeconds));
    }
}
