using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ActionManager))]
public class TimerTrigger : Trigger {
    [SerializeField]
    public float waitTime = 0f;

    float counter = 0f;

    public override void Update() {
        counter += Time.deltaTime;

        if (counter >= waitTime) {
            counter = 0;
            state = true;
        }

        base.Update();
    }
}

[CustomEditor(typeof(TimerTrigger))]
public class TimerEditor : Editor {
    TimerTrigger action;

    private void OnEnable() {
        action = (TimerTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Trigger every " + action.waitTime + " seconds", MessageType.None);
        base.OnInspectorGUI();
    }
}