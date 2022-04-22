using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ActionManager))]
public class DistanceTrigger : Trigger {
    [SerializeField]
    public GameObject other;

    [HideInInspector]
    [SerializeField]
    public float distance;

    [HideInInspector]
    [SerializeField]
    public Cmp cmp;

    new private void Update() {
        if (Conditional.ApplyOp(Vector2.Distance(transform.position, other.transform.position), distance, cmp)) {
            state = true;
        }

        base.Update();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}

[CustomEditor(typeof(DistanceTrigger))]
public class DistanceToEditor : Editor {
    DistanceTrigger action;

    private void OnEnable() {
        action = (DistanceTrigger)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Trigger when distance between objects meets requirement", MessageType.None);

        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();

        action.cmp = (Cmp)EditorGUILayout.EnumPopup("Distance", action.cmp);
        action.distance = EditorGUILayout.FloatField(action.distance);

        GUILayout.EndHorizontal();
    }
}
