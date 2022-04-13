using UnityEditor;
using UnityEngine;

public enum Direction {
    Right,
    Down,
    Left,
    Up,
}

[RequireComponent(typeof(ActionManager))]
public class Move : Action {
    [SerializeField]
    public ConditionalManager move = new ConditionalManager();

    [SerializeField]
    public Direction direction;
    [SerializeField] 
    public float speed;
    [SerializeField]
    public float acceleration;

    public override bool condition() {
        return move.evaluate(gameObject);
    }

    public override void doAction() {
        switch (direction) {
            case Direction.Right:
                rb.velocity = new Vector2(Mathf.Clamp(acceleration * Time.deltaTime + rb.velocity.x, 0, speed), rb.velocity.y);
                break;
            case Direction.Left:
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - acceleration * Time.deltaTime, -speed, 0), rb.velocity.y);
                break;
            case Direction.Up:
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(acceleration * Time.deltaTime + rb.velocity.x, 0, speed));
                break;
            case Direction.Down:
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.x - acceleration * Time.deltaTime, -speed, 0));
                break;
        }
    }

    public override void serialize() {
        ConditionalManager.Serialize(ref move);
    }

    public override void deserialize() {
        move.deserialize();
    }
}

[CustomEditor(typeof(Move))]
public class MoveEditor : Editor {
    Move action;

    private void OnEnable() {
        action = (Move)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Control the horizontal movement of an object", MessageType.None);

        GUIUtil.CondMenu("Move " + action.direction.ToString(), action, ref action.move);

        action.direction = (Direction)EditorGUILayout.EnumPopup("Direction", action.direction);

        action.acceleration = EditorGUILayout.FloatField("Acceleration", action.acceleration);
        action.speed = EditorGUILayout.FloatField("Maximum Speed", action.speed);
    }
}
