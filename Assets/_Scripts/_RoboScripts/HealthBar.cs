using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class HealthBar : MonoBehaviour {


	public GameObject healthBar;

	public bool hideBar = false;

	IHealth health;
	void Start () {
		
		health = GetComponentInParent<Health>();
        if ( health == null)
        {
            health = GetComponentInParent<IHealth>();
        }
        Assert.IsNotNull(health, "Tag: " + tag + " Name:"+name);
	}
	
	// Update is called once per frame
	void Update () {

		if (  health  != null) {
			float percentage = health.GetHealthPercent ();
			SetHealthPercent (percentage);
			//healthBar.transform.localScale = new Vector3 (percent, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
		}
	
	}

	public void SetHealthPercent(float percentage){ 
		if (percentage < 0) {
			percentage = 0;
		}
 		healthBar.transform.localScale = new Vector3(percentage  , healthBar.transform.localScale.y, healthBar.transform.localScale.z);

	}
		
}
