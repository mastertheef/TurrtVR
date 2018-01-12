using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRocket : Projectile {

    [SerializeField] private Explosion ExplosionPrefab;
    [SerializeField] private float resorceProbability = 0.5f;

    // Use this for initialization
    void Start () {
        transform.LookAt(Camera.main.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var exp = Instantiate(ExplosionPrefab, gameObject.transform);
            exp.transform.position = gameObject.transform.position;
            
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Laser" ||
            collision.gameObject.tag == "LaserBeam")
        {
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        
    }

    // todo: refactor code duplication
    private void GiveResource()
    {
        float probe = Random.Range(0f, 1f);
        if (probe < resorceProbability)
        {
            float choice = Random.Range(0f, 1f);
            if (choice < 0.5)
            {
                LaserBeamController.Instance.AddCharge();
            }
            else
            {
                RocketController.Instance.AddRocket();
            }
        }
    }
}
