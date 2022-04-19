using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ActionManager))]
public class DistanceTo : Trigger {
    [SerializeField]
    public GameObject other;

    [HideInInspector]
    [SerializeField]
    public float distance;

    [HideInInspector]
    [SerializeField]
    public Cmp cmp;

    new private void Update() {
        if (ConditionalManager.ApplyOp(Vector2.Distance(transform.position, other.transform.position), distance, cmp)) {
            state = true;
        }

        base.Update();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}

[CustomEditor(typeof(DistanceTo))]
public class DistanceToEditor : Editor {
    DistanceTo action;

    private void OnEnable() {
        action = (DistanceTo)target;
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
