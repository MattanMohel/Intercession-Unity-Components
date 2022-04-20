using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ActionManager))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Oscillator : Action {
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
            transform.position = Vector3.MoveTowards(transform.position, transforms[curTransform].position, speed * Time.deltaTime);
        }
    }

    public override void serialize() {
        move.serialize();
    }

    public override void deserialize() {
        move.deserialize();
    }
}

[CustomEditor(typeof(Oscillator))]
public class OscillatorEditor : Editor {
    Oscillator action;

    private void OnEnable() {
        action = (Oscillator)target;
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
