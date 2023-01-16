using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;


public class MeleeAttack : MonoBehaviour
{

    [System.Serializable]
    public enum MyInput{
	    Key,Axis
    }


	public MyInput input;

	public string AxisName;

	public KeyCode key;

	public string Tag;

	public GameObject melee;

	public MonoBehaviour[] disabledBehaviors; 

	public bool IsRequestingMelee;

	private bool lastRead = false;

	IHealth healthScript;


    private void Awake()  
    {
 
		if (melee == null) {
			melee = GameObject.FindGameObjectWithTag (Tag);
		}
        melee.SetActive(false);

		healthScript = GetComponent<IHealth> ();
    }

    void Update () {
	
		//if (melee == null) {
		//	Debug.Log ("mising melee oobject " + tag);
		//	return;
		//}
        Assert.IsNotNull(melee);

		bool IsRequestingMelee = false;

		if (input == MyInput.Axis) {
			float on = Input.GetAxis (AxisName);
			if (on > 0) {
				IsRequestingMelee = true;
			}
		} else {

			IsRequestingMelee = Input.GetKeyDown (key);
			Debug.Log ("Reading Key " + key + " = " +  IsRequestingMelee);
		}

		//if (IsRequestingMelee)
  //      {
		//	if (healthScript != null) {
		//		healthScript.SetImmunityForSec (.2f);
		//	}
  //          melee.SetActive(true);
  //      }
  //      else
  //      { 
  //          melee.SetActive(false);
  //      }

        if (IsRequestingMelee)
        {
            melee.SetActive(true);
        }
        else
        {
            melee.SetActive(false);
        }




    }
}
