using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ActionManager))]
public class Enable : Action {
    [SerializeField]
    public Conditional destroy = new Conditional();

    [SerializeField]
    public bool enable;

    public override bool condition() {
        return destroy.isTrue(gameObject);
    }

    public override void doAction() {
        gameObject.SetActive(enable);
    }

    public override void serialize() {
        destroy.serialize();
    }

    public override void deserialize() {
        destroy.deserialize();
    }
}

[CustomEditor(typeof(Enable))]
public class EnableEditor : Editor {
    Enable action;

    private void OnEnable() {
        action = (Enable)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Control if object is enabled or disabled", MessageType.None);

        string label = action.enable ? "Enable" : "Disable";


        GUIUtil.CondMenu(label, action.destroy);
        action.enable = EditorGUILayout.Toggle(action.enable);
    }
}
