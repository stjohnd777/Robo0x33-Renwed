using UnityEngine;
using System.Collections;

public interface IInventoryManager   {

  void AddInventory(string type, int amount );

	void AddInventory(string type, int amount, AudioClip sfx);

  int GetInventory(string type);

  void RemoveInventory(string type, int amount ) ;

}
