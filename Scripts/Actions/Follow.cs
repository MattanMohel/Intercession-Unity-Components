using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ActionManager))]
public class Follow : Action {
    [SerializeField]
    public bool followOnXAxis = true;

    [SerializeField]
    public bool followOnYAxis = true;    

    [SerializeField]
    public List<Transform> transforms = new List<Transform>();

    [SerializeField]
    [HideInInspector]
    public float speed;

    [SerializeField]
    [HideInInspector]
    public float precision;

    [SerializeField]
    [HideInInspector]
    public Conditional move = new Conditional();

    int curTransform = 0;

    public override bool condition() {
        return move.isTrue(gameObject);
    }

    public override void doAction() {
        if (Vector2.Distance(transform.position, transforms[curTransform].position) < precision) {
            curTransform = (curTransform + 1) % transforms.Count;
        } else  {
            var delta = Vector3.MoveTowards(transform.position, transforms[curTransform].position, speed * Time.deltaTime) - transform.position;
            int followX = followOnXAxis? 1 : 0;
            int followY = followOnYAxis? 1 : 0;

            transform.position = new Vector3(transform.position.x + delta.x * followX, transform.position.y + delta.y * followY, delta.z);
        }
    }

    public override void serialize() {
        move.serialize();
    }

    public override void deserialize() {
        move.deserialize();
    }
}

[CustomEditor(typeof(Follow))]
public class FollowEditor : Editor {
    Follow action;

    private void OnEnable() {
        action = (Follow)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Move object between set points", MessageType.None);

        base.OnInspectorGUI();

        GUIUtil.CondMenu("Move", action.move);

        action.speed = EditorGUILayout.FloatField("Speed", action.speed);

        action.precision = EditorGUILayout.FloatField(new GUIContent("Precison",
            "The distance from the target needed to satisfy arrival at destination"), action.precision);
    }
}
