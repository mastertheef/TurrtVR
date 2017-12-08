using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{

    [SerializeField] private GameObject LaserEffects;
    [SerializeField] private AudioSource LaserChargeAudio;
    [SerializeField] private AudioSource LaserAudio;
    [SerializeField] private AudioSource LaserStopAudio;


    private bool LaserChargeFlag = false;

    // Use this for initialization
    void Start()
    {
        LaserChargeFlag =false;
        LaserChargeAudio.Play();
        
        //LaserChargeBeam.SetActive(true);
        StartCoroutine(LaserChargeWait());
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
