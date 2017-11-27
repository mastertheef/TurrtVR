using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Enemy
{
    [SerializeField] private int hitsToExplode = 4;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private BaseEffect debuff;
    
    private Vector3 rotationDirection;

	// Use this for initialization
	void Start () {
        
        rotationDirection = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
    }
	
	// Update is called once per frame
	void Update () {
	    if (hitsToExplode <= 0 && !isExploded)
        {
            Explode();
        }
	}

    private void FixedUpdate()
    {
        transform.Rotate(rotationDirection, rotationSpeed);
        StartCoroutine(MoveToPlayer());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Laser")
        {
            hitsToExplode--;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            this.Explode();

            EffectSystem.Instance.AddEffect(debuff);
            //Turret.Instance.ShootCounterPenetration = GameManager.Instance.FireSpeedPenetration;
            //GameManager.Instance.StartSpeedCountDown();
            //Turret.Instance.RestartIfFiring();
        }
    }

    private IEnumerator MoveToPlayer()
    {
        while(!isExploded)
        {
            transform.localPosition = Vector3.MoveTowards(transform.position, Vector3.zero, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    protected override void Explode()
    {
        base.Explode();
        GameManager.Instance.AsteroidsCount++;
    }
}
