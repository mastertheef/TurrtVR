﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Singleton<Turret>
{

    [SerializeField] private Projectile Shot;
    [SerializeField] private GameObject CannonLeft;
    [SerializeField] private GameObject CannonRight;

    [SerializeField] private float shootCounter;
    [SerializeField] private float shootCounterPenetration = 0;

    private bool canFire;
    private bool startedFiring;
    private bool isFiring;
    private bool isDamaged;

    public float ShootCounterPenetration
    {
        get { return shootCounterPenetration; }
        set { shootCounterPenetration = value; }
    }

    public bool IsFiring { get { return isFiring; } }
    public bool CanFire { get { return canFire && !isDamaged; } }
    public bool IsDamaged {
        get { return isDamaged; }
        set { isDamaged = value; }
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

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!startedFiring)
            {
                StartCoroutine(PlayStartShootAndWait());
            }
        }
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


        if (CanFire && !isFiring)
        {
            StartFiring();
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

    public void RestartIfFiring()
    {
        if (isFiring)
        {
            StopFiring();
            StartFiring();
        }
    }

    public IEnumerator GetDamage()
    {
        isDamaged = true;
        StopFiring();
        yield return new WaitForSeconds(GameManager.Instance.CantFireTimer);
        isDamaged = false;
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
