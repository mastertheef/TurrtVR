using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserBeamController : Singleton<LaserBeamController> {

    [SerializeField] private List<LaserBeam> lasers;
    [SerializeField] private float MaxEnergy;
    [SerializeField] private float EnergyUseSpeed = 0.5f;
    [SerializeField] private float EnergyRechargeSpeed = 0.1f;
    [SerializeField] private float NewLaserDelayAfterfullUse = 5f;
    [SerializeField] private Image EnergyBar;


    private bool isFiring = false;
    private bool canFire = true;
    [SerializeField] private float energy;
    public float Energy { get { return energy; } }

	// Use this for initialization
	void Start () {
        energy = MaxEnergy;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetButton("Fire2") && canFire && !isFiring)
        {
            if (energy >= 0)
            {
                
                lasers.ForEach(x => x.Fire());
                isFiring = true;
            }
        } 

        if (!Input.GetButton("Fire2") && isFiring)
        {
            StopFiring();
        }

        if (isFiring)
        {
            energy -= EnergyUseSpeed * Time.deltaTime;
        }

        if (energy <= 0 && isFiring)
        {
            StartCoroutine(WaitAfterFullUse());
            StopFiring();
        }

        if (!isFiring && energy < MaxEnergy)
        {
            energy += EnergyRechargeSpeed * Time.deltaTime;
        }

        float XScale = (energy * 100 / MaxEnergy) / 100;
        EnergyBar.transform.localScale = new Vector3(XScale, EnergyBar.transform.localScale.y, EnergyBar.transform.localScale.z);
    }


    private void StopFiring()
    {
        lasers.ForEach(x => x.StopFire());
        isFiring = false;
    }

    private IEnumerator WaitAfterFullUse()
    {
        canFire = false;
        yield return new WaitForSeconds(NewLaserDelayAfterfullUse);
        canFire = true;
    }
}
