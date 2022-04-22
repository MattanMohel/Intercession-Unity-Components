using UnityEditor;
using UnityEngine;

public enum CollisionType {
    Enter, 
    Exit, 
    Stay,
}

[RequireComponent(typeof(ActionManager))]
public class CollisionTrigger : Trigger
{
    [SerializeField]
    public Collider2D usedCollider;

    [SerializeField]
    private CollisionType collisionType;

    [SerializeField]
    [HideInInspector]
    public Conditional collisionCond = new Conditional();

    public override void serialize() {
        collisionCond.serialize();
    }

    public override void deserialize() {
        collisionCond.deserialize();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collisionType == CollisionType.Enter &&
            collision.otherCollider == usedCollider &&
            collisionCond.isTrue(collision.gameObject)) {

            state = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collisionType == CollisionType.Exit && 
            collision.otherCollider == usedCollider &&
            collisionCond.isTrue(collision.gameObject)) {

            state = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collisionType == CollisionType.Stay &&
            collision.otherCollider == usedCollider &&
            collisionCond.isTrue(collision.gameObject)) {

            state = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collisionType == CollisionType.Enter &&
            collisionCond.isTrue(collider.gameObject)) {

            state = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collisionType == CollisionType.Exit &&
            collisionCond.isTrue(collider.gameObject)) {

            state = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collider) {
        if (collisionType == CollisionType.Stay &&
            collisionCond.isTrue(collider.gameObject)) {

            state = true;
        }
    }
}

[CustomEditor(typeof(CollisionTrigger))]
public class CollisionEditor : Editor {
    CollisionTrigger action;

    private void OnEnable() {
        action = (CollisionTrigger)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Trigger on collison", MessageType.None);

        GUIUtil.CondMenu("Respond to Other Collider", action.collisionCond);
        base.OnInspectorGUI();
    }
}
