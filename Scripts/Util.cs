using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using System.IO;
using System.Linq;

public static class GUIUtil {
    public static void SerCode(string label, ref KeyCode code) {
        code = (KeyCode)EditorGUILayout.EnumPopup(label, (KeyAlias)code);
    }
    public static void SerCmp(ref Cmp cmp) {
        cmp = (Cmp)EditorGUILayout.EnumPopup(cmp);
    }
    public static void SerFloat(ref float val) {
        val = EditorGUILayout.FloatField(val);
    }

    public static GUIStyle style = new GUIStyle { richText = true };

    public static string WithStyle(string src) {
        string indent = "";
        for (int i = 0; i < EditorGUI.indentLevel; i++) {
            indent += " ";
        }

        return string.Format("<size=15><color=silver><b>{0}{1}</b></color></size>", indent, src);
    }

    public static void CondMenu(string label, Conditional manager) {
        EditorGUI.indentLevel+=5;

        GUILayout.Label(WithStyle(label + " If"), style);

        EditorGUI.indentLevel-=2;

        manager.serialize();

        for (int i = 0; i < manager.conds.Count; i++) {
            var cond = manager.conds[i];

            GUILayout.BeginHorizontal();

            if (!manager.editing) {

                manager.states[i] = (BoolState)EditorGUILayout.EnumPopup(manager.states[i], GUILayout.Height(20));

                cond.display();
                GUILayout.Space(25);


                if (i + 1 < manager.conds.Count) {
                    manager.ops[i] = (Op)EditorGUILayout.EnumPopup(manager.ops[i], GUILayout.Height(20));
                }
                else if (!Application.isPlaying && GUILayout.Button("Edit")) { 
                    manager.editing = true;
                }
            }

            else {
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                State newState = (State)EditorGUILayout.EnumPopup(cond.state());  

                if (GUILayout.Button("Delete")) {
                    if (manager.conds.Count == 1) {
                        Debug.LogWarning("Cannot remove conditional - must have at least one condition!");

                        EditorGUI.indentLevel -= 3;
                        return;
                    }

                    manager.conds.RemoveAt(i);
                    manager.states.RemoveAt(i);
                    manager.ops.RemoveAt(i);

                    EditorGUI.indentLevel -= 3;
                    return;
                }

                if (newState != cond.state()) {
                    manager.conds[i] = Conditional.CreateFromState(newState);            
                }

                GUILayout.Space(20);
            }

            GUILayout.EndHorizontal();
        }

        // If empty
        if (manager.conds.Count == 0 && GUILayout.Button("Add")) {
            manager.conds.Add(new False());
            manager.states.Add(BoolState.True);
            manager.ops.Add(Op.And);
        }

        else if (manager.editing) {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add")) {
                manager.conds.Add(new False());
                manager.states.Add(BoolState.True);
                manager.ops.Add(Op.And);
            }
            if (GUILayout.Button("Finish")) {
                manager.editing = false;
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);
        EditorGUI.indentLevel-=3;
    }

    public static void tagMenu(ref List<int> tags) {
        for (int i = 0; i < tags.Count; ++i) {
            GUILayout.BeginHorizontal();

            int cur = EditorGUILayout.Popup(tags[i], IOUtil.Tags());

            for (int j = 0; j < tags.Count; ++j) {
                if (j != i && tags[j] == cur) {
                    Debug.LogWarning(string.Format("Object already has tag '{0}'", IOUtil.Tags()[i]));
                    return;
                }
            }

            tags[i] = cur;

            if (GUILayout.Button("Remove")) {
                tags.RemoveAt(i);
                --i;
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Add Tag")) {
            int defaultTag = 0;

            for (int i = 0; i < tags.Count; ++i) {
                if (tags[i] == defaultTag) {
                    defaultTag++;
                    i = 0;
                }
            }

            if (defaultTag == IOUtil.Tags().Length - 1) {
                Debug.LogWarning("Object already contains all possible tags");
            }
            else {
                tags.Add(defaultTag);
            }
        }

        GUILayout.Space(20);
    }
}

public static class IOUtil {
    // resources directory name
    const string resources = "Resources";
    // tag text-file name
    const string tagsPath = "tags.txt";
    // stores project tags
    private static List<string> tags = new List<string>();

    public static string[] Tags() {
        return File.ReadAllLines(IntoPath(true, tagsPath));
    }

    // asserts path exists and creates necessary parts if it doesn't
    public static void AssertPath(string path) {
        var partial = Application.dataPath + "/" + resources;

        if (!Directory.Exists(partial)) {
            Directory.CreateDirectory(partial);
        }

        if (!File.Exists(IntoPath(true, path))) {
            File.Create(IntoPath(true, path));
        }
    }

    // adds tag to designated file
    public static bool AddTag(string tag) {
        AssertPath(tagsPath);
        tags = File.ReadAllLines(IntoPath(true, tagsPath)).ToList();

        if (tags.Contains(tag)) {
            return false;
        }

        tags.Add(tag);
        File.WriteAllLines(IntoPath(true, tagsPath), tags);

        return true;
    }

    // convert parts of path with '\\'
    public static string IntoPath(bool relative = true, params string[] paths) {
        string path = string.Empty;

        if (relative) {
            path += Application.dataPath + "/" + resources;
        }
        foreach (string p in paths) {
            path += "/" + p;
        }

        return path;
    }
}