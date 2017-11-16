using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

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
    [SerializeField] private float fireSpeedPenetration = 0.15f;
    [SerializeField] private float fireSpeedPenetrationSconds = 3;
    [SerializeField] private float cantFireTimer = 3;
    [SerializeField] private Text asteroidsLabel;
    [SerializeField] private Text shipsLabel;
    [SerializeField] private Text speedPenetrationTimerLabel;


    private int asteroidsCount = 0;
    private int shipsCount = 0;
    private int asteroidsMissed = 0;
    private int shipsMissed = 0;
    private bool shipDamaged = false;
    [SerializeField] private float speedPenetrationTimer = 0;

    public int AsteroidsCount
    {
        get { return asteroidsCount; }
        set
        {
            asteroidsCount = value;
            asteroidsLabel.text = string.Format("Asteroids: {0}", asteroidsCount);
        }
    }

    public int ShipsCount
    {
        get { return shipsCount; }
        set
        {
            shipsCount = value;
            shipsLabel.text = string.Format("Ships: {0}", shipsCount);
        }
    }

    public float FireSpeedPenetration { get { return fireSpeedPenetration; } }
    public float FireSpeedPenetrationTimer { get { return fireSpeedPenetrationSconds; } }
    public float CantFireTimer { get { return cantFireTimer; } }

    public float SpawnDistance { get { return spawnDistance; } }
    public float FirstShootDistance { get { return firstShootDistance; } }
    public float SecondShootDistance { get { return secondShootDistance; } }

    private int enemyCount = 0;
    private float spawnCounter;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpeedPenetrationCountDown()
    {
        speedPenetrationTimerLabel.enabled = true;
        if (--speedPenetrationTimer == 0)
        {
            CancelInvoke("SpeedPenetrationCountDown");
            speedPenetrationTimerLabel.enabled = false;
            Turret.Instance.ShootCounterPenetration = 0;
            Turret.Instance.RestartIfFiring();
            
        };
        speedPenetrationTimerLabel.text = speedPenetrationTimer.ToString();
    }

    public void StartSpeedCountDown()
    {
        speedPenetrationTimer = fireSpeedPenetrationSconds;
        InvokeRepeating("SpeedPenetrationCountDown", 1, 1);
    }

    private void SpawnEnemy()
    {
        //Enemy enemy = Instantiate(enemies[enemies.Count - 1]);
        Enemy enemy = Instantiate(enemies[Random.Range(0, enemies.Count - 1)]);
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
