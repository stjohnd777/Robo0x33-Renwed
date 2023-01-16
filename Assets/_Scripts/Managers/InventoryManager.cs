using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {


	public static InventoryManager instance;


	Dictionary<string,int> inventory = new Dictionary<string,int>();

	void Awake(){

		if (instance == null){
			instance = this.GetComponent<InventoryManager>();
		}
 
	}

	public void AddItem(string tag){

		int temp = 0;
		if(inventory.TryGetValue(tag, out temp))
		{
			temp++;
			inventory[tag] = temp;
		}
		else
		{
			inventory[tag] = 1;
		}
	}

	public void RemoveItem(string tag){
 
		int temp = 0;
		if(inventory.TryGetValue(tag, out temp))
		{
			if ( temp > 0 ){
				temp --;
			}
			inventory[tag] = temp;
		}
		else
		{
			inventory[tag] = 0;
		}
	}

	public bool HasItem(string tag){
		bool ret = false;
		int temp = 0;
		if(inventory.TryGetValue(tag, out temp))
		{
			if ( temp > 0 ){
				ret =  true;
			}
		}
		return ret;
	}

	public bool HasNItem(string tag,int n){
		bool ret = false;
		int temp = 0;
		if(inventory.TryGetValue(tag, out temp))
		{
			if ( temp >= n ){
				ret =  true;
			}
		}
		return ret;
	}

	public int GetItemCount(string key){
		int ret = 0;
		int temp = 0;
		if ( inventory.TryGetValue(tag, out temp) ){
			ret = inventory [key];
		}

		return ret;
	}
 

}
