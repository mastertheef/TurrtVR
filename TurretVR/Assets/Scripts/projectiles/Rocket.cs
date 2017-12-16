using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Rocket : Projectile {

    [SerializeField] private Explosion explosion;
    private GameObject target;
    private bool isExploded = false;

	// Use this for initialization
	void Start () {
        target = FindNearestTarget();
        if (target != null)
        {
            transform.LookAt(target.transform);
        }
        else
        {
            transform.rotation = Camera.main.transform.rotation;
        }
        StartCoroutine(Move());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        string colTag = collision.gameObject.tag;
        if (colTag == "EnemyShip" || colTag == "Asteroid" )
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            StartCoroutine(Explode());
        }
        else if (colTag == "EnemyPart")
        {
            collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(damage);
            StartCoroutine(Explode());
        }
    }

    private GameObject FindNearestTarget()
    {
        // TODO: filter exploded enemies

        List<GameObject> allEnemies = GameObject.FindGameObjectsWithTag("Asteroid").ToList();
        allEnemies.AddRange(GameObject.FindGameObjectsWithTag("EnemyShip").ToList());

        List<GameObject> allVisible = GetAllOnScreen(allEnemies);
        return GetNearest(allVisible);

    }

    private List<GameObject> GetAllOnScreen(List<GameObject> allEnemies)
    {
        for (int i = allEnemies.Count - 1; i >= 0; i--)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(allEnemies[i].transform.position);
            bool isExploded = allEnemies[i].GetComponent<Enemy>().IsExploded;
            if (!IsOnScreen(screenPoint) || isExploded)
            {
                allEnemies.Remove(allEnemies[i]);
            }
        }

        return allEnemies;
    }

    private GameObject GetNearest(List<GameObject> allVisible)
    {
        List<float> ranges = new List<float>();

        if (allVisible == null || allVisible.Count == 0)
        {
            return null;
        }

        allVisible.ForEach(x => ranges.Add(Vector3.Distance(Vector3.zero, x.transform.position)));
        int minIndex = ranges.IndexOf(Mathf.Min(ranges.ToArray()));
        return  allVisible[minIndex];
    }

    private bool IsOnScreen(Vector3 point)
    {
        return point.z > 0 && point.x > 0 && point.x < 1 && point.y > 0 && point.y < 1;
    }

    protected override IEnumerator Move()
    {
        while (GetDistance() < ShotMaxRange)
        {
            if (target == null || target.GetComponent<Enemy>().IsExploded)
            {
                target = FindNearestTarget();
            }

            if (target != null && !target.GetComponent<Enemy>().IsExploded)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, ShotSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.forward * ShotSpeed * Time.deltaTime);
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator Explode()
    {
        if (!isExploded)
        {
            Instantiate(explosion, transform);
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            var emmision = GetComponentInChildren<ParticleSystem>().emission;
            emmision.enabled = false;

            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }
}
