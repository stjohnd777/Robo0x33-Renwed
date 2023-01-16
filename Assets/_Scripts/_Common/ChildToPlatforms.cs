using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is on the character
public class ChildToPlatforms : MonoBehaviour {


    public string[] prejudiceTagList = { "MovingPlatform" };

    // if the player collides with a MovingPlatform, then make it a child of that platform
    // so it will go for a ride on the MovingPlatform
    void OnCollisionEnter2D(Collision2D other)
    {
        if (IsInPrejudiceTagList (other.gameObject.tag ))
        {
            this.transform.parent = other.transform;
        }
    }

	void OnCollision2D(Collision2D other)
	{
		if (IsInPrejudiceTagList (other.gameObject.tag ))
		{
			this.transform.parent = other.transform;
		}
	}

    // if the player exits a collision with a moving platform, then unchild it
    void OnCollisionExit2D(Collision2D other)
    {
        if (IsInPrejudiceTagList (other.gameObject.tag))
        {
            this.transform.parent = null;
        }
    }

    public bool IsInPrejudiceTagList(string tag)
    {
        bool ret = false;
        foreach (string prejudiceTag in prejudiceTagList)
        {
            if (tag == prejudiceTag)
            {
                ret = true;
                return ret;
            }
        }
        return ret;
    }
}
