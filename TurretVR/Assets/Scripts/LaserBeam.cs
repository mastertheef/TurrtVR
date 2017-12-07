using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

    private Renderer renderer;
    [SerializeField] private float speed = 0.5f;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        renderer.material.SetTextureOffset("_MainTex", new Vector2(-1 * speed * Time.time, 0));
	}
}
