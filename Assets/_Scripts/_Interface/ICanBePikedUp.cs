using UnityEngine;
using System.Collections;

public interface ICanBePikedUp  {
 
	bool IsTaken();

	void SetTaken(bool taken);

	void PickUp(string itemKey,int numberItems, int itemValue);
}
