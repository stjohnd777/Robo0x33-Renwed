using UnityEngine;
using System.Collections;

#if UNITY_EDITOR 
using UnityEditor;
#endif

public class SlashAttack : AbstractBehaviour
{

    public GameObject slasher;

    private State state;

    private void Awake()  
    {
        base.Awake();
        if (slasher == null)
        {
            slasher = GameObject.FindGameObjectWithTag("Mele1");
        }
        slasher.SetActive(false);

        if (state == null)
        {
            state = GetComponent<State>();
        }
    }

    void Update () {

        bool IsRequesting = IsRequestingBehavior();

        if (IsRequesting)
        {
            SetRelatedBehaviorsToActive(false);
            slasher.SetActive(true);
            if (state != null)
            {
                state.IsSlashing = true;
            }
        }
        else
        {
            SetRelatedBehaviorsToActive(true);
            slasher.SetActive(false);

            if (state != null)
            {
                state.IsSlashing = false;
            }
        }

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SlashAttack))]
class SlashEditor : AbstractBehaviorEditor
{
    SlashAttack slashAttack;

    public void OnEnable()
    {
        base.OnEnable();
        slashAttack = target as SlashAttack;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawDefaultInspector();

        serializedObject.Update();
    }
}
#endif


