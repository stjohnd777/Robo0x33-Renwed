using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasValue  {

    // Use this for initialization
    int GetValue();

    void SetValue(int value);

    bool IsTaken();

    int Take();
}
