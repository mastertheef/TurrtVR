using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    [SerializeField] private Explosion explosion;
    [SerializeField] protected float hitPoints = 12;
    [SerializeField] protected float moveSpeed = 0.05f;
    [SerializeField] protected int score = 5;
    [SerializeField] protected float addSeconds = 1;
    [SerializeField] protected float reduceSeconds = 1;

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

        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            Destroy(canvas.gameObject);
        }

        GameManager.Instance.CountDown += addSeconds;
    }

    protected void ExplodeIfKilled()
    {
        if (hitPoints <= 0 && !isExploded)
        {
            Explode();
            GameManager.Instance.Score += score;
        }
    }

    public virtual void TakeDamage(Collider collider)
    {
        var projectile = collider.gameObject.GetComponent<Projectile>();
        hitPoints -= projectile.Damage;
    }
}
