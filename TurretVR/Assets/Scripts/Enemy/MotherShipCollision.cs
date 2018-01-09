﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponentInParent<MotherShip>().Collision(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        gameObject.GetComponentInParent<MotherShip>().CollisionExit(collision);
    }
}
