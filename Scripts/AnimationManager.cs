using System.Collections;
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
    public AnimationData defaultAnimation;

    [SerializeField]
    public List<AnimationData> data = new List<AnimationData>();

    SpriteRenderer rend;

    AnimationData curAnim;

    bool playingDefault = true;

    public void Start(){
        rend = GetComponent<SpriteRenderer>();
        curAnim = defaultAnimation;
    }

    public void FixedUpdate(){
        bool change = false;

        foreach(AnimationData info in data) {
            if(info.condition.evaluate(gameObject)) {
                curAnim.reset();
                playingDefault = false; 
                curAnim = info;
                change = true;
                break;
            }
        }

        if(!change && !playingDefault) {
            curAnim.reset();
            curAnim = defaultAnimation;
        }

        rend.sprite = curAnim.getFrame();
    }

    public override void serialize() {
        foreach(AnimationData info in data){
            ConditionalManager.Serialize(ref info.condition);
        }
    }

    public override void deserialize() {
        foreach(AnimationData info in data){
            info.condition.deserialize();
        }
    }
}

// [CustomEditor(typeof(AnimationManager))]
// public class AnimationManagerEditor : Editor {
//     AnimationManager action;

//     private void OnEnable() {
//         action = (AnimationManager)target;
//         action.deserialize();
//     }

//     public override void OnInspectorGUI() {
//         EditorGUILayout.HelpBox("Handles Triggers for Animation", MessageType.None);

//         //action.defaultAnimation = (AnimationClip) EditorGUILayout.ObjectField("Default Animation", action.defaultAnimation, typeof(AnimationClip), true);
//         EditorGUILayout.PropertField("")

//         GUILayout.Space(10);

//         for (int i = 0; i < action.data.Count; ++i) {
//             GUILayout.BeginHorizontal();
            
//             action.data[i].clip= (AnimationClip) EditorGUILayout.ObjectField("Animation", action.data[i].clip, typeof(AnimationClip), true);

//             GUILayout.EndHorizontal();

//             GUIUtil.CondMenu("Play Animation", action, ref action.data[i].condition);

            
//             GUILayout.BeginHorizontal();
//             if (GUILayout.Button("Remove")) {
//                 action.data.RemoveAt(i);
//                 --i;
//             }

//             GUILayout.EndHorizontal();
//         }

//         GUILayout.Space(5);

//         if (GUILayout.Button("Add Animation")) {
//             action.data.Add(new AnimationData());
//         }

//     }
// }

[System.Serializable]
public class AnimationData {
    [SerializeField]
    public Sprite[] clip;
    int index = 0;

    [SerializeField]
    public ConditionalManager condition = new ConditionalManager();

    public void reset() {
        index = 0;
    }

    public Sprite getFrame() {
        index = (index + 1) % clip.Length;
        return clip[index];
    }
}