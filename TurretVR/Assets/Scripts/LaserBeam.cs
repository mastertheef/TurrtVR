using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    // TODO: rebild to incapsulate the energy logic
    [SerializeField] private GameObject LaserEffects;
    [SerializeField] private AudioSource LaserChargeAudio;
    [SerializeField] private AudioSource LaserAudio;
    [SerializeField] private AudioSource LaserStopAudio;
    [SerializeField] private float energyUseSpeed = 0.5f;
    [SerializeField] private float rechargeSpeed = 0.2f;

    [SerializeField] private float MaxEnergy = 100f;
    private float energy;
    
    public float Energy { get { return energy; } }

    private bool LaserChargeFlag = false;
    private bool rechardeStarted = false;

    // Use this for initialization
    void Start()
    {
        LaserChargeFlag = false;
        LaserChargeAudio.Play();
        
        //LaserChargeBeam.SetActive(true);
        StartCoroutine(LaserChargeWait());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            energy -= energyUseSpeed * Time.deltaTime;

            if (energy <= 0)
            {
                SelfDestruct();
            }
        }
    }

    private void Recharge()
    {
        rechardeStarted = true;
    }


    private IEnumerator LaserChargeWait()
    {
        yield return new WaitForSeconds(1.4f);

        if (LaserChargeFlag == false)
        {
            LaserEffects.SetActive(true);
            LaserAudio.Play();
            LaserEffects.SetActive(true);
            LaserChargeFlag = false;
        }
    }

    public IEnumerator SelfDestruct()
    {
        LaserChargeFlag = true;
        LaserEffects.SetActive(false);
        LaserAudio.Stop();
        LaserStopAudio.Play();
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
