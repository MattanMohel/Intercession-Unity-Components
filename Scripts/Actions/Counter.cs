using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[ExecuteAlways]
public class Counter : MonoBehaviour
{
    [SerializeField]
    public string strName;

    [SerializeField]
    public int counter;

    public Text text;

    [ExecuteAlways]
    private void Update() {
        if (text != null)
        text.text = strName + counter.ToString();
    }

    public void Start(){
        text = GetComponent<Text>();
        updateCounter(0);
    }

    public void updateCounter(int update){
        counter += update;
    }

    public void setCounter(int set){
        counter = set;
    }

}

[CustomEditor(typeof(Counter))]
public class CounterEditor : Editor {
    Counter action;

    private void OnEnable() {
        action = (Counter)target;
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("A Counter that keeps track of some value", MessageType.None);

        action.strName = EditorGUILayout.TextField("Counter Name", action.strName);
        action.counter = EditorGUILayout.IntField("Starting Value", action.counter);
    }
}