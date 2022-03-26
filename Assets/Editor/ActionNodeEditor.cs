using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionNode))]
public class ActionNodeEditor : Editor {
    ActionType action_type = ActionType.NONE;
    ActionNode action;

    Key actionKeyAlias = Key.NONE;
    private void OnEnable() {
        action_type = ActionType.NONE;
        action = (ActionNode)target;
    }
    public override void OnInspectorGUI() {

        GUILayout.BeginHorizontal();
        GUILayout.Label("Key:");
        action.key = (KeyCode)EditorGUILayout.EnumPopup(actionKeyAlias);
        GUILayout.Label("Action");
        actionKeyAlias = (Key)action.key;

        ActionType new_action = (ActionType)EditorGUILayout.EnumPopup(action_type);
        GUILayout.EndHorizontal();

        // Action Mutation

        if (new_action != action_type) {
            action_type = new_action;

            switch (action_type) {
                case ActionType.NONE:
                    action.action = null;
                    break;
                case ActionType.JUMP:
                    action.action = new Jump();
                    break;
                case ActionType.MOVE:
                    action.action = new Move();
                    break;
            }
        }

        // Action Serialization

        switch (action_type) {
            case ActionType.NONE:
                EditorGUILayout.HelpBox("Empty Action", MessageType.None);

                break;

            case ActionType.JUMP:
                Jump jump = (Jump)action.action;

                EditorGUILayout.HelpBox("Control the height properties of an object", MessageType.None);

                jump.jump_height   = EditorGUILayout.Slider("Height", jump.jump_height, 0.0f, 10.0f);
                jump.jump_duration = EditorGUILayout.Slider("Duration", jump.jump_duration, 0.0f, 10.0f);

                break;

            case ActionType.MOVE:
                Move move = (Move)action.action;

                EditorGUILayout.HelpBox("Control side movement of an object", MessageType.None);

                move.dir = (Direction)EditorGUILayout.EnumPopup("Movement Direction", move.dir);
                move.speed = EditorGUILayout.Slider("Speed", move.speed, 0.0f, 10.0f);
                move.acceleration = EditorGUILayout.Slider("Acceleration", move.acceleration, 0.0f, 10.0f);

                break;
        }
    }
}
