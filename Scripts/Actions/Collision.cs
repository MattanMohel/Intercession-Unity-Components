using UnityEditor;
using UnityEngine;

public enum CollisionType {
    Enter, 
    Exit, 
    Stay,
}

[RequireComponent(typeof(ActionManager))]
public class Collision : Trigger
{
    [SerializeField]
    public Collider2D usedCollider;

    [SerializeField]
    private CollisionType collisionType;

    [SerializeField]
    [HideInInspector]
    public ConditionalManager collisionCond = new ConditionalManager();  

    public override void serialize() {
        ConditionalManager.Serialize(ref collisionCond);
    }

    public override void deserialize() {
        collisionCond.deserialize();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collisionType == CollisionType.Enter &&
            collision.otherCollider == usedCollider &&
            collisionCond.evaluate(collision.gameObject)) {

            state = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collisionType == CollisionType.Exit && 
            collision.otherCollider == usedCollider &&
            collisionCond.evaluate(collision.gameObject)) {

            state = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collisionType == CollisionType.Stay &&
            collision.otherCollider == usedCollider &&
            collisionCond.evaluate(collision.gameObject)) {

            state = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collisionType == CollisionType.Enter &&
            collisionCond.evaluate(collider.gameObject)) {

            state = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collisionType == CollisionType.Exit &&
            collisionCond.evaluate(collider.gameObject)) {

            state = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collider) {
        if (collisionType == CollisionType.Stay &&
            collisionCond.evaluate(collider.gameObject)) {

            state = true;
        }
    }
}

[CustomEditor(typeof(Collision))]
public class CollisionEditor : Editor {
    Collision action;

    private void OnEnable() {
        action = (Collision)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Trigger on collison", MessageType.None);

        GUIUtil.CondMenu("Respond to Other Collider", action, ref action.collisionCond);
        base.OnInspectorGUI();
    }
}
