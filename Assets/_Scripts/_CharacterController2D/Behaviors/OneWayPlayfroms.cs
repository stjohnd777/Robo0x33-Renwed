 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WhatWay {
    UP,
    Down,
    Right,Left
}
public class OneWayPlayfroms : MonoBehaviour {


	//public LayerMask WhatIsOneWay;

    public string OneWayPlatformLayerName =  "OneWayPlatform" ;

    public WhatWay direction = WhatWay.UP;

	// store the layer the player is on (setup in Awake)
	int playerLayerNumber;

	Rigidbody2D body2D;

	int oneWay;

	void Awake () {
		playerLayerNumber = LayerMask.NameToLayer("Player");

		body2D = GetComponent<Rigidbody2D> ();
		if (body2D==null){ 
			Debug.LogError("Rigidbody2D component missing OneWayPlatformLayerName");
		}

		oneWay  = LayerMask.NameToLayer(OneWayPlatformLayerName);
	}
 
	void Update () {

        bool deactivate = false;
        switch (direction)
        {
            case WhatWay.UP:
                deactivate = (body2D.velocity.y > 0);
                break;
            case WhatWay.Down:
                deactivate = (body2D.velocity.y < 0);
                break;
            case WhatWay.Right:
                deactivate = (body2D.velocity.x > 0);
                break;
            case WhatWay.Left:
                deactivate = (body2D.velocity.x < 0);
                break;
            default:
                 
                break;
        }
        Physics2D.IgnoreLayerCollision  (playerLayerNumber, oneWay, deactivate);
	}
}
