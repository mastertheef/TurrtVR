using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    //[SerializeField] private List<Asteroid> asteroids;
    [SerializeField] private List<Enemy> enemies;

    [SerializeField] private int maxEnemies = 20;
    [SerializeField] private float spawnDistance = 500f;
    [SerializeField] private float firstShootDistance = 400f;
    [SerializeField] private float secondShootDistance = 200f;

    [SerializeField] private float spawnDelay = 2f;

    [SerializeField] private float enemyMaxLeft = -60;
    [SerializeField] private float enemyMaxRight = 60;
    [SerializeField] private float enemyMaxTop = 40;
    [SerializeField] private float enemyMaxBottom = -5;

    public float SpawnDistance { get { return spawnDistance; } }
    public float FirstShootDistance { get { return firstShootDistance; } }
    public float SecondShootDistance { get { return secondShootDistance; } }

    private int enemyCount = 0;
    private float spawnCounter;

	// Use this for initialization
	void Start () {
        StartCoroutine(Spawn());
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(enemies[enemies.Count - 1]);
        //Enemy enemy = Instantiate(enemies[Random.Range(0, enemies.Count - 1)]);
        Quaternion randAng = Quaternion.Euler(Random.Range(enemyMaxLeft, enemyMaxRight), Random.Range(enemyMaxBottom, enemyMaxTop), 0);
        enemy.transform.position = transform.position + randAng * Vector3.forward * spawnDistance;
        enemyCount++;
    }

    private IEnumerator Spawn()
    {
        while (enemyCount <= maxEnemies)
        {
           SpawnEnemy();
           yield return new WaitForSeconds(spawnDelay);
        }
    }
}
