using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ActionManager))]
public class FollowCamera : Action {
    [SerializeField]
    public ConditionalManager follow = new ConditionalManager();

    public override bool condition() {
        return follow.evaluate(gameObject);
    }

    public override void doAction() {
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
    }

    public override void serialize() {
        ConditionalManager.Serialize(ref follow);
    }

    public override void deserialize() {
        follow.deserialize();
    }
}

[CustomEditor(typeof(FollowCamera))]
public class FollowCameraEditor : Editor {
    FollowCamera action;

    private void OnEnable() {
        action = (FollowCamera)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Make camera follow object", MessageType.None);

        GUIUtil.CondMenu("Follow", action, ref action.follow);
    }
}