using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum FacingDir{
    Right,
    Left,
    Unchanged
}

[RequireComponent(typeof(ActionManager))]
public class AnimationManager : Node
{
    [SerializeField]
    public AnimationData defaultAnim;
    [SerializeField]
    public AnimationData curAnim;

    [SerializeField]
    public Conditional flipLeft = new Conditional();   
    [SerializeField]
    public Conditional flipRight = new Conditional();

    [SerializeField]
    public List<AnimationData> anims = new List<AnimationData>();
    [SerializeField]
    public List<Conditional> animConds = new List<Conditional>();

    bool playingDefault = false;
    int animIndex = -1;

    public void Update() {
        if (flipLeft.isTrue(gameObject)) {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (flipRight.isTrue(gameObject)) {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }

            rend.sprite = curAnim.getFrame();

        bool curAnimIsTrue = animIndex != -1 && animConds[animIndex].isTrue(gameObject);
        bool changed = false;

        for(int i = 0; i < animConds.Count; ++i) {
            if (animConds[i].isTrue(gameObject)) {           
                if (curAnimIsTrue && !playingDefault && anims[i].precedence <= curAnim.precedence) {
                    continue;
                }

                curAnim = anims[i];
                playingDefault = false;
                curAnimIsTrue = true;
                changed = true;

                animIndex = i;
                curAnim.reset();
            }
        }

        if (!(changed || curAnimIsTrue || playingDefault)) {
            curAnim = defaultAnim;
            playingDefault = true;
            animIndex = -1;
        }
    }

    public override void serialize() {
        flipLeft.serialize();
        flipRight.serialize();
        foreach (var cond in animConds) {
            cond.serialize();
        }
    }

    public override void deserialize() {
        flipLeft.deserialize();
        flipRight.deserialize();
        foreach (var cond in animConds) {
            cond.deserialize();
        }
    }
}

[CustomEditor(typeof(AnimationManager))]
public class AnimationManagerEditor : Editor {
    AnimationManager action;

    private void OnEnable() {
        action = (AnimationManager)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {

        GUIUtil.CondMenu("Flip Right", action.flipRight); 
        GUIUtil.CondMenu("Flip Left", action.flipLeft); 

        EditorGUILayout.HelpBox("Handles Triggers for Animation", MessageType.None);
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultAnim"), new GUIContent("Default Animation"), true);

        GUILayout.Space(20);

        GUILayout.Label("States");

        GUILayout.Space(20);

        for (int i = 0; i < action.anims.Count; ++i) {
            GUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("anims").GetArrayElementAtIndex(i), true);

            GUILayout.EndHorizontal();

            if (serializedObject.FindProperty("anims").GetArrayElementAtIndex(i).isExpanded) {
                GUIUtil.CondMenu("Play Animation", action.animConds[i]);
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove")) {
                action.anims.RemoveAt(i);
                action.animConds.RemoveAt(i);
                --i;
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Add Animation")) {
            action.anims.Add(new AnimationData());
            action.animConds.Add(new Conditional());
        }

        serializedObject.ApplyModifiedProperties();
    }
}

[System.Serializable]
public class AnimationData {
    [SerializeField]
    public Sprite[] clips;
    [SerializeField]
    public int precedence = 0;
    [SerializeField]
    public float speed = 24;
    [SerializeField]
    bool repeat = true;
    [HideInInspector]
    public int index = 0;

    float frameIncr = 0f;

    public Sprite getFrame() {
        if (clips.Length == 0) {
            return null;
        }

        frameIncr += speed * Time.deltaTime;

        if (frameIncr >= 1f) {
            frameIncr = 0f;

            if (repeat) {
                index = (index + 1) % clips.Length;
            }
            else {
                index = Mathf.Clamp(index + 1, 0, clips.Length - 1);
            }
        }

        return clips[index];
    }

    public void reset() {
        index = 0;
    }
}