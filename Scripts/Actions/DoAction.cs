using UnityEngine;
using UnityEditor;

public class DoAction : Action
{
    [SerializeField]
    public Action action;
    [SerializeField]
    [HideInInspector]
    public Conditional cond;

    public override bool condition() {
        return cond.isTrue(gameObject);
    }

    public override void doAction() {
        action.doAction();
    }

    public override void serialize() {
        cond.serialize();
    }

    public override void deserialize() {
        cond.deserialize();
    }
}

[CustomEditor(typeof(DoAction))]
public class DoActionEditor : Editor {
    DoAction action;

    private void OnEnable() {
        action = (DoAction)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Does an action given a condition", MessageType.None);

        GUIUtil.CondMenu("Do Action", action.cond);
        base.OnInspectorGUI();
    }
}
