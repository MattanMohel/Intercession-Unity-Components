using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ActionManager))]
public class DestroyOnCond : Action
{
    [SerializeField]
    [HideInInspector]
    public ConditionalManager destroyCond = new ConditionalManager();

    public override bool condition() {
        return destroyCond.evaluate(gameObject);
    }

    public override void doAction() {
        Destroy(gameObject);
    }

    public override void serialize() {
        ConditionalManager.Serialize(ref destroyCond);
    }

    public override void deserialize() {
        destroyCond.deserialize();
    }
}

[CustomEditor(typeof(DestroyOnCond))]
public class DestroyOnCondEditor : Editor {
    DestroyOnCond action;

    private void OnEnable() {
        action = (DestroyOnCond)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Destroys the Object on a Condition", MessageType.None);

        GUIUtil.CondMenu("Destroy", action, ref action.destroyCond);
        base.OnInspectorGUI();
        if (GUI.changed) { EditorUtility.SetDirty(action); }
        
        Undo.RecordObject(action, "");
    }
}
