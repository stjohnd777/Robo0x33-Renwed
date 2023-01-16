using UnityEngine;
using System.Collections;

#if UNITY_EDITOR 
using UnityEditor;
#endif

public class Walk : AbstractBehaviour//, IFacingRight
{

    public float HorizontalSpeed = 50;

    public float HorizontalSpeedMultiplier = 2f;

    public bool Running = false;
 
    void Update()
    {

        Running = false;

        bool right = inputState.GetButtonValue(inputsButtons[0]);

        bool left = inputState.GetButtonValue(inputsButtons[1]);

        bool sprint = inputState.GetButtonValue(inputsButtons[2]);

        float velocityX = 0;
        if (right || left)
        {
            velocityX = HorizontalSpeed;
            if (sprint && HorizontalSpeedMultiplier > 0)
            {
                velocityX = velocityX * HorizontalSpeedMultiplier;
                Running = true;
            }
            velocityX *= (float)inputState.direction;
        }

        Vector2 newVelocity = new Vector2(velocityX, body2d.velocity.y);
        body2d.velocity = newVelocity;

    }
 
}

#if UNITY_EDITOR
[CustomEditor(typeof(Walk))]
class WalkEditor : AbstractBehaviorEditor
{
    Walk walk;

    public void OnEnable()
    {
        base.OnEnable();
        walk = target as Walk;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawDefaultInspector();

        serializedObject.Update();
    }
}
#endif