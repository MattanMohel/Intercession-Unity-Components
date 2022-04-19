using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ActionManager))]
public class Spawner : Action
{
    [SerializeField]
    [HideInInspector]
    public ConditionalManager spawn = new ConditionalManager();
    
    [SerializeField]
    public Object spawnObj;
    [SerializeField]
    public float xOffset;
    [SerializeField]
    public float yOffset;

    public override bool condition() {
        return spawn.evaluate(gameObject);
    }

    public override void doAction() {
        Vector3 loc = transform.position;
        loc.x += xOffset;
        loc.y += yOffset;
        GameObject obj = (GameObject) Instantiate(spawnObj, loc, transform.rotation);
    }

    public override void serialize() {
        ConditionalManager.Serialize(ref spawn);
    }

    public override void deserialize() {
        spawn.deserialize();
    }
}

[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor {
    Spawner action;

    private void OnEnable() {
        action = (Spawner)target;
        action.deserialize();
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Spawns An Object given a Condition", MessageType.None);

        GUIUtil.CondMenu("Spawn", action, ref action.spawn);
        base.OnInspectorGUI();
    }
}
