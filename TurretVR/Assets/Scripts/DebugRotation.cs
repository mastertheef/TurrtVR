using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugRotation : MonoBehaviour {

    [SerializeField] private Text label;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        label.text = gameObject.tag + " x: " + Camera.main.transform.rotation.x + ", " +
           "y: " + Camera.main.transform.rotation.y + ", " +
           "z: " + Camera.main.transform.rotation.z;

    }
}
