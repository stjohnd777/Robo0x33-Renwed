using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasValue : MonoBehaviour , IHasValue
{

    public int value;

    public bool taken= false;

    public int GetValue()
    {
        return value;
    }

    public void SetValue(int value)
    {
        this.value = value;
    }

    public bool IsTaken()
    {
        return taken;
    }

    public int Take()
    {
        taken = true;
        return value;
    }


}
