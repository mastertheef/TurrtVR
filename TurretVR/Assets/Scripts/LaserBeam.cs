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

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        
        LaserAudio.Play();
        LaserEffects.SetActive(true);
    }

    public void StopFire()
    {
        LaserAudio.Stop();
        LaserChargeAudio.Stop();
        LaserStopAudio.Play();
        LaserEffects.SetActive(false);
    }

}
