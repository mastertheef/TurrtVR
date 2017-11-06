using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private Explosion explosion;
    [SerializeField] protected float hitPoints = 100;
    [SerializeField] protected float moveSpeed = 0.05f;
    protected bool isExploded = false;

   // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual void Explode()
    {
        var exp = Instantiate(explosion, gameObject.transform);
        exp.transform.position = gameObject.transform.position;
        isExploded = true;
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        Collider collider = GetComponent<Collider>();

        if (meshRenderer != null)
            meshRenderer.enabled = false;
        if (collider != null)
            collider.enabled = false;
        gameObject.GetComponent<EnemyPointer>().RemoveIndication();
    }

    public virtual void TakeDamage(Collider collider)
    {
        var projectile = collider.gameObject.GetComponent<Projectile>();
        hitPoints -= projectile.Damage;
    }
}
