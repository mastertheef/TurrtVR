using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Singleton<Turret>
{

    [SerializeField] private Projectile Shot;
    [SerializeField] private GameObject CannonLeft;
    [SerializeField] private GameObject CannonRight;

    [SerializeField] private float shootCounter;
    [SerializeField] private float shootCounterPenetration = 0;
    private float leftShootCounter;
    private float rightShootCounter;

    private bool leftShoot = false;
    private bool rightShoot = false;


    private float fireSpeedPenetrationTimer = 0;
    private float cantFireTimer = 0;
    private bool canFire;
    private bool startedFiring;
    private bool isFiring;

    public float ShootCounterPenetration
    {
        get { return shootCounterPenetration; }
        set { shootCounterPenetration = value; }
    }

    public bool IsFiring { get { return isFiring; } }

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
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) )
        {
            if (!startedFiring)
            {
                StartCoroutine(PlayStartShootAndWait());
            }
        }
    }

    private IEnumerator PlayStartShootAndWait()
    {
        startedFiring = true;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
        canFire = false;
        yield return new WaitForSeconds(3);
        canFire = true;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            StopCoroutine(PlayStartShootAndWait());
            startedFiring = false;
            canFire = false;
            StopFiring();
        }

        if (canFire && !isFiring)
        {
            StartFiring();
        }
    }

    public void RestartIfFiring()
    {
        if (isFiring)
        {
            StopFiring();
            StartFiring();
        }
    }

    private void StartFiring()
    {
        InvokeRepeating("FireLeft", 0, ShootCounter);
        InvokeRepeating("FireRight", ShootCounter / 2, ShootCounter);
        isFiring = true;
    }

    private void StopFiring()
    {
        CancelInvoke("FireLeft");
        CancelInvoke("FireRight");
        isFiring = false;
    }

    private void FireProjectile(GameObject Cannon)
    {
        Instantiate(Shot, Cannon.transform.position, transform.rotation).Fire();
    }

    private void FireLeft()
    {
        Instantiate(Shot, CannonLeft.transform.position, transform.rotation).Fire();
    }

    private void FireRight()
    {
        Instantiate(Shot, CannonRight.transform.position, transform.rotation).Fire();
    }
}
