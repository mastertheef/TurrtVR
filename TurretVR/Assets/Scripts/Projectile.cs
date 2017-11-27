using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] private int ShotMaxRange = 100;
    [SerializeField] private float ShotSpeed = 100f;
    [SerializeField] private float damage = 10;

    [SerializeField] HitEffect HitEffect;
    [SerializeField] BaseEffect debuf;

    public float Damage { get { return damage; } }

    private Vector3 startPosition;
    private bool fire = false;
    
    // Use this for initialization
    void Start () {
        startPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fire) { 
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        while (GetDistance() < ShotMaxRange)
        {
            transform.localPosition += transform.forward * Time.deltaTime * ShotSpeed;
            yield return null;
        }

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag != "EnemyLaser" && (collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "EnemyShip"))
        {
            var hitPosition = collision.contacts[0].point;
            Instantiate(HitEffect, hitPosition, Quaternion.identity);
        }

        else if (gameObject.tag == "EnemyLaser" && collision.gameObject.tag == "Player")
        {
            EffectSystem.Instance.AddEffect(debuf);
            Destroy(gameObject);
        }
    }

    private float GetDistance()
    {
        return Mathf.Abs(Vector3.Distance(startPosition, this.transform.position));
    }

    public void Fire()
    {
        fire = true;
    }

}
