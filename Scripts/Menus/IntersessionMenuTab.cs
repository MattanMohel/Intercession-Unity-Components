using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class IntersessionMenuTab : MonoBehaviour
{
    [MenuItem("Intersession/Create Canvas")]
    public static void createCanvas(){
        Object prefab = Resources.Load("Canvas");
        Camera cam = FindObjectOfType<Camera>();
        var inst = (GameObject) Instantiate(prefab, cam.transform.position, cam.transform.rotation);
        Canvas canvas = inst.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
    }

    [MenuItem("Intersession/Create Tilemap")]
    public static void createTilemap(){
        Object prefab = Resources.Load("Grid");
        var inst = (GameObject) Instantiate(prefab, new Vector3(0,0,0), new Quaternion()); 
    }
}