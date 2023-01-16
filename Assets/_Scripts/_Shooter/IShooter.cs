using UnityEngine;
using System.Collections;

public interface IShooter {

	void SetActive(bool isActive);

	bool IsActive();

	void DoFire();

	void DisableFor(int sceonds);
}
