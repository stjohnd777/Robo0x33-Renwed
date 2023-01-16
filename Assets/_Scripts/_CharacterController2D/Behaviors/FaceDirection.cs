using UnityEngine;
using System.Collections;

public class FaceDirection : AbstractBehaviour, IFacingRight
{
    public bool isFacingRight = true;

    //public GameObject target;

 
    void Start()
    {
        //target.transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    void LateUpdate () {

		if (inputState.direction == Directions.Right) {
			isFacingRight = true;
			transform.localScale = new Vector3(  Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}
		if (inputState.direction == Directions.Left) {
			isFacingRight = false;
			transform.localScale = new Vector3( -Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}

        //FaceCorrectDirection(inputState.HInput);

        //if (inputState.IsRight)
        //{
        //    isFaceingRight = true;
        //    target.transform.localScale = new Vector3(  Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //}
        //if (inputState.IsLeft)
        //{
        //    isFaceingRight = false;
        //    target.transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //}

        //target.transform.localScale = new Vector3(-1 * transform.localScale.x , transform.localScale.y, transform.localScale.z);
    }

    public void FaceCorrectDirection(float valueH)
    {
        if ((inputState.HInput < 0 && isFacingRight) || (inputState.HInput > 0 && !isFacingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }


    public bool IsFacingRight()
	{
        return isFacingRight;
	}
}
