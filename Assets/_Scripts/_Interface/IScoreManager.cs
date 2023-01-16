using UnityEngine;
using System.Collections;

public interface IScoreManager{

	void AddPoints(int amount);

	int GetPoint();

	void RemovePoints(int amount);
}
