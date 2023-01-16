using UnityEngine;
using System.Collections;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class StabAttack : AbstractBehaviour
{

	public bool IsRequestingStab ;

    public GameObject stabber;

    private State state;

   private void Awake()
    {
        base.Awake();
       
        if (state == null)
        {
            state = GetComponent<State>();
        }

        if (stabber == null)
        {
            stabber = GameObject.FindGameObjectWithTag("Mele2");
        }

        stabber.SetActive(false);
    }

    void Update () {

       IsRequestingStab = IsRequestingBehavior();

        if (IsRequestingStab)
        {
            SetRelatedBehaviorsToActive (false);

            stabber.SetActive(true); 

            if (state != null)
            {
                state.IsStabbing = true;
            }

        } else
        {
            SetRelatedBehaviorsToActive (true);

            stabber.SetActive(false);

            if (state != null)
            {
                state.IsStabbing = false;
            }
        }
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(StabAttack))]
class StabEditor : AbstractBehaviorEditor
{
    StabAttack stabAttack;

    public void OnEnable()
    {
        base.OnEnable();
        stabAttack = target as StabAttack;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawDefaultInspector();

        serializedObject.Update();
    }
}
#endif