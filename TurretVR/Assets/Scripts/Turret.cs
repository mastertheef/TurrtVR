using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Singleton<Turret>
{

    [SerializeField] private Projectile Shot;
    [SerializeField] private GameObject CannonLeft;
    [SerializeField] private GameObject CannonRight;

    [SerializeField] private float shootCounter;
    private float shootCounterPenetration = 0;
    private float leftShootCounter;
    private float rightShootCounter;

    private bool leftShoot = false;
    private bool rightShoot = false;


    private float fireSpeedPenetrationTimer = 0;
    private float cantFireTimer = 0;
    private bool cantFire;

    public float ShootCounterPenetration
    {
        get { return shootCounterPenetration; }
        set { shootCounterPenetration = value; }
    }

    public float ShootCounter
    {
        get
        {
            return shootCounter + shootCounterPenetration;
        }
    }

    // Use this for initialization
    void Start()
    {
        //SetStartShootCounters();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !cantFire)
        {
            leftShootCounter -= Time.deltaTime;
            rightShootCounter -= Time.deltaTime;

            if (leftShootCounter <= 0)
            {
                leftShoot = true;
                leftShootCounter = ShootCounter;
            }

            if (rightShootCounter <= 0)
            {
                rightShoot = true;
                rightShootCounter = ShootCounter;
            }
        }


        if (fireSpeedPenetrationTimer > 0)
        {
            fireSpeedPenetrationTimer -= Time.deltaTime;
        }
        else
        {
            shootCounterPenetration = 0;
            fireSpeedPenetrationTimer = 0;
        }

        if (cantFireTimer > 0)
        {
            cantFireTimer -= Time.deltaTime;
            cantFire = true;
        }
        else
        {
            cantFireTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SetStartShootCounters();
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
        rightShootCounter = ShootCounter / 2;
    }
}
