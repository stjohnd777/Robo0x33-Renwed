using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMapController : MonoBehaviour {

    public GameObject miniMap;

    public bool Active = true;

	public void Show()
    {
        miniMap.SetActive(true);
    }

    public void Hide()
    {
        miniMap.SetActive(false);
    }

    public void toggel()
    {
        Active = !Active;
        miniMap.SetActive(Active);
        
    }

}
