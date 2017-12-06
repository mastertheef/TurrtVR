﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Singleton<Turret>
{

    [SerializeField] private Projectile Shot;
    [SerializeField] private GameObject CannonLeft;
    [SerializeField] private GameObject CannonRight;
    [SerializeField] private GameObject FireStartEffect;
    [SerializeField] private float shootCounter;

    private float shootCounterPenetration = 0;
    private float shootCounterSpeedUp = 0;
    private float projectileAdditionalDamage = 0;

    private GameObject fireStartLeft;
    private GameObject fireStartRight;

    private bool canFire;
    private bool startedFiring;
    private bool isFiring;
    private bool isDamaged;

    public float ShootCounterPenetration
    {
        get { return shootCounterPenetration; }
        set { shootCounterPenetration = value; }
    }

    public float ShootCounterSpeedUp
    {
        get { return shootCounterSpeedUp; }
        set { shootCounterSpeedUp = value; }
    }

    public float ProjectileAdditionalDamage
    {
        get { return projectileAdditionalDamage; }
        set { projectileAdditionalDamage = value; }
    }

    public bool IsFiring { get { return isFiring; } }
    public bool CanFire { get { return canFire && !isDamaged; } }
    public bool IsDamaged
    {
        get { return isDamaged; }
        set { isDamaged = value; }
    }

    public float ShootCounter
    {
        get
        {
            return shootCounter + shootCounterPenetration - shootCounterSpeedUp;
        }
    }

    public float ProjectileAdditionalScale { get; set; }

    // Use this for initialization
    void Start()
    {
        Debug.Log(GameObject.FindWithTag("GunLeft").GetComponent<Renderer>().material.shader.name);

        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!startedFiring)
            {
                StartCoroutine(PlayStartShootAndWait());
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                DestroyFireStart();
            }
            if (CanFire) { StopCoroutine(PlayStartShootAndWait()); }
            startedFiring = false;
            canFire = false;
            StopFiring();
        }

        if (Input.GetButton("Fire1") && CanFire && !isFiring)
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

        ShowFireStart();
        yield return new WaitForSeconds(3);
        canFire = true;
        DestroyFireStart();
    }

    private void ShowFireStart()
    {
        fireStartLeft = fireStartLeft == null
           ? Instantiate(FireStartEffect, CannonLeft.transform)
           : fireStartLeft;
        fireStartRight = fireStartRight == null
            ? Instantiate(FireStartEffect, CannonRight.transform)
            : fireStartRight;
    }

    private void DestroyFireStart()
    {
        if (fireStartLeft != null) Destroy(fireStartLeft);
        if (fireStartRight != null) Destroy(fireStartRight);
    }

    public void RestartIfFiring()
    {
        if (isFiring)
        {
            StopFiring();
            StartFiring();
        }
    }

    public void StartFiring()
    {
        isFiring = true;
        if (!IsInvoking("FireLeft"))
        {
            InvokeRepeating("FireLeft", 0, ShootCounter);

           
        }
        if (!IsInvoking("FireRight"))
            InvokeRepeating("FireRight", ShootCounter / 2, ShootCounter);

    }

    public void StopFiring()
    {
        isFiring = false;
        CancelInvoke("FireLeft");
        CancelInvoke("FireRight");
    }

    private void FireProjectile(GameObject Cannon)
    {
        Projectile proj = Instantiate(Shot, Cannon.transform.position, transform.rotation);
        ApplyProjectileModifiers(proj);
        proj.Fire();
    }

    private void FireLeft()
    {
        Projectile proj = Instantiate(Shot, CannonLeft.transform.position, transform.rotation);
        ApplyProjectileModifiers(proj);
        proj.Fire();
    }

    private void FireRight()
    {
        Projectile proj = Instantiate(Shot, CannonRight.transform.position, transform.rotation);
        ApplyProjectileModifiers(proj);
        proj.Fire(); ;
    }

    private void ApplyProjectileModifiers(Projectile proj)
    {
        proj.AddDamage = ProjectileAdditionalDamage;
        proj.transform.localScale += new Vector3(ProjectileAdditionalScale, ProjectileAdditionalScale, ProjectileAdditionalScale);
    }
}
