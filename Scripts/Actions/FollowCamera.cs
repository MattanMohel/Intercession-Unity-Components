using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ActionManager))]
public class FollowCamera : Action {
    [SerializeField]
    public Conditional follow = new Conditional();

    [SerializeField]
    public float speed = 0;

    public override bool condition() {
        return follow.isTrue(gameObject);
    }

    public override void doAction() {
        Vector3 vec = Vector3.Lerp(cam.transform.position, transform.position, speed);
        cam.transform.position = new Vector3(vec.x, vec.y, cam.transform.position.z);
    }

    public override void serialize() {
        follow.serialize();
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

        GUIUtil.CondMenu("Follow", action.follow);

        action.speed = EditorGUILayout.FloatField("Follow Speed", action.speed);
    }
}