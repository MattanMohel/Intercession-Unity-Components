using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ActionManager))]
public class PlaySound : Action
{
    [SerializeField]
    [HideInInspector]
    public Conditional play = new Conditional();

    [HideInInspector]
    public AudioSource audioSource;
    
    [SerializeField]
    public AudioClip clip;

    public void Start(){
        audioSource = FindObjectOfType<AudioSource>();
    }

    public override bool condition() {

        return play.isTrue(gameObject);
    }

    public override void doAction() {
        audioSource.PlayOneShot(clip);
    }

    public override void serialize() {
        play.serialize();
    }

    public override void deserialize() {
        play.deserialize();
    }
}

[CustomEditor(typeof(PlaySound))]
public class PlaySoundEditor : Editor {
    PlaySound action;

    private void OnEnable() {
        action = (PlaySound)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Play a sound on a condition", MessageType.None);

        GUIUtil.CondMenu("Play Sound", action.play);
        base.OnInspectorGUI();
    }
}