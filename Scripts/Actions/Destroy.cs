using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ActionManager))]
public class Destroy : Action
{
    [SerializeField]
    [HideInInspector]
    public Conditional destroyCond = new Conditional();

    public override bool condition() {
        return destroyCond.isTrue(gameObject);
    }

    public override void doAction() {
        Destroy(gameObject);
    }

    public override void serialize() {
        destroyCond.serialize();
    }

    public override void deserialize() {
        destroyCond.deserialize();
    }
}

[CustomEditor(typeof(Destroy))]
public class DestroyOnCondEditor : Editor {
    Destroy action;

    private void OnEnable() {
        action = (Destroy)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Destroys the Object on a Condition", MessageType.None);

        GUIUtil.CondMenu("Destroy", action.destroyCond);
        base.OnInspectorGUI();
        if (GUI.changed) { EditorUtility.SetDirty(action); }
        
        Undo.RecordObject(action, "");
    }
}
