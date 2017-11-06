using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Enemy {

    [SerializeField] private Projectile laser;
    [SerializeField] private GameObject[] cannons;
    [SerializeField] private float shootDelay = 5;
    [SerializeField] private float shootScatter = 10;
    [SerializeField] private float moveScatter = 20;
    [SerializeField] private float flyAwayDistance = 600;
    [SerializeField] private float shootDistance = 300;

    private bool canShoot = false;
    private float shootTimer;
    private Vector3 targetPoint;
     
	// Use this for initialization
	void Start () {
        //Explode();
        shootTimer = shootDelay;
        float left = Random.Range(moveScatter * (-2), moveScatter * (-1));
        targetPoint = (Vector3.zero - transform.position) + Random.insideUnitSphere * moveScatter;
        transform.rotation = Quaternion.LookRotation(targetPoint.normalized);
	}
	
	// Update is called once per frame
	void Update () {
		if (hitPoints <= 0 && !isExploded)
        {
            Explode();
        }

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            canShoot = true && (Vector3.Distance(Vector3.zero, transform.position) <= shootDistance);
            shootTimer = shootDelay;
        }

	}
    
    protected override void Explode()
    {
        base.Explode();
        Rigidbody[] children = gameObject.GetComponentsInChildren<Rigidbody>();
        Collider[] chidrenColliders = gameObject.GetComponentsInChildren<Collider>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].isKinematic = false;
            chidrenColliders[i].isTrigger = false;
        }
    }

    private void Shoot()
    {
        var target = Camera.main.transform.position;
        Projectile l = Instantiate(laser, cannons[Random.Range(0, cannons.Length - 1)].transform.position, transform.rotation);
        var targetRange = new Vector3(target.x + Random.Range(shootScatter *-1, shootScatter), target.y + Random.Range(shootScatter*-1, shootScatter), target.z);
        l.transform.LookAt(targetRange);
        l.Fire();
        canShoot = false;
    }

    private void FixedUpdate()
    {
        if (canShoot && !isExploded && !isBehind())
        {
            Shoot();
        }

        if (fliedAway())
        {
            Destroy(gameObject);
        }
        StartCoroutine(Fly());
    }

    private IEnumerator Fly()
    {
        while (!isExploded)
        {
            transform.position += targetPoint.normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private bool fliedAway()
    {
        return Vector3.Distance(Vector3.zero, transform.position) >= flyAwayDistance;
    }

    private bool isBehind()
    {
        return (Vector3.zero - transform.position).z > 0;
    }
}
