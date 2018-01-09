using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotherShip : MonoBehaviour {
    [SerializeField] private float shipLength = 12f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float portalInOffset = 102f;
    [SerializeField] private float portalOutOffset = 227f;
    [SerializeField] private float spawnDistance = 350f;

    [SerializeField] private GameObject PortalPrefab;
    [SerializeField] private GameObject ship;
    [SerializeField] private GameObject invisibleCapsule;

    [SerializeField] private float EnergyShield = 1000f;
    [SerializeField] private float EnergyShieldRechrgeDelay = 2f;
    [SerializeField] private float EnergyShieldRestoreSpeed = 10f;
    [SerializeField] private float Hitpoints = 5000f;
    [SerializeField] private float LaserBeamDamage = 5f;

    [SerializeField] Image ShieldBar;
    [SerializeField] Image HeathBar;
    

    private bool started = false;
    private bool visible = false;
    private Vector3 invisibleCapsuleDefaultPosition;
    private GameObject portal;
    [SerializeField] private float energyShield;
    [SerializeField] private float hitPoints;
    private Coroutine beamDamageCoroutine;


	// Use this for initialization
	void Start () {
        invisibleCapsuleDefaultPosition = invisibleCapsule.transform.localPosition;
        energyShield = EnergyShield;
        hitPoints = Hitpoints;
    }

    private void FixedUpdate()
    {
        if (!started)
        {
            StartCoroutine(Teleporting());
        }
    }

    // Update is called once per frame
    void Update () {
        var energyScale = ShieldBar.transform.localScale;
        var healthScale = HeathBar.transform.localScale;
        var shield = (energyShield * 100 / EnergyShield) / 100;
        shield = shield < 0 ? 0 : shield;
        var health = (hitPoints * 100 / Hitpoints) / 100;
        health = health < 0 ? 0 : health;
        ShieldBar.transform.localScale = new Vector3(shield, energyScale.y, energyScale.z);
        HeathBar.transform.localScale = new Vector3(health, healthScale.y, healthScale.z);
    }


    private IEnumerator MoveIn()
    {
        Vector3 portalPosition = new Vector3(transform.localPosition.x - portalInOffset, transform.localPosition.y, transform.localPosition.z);
        portal = Instantiate(PortalPrefab, portalPosition, transform.rotation);
        Vector3 direction = new Vector3(0, 1, 0);
        while (ship.transform.localPosition.z > shipLength * (-1)) 
        {
            ship.transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
       
        Destroy(portal.gameObject);
        visible = true;
    }

    private IEnumerator MoveOut()
    {
        Vector3 portalPosition = new Vector3(transform.localPosition.x - portalOutOffset, transform.localPosition.y, transform.localPosition.z);
        portal = Instantiate(PortalPrefab, portalPosition, transform.rotation);
        invisibleCapsule.transform.localPosition = new Vector3(0, 0, -4.4f - shipLength*2);
        Vector3 direction = new Vector3(0, 1, 0);
        while (ship.transform.localPosition.z > shipLength * (-2))
        {
            ship.transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(portal.gameObject);

        ship.transform.localPosition = Vector3.zero;
        invisibleCapsule.transform.localPosition = invisibleCapsuleDefaultPosition;
        visible = false;
    }

    private Vector3 GetRandomPosition()
    {
        Quaternion randAng = Quaternion.Euler(Random.Range(GameManager.Instance.EnemyMaxBottom, GameManager.Instance.EnemyMaxTop), Random.Range(GameManager.Instance.EnemyMaxLeft, GameManager.Instance.EnemyMaxRight), 0);
        return randAng * Vector3.forward * spawnDistance;
    }

    private IEnumerator Teleporting()
    {
        started = true;
        while (true)
        {
            transform.position = GetRandomPosition();

            StartCoroutine(MoveIn());
            while (!visible) { yield return null; }

            yield return new WaitForSeconds(5);
            StartCoroutine(MoveOut());
            while (visible) { yield return null; }

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator TakeBeamDamage()
    {
        while (true)
        {
            if (energyShield > 0)
            {
                energyShield -= LaserBeamDamage * Time.deltaTime;
                yield return null;
            }
        }
    }

    public void Collision(Collision collision)
    {
        if (collision.gameObject.tag == "Laser" || collision.gameObject.tag == "Rocket")
        {
            var damage = collision.gameObject.GetComponent<Projectile>().Damage;

            if (energyShield > 0)
            {
                energyShield -= damage;
            }
            else
            {
                hitPoints -= damage;
            }
        }

        if (collision.gameObject.tag == "LaserBeam" && beamDamageCoroutine == null)
        {
            beamDamageCoroutine = StartCoroutine(TakeBeamDamage());
        }
    }

    public void CollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "LaserBeam" && beamDamageCoroutine != null)
        {
            StopCoroutine(beamDamageCoroutine);
            beamDamageCoroutine = null;
        }
    }
}
