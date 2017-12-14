using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour {

    [SerializeField] int MaxRockets = 2;
    [SerializeField] Rocket RocketPrefab;

    private int RocketCount;

	// Use this for initialization
	void Start () {
        RocketCount = MaxRockets;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire3") && RocketCount > 0)
        {
            Instantiate(RocketPrefab);
            RocketCount--;
        }
	}
}
