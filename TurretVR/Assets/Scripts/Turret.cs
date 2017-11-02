using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [SerializeField] private Projectile Shot;
    [SerializeField] private GameObject CannonLeft;
    [SerializeField] private GameObject CannonRight;

    [SerializeField] private float shootCounter;
    private float leftShootCounter;
    private float rightShootCounter;

    private bool leftShoot = false;
    private bool rightShoot = false;



	// Use this for initialization
	void Start () {
        SetStartShootCounters();
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SetStartShootCounters();
        }

        if (Input.GetMouseButton(0))
        {
            leftShootCounter -= Time.deltaTime;
            rightShootCounter -= Time.deltaTime;

            if (leftShootCounter <= 0)
            {
                leftShoot = true;
                leftShootCounter = shootCounter;
            }

            if (rightShootCounter <= 0)
            {
                rightShoot = true;
                rightShootCounter = shootCounter;
            }
        }

        if (leftShoot)
        {
            FireProjectile(CannonLeft);
            leftShoot = false;
        } 

        if (rightShoot)
        {
            FireProjectile(CannonRight);
            rightShoot = false;
        }
    }

    private void FireProjectile(GameObject Cannon)
    {
        Instantiate(Shot, Cannon.transform.position, transform.rotation).Fire();
    }

    private void SetStartShootCounters()
    {
        leftShootCounter = 0f;
        rightShootCounter = shootCounter / 2;
    }
}
