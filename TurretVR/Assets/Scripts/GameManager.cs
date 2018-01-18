﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    //[SerializeField] private List<Asteroid> asteroids;


    [Header("Enemies")]
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private MotherShip MotherShipPrefab;

    [Header("Spawning")]
    [SerializeField] private float[] spawnDelayRange = new float[2] { 1, 5 };
    [SerializeField] private float spawnDistance = 500f;
    [SerializeField] private float firstShootDistance = 400f;
    [SerializeField] private float secondShootDistance = 200f;
    [SerializeField] private float enemyMaxLeft = -60;
    [SerializeField] private float enemyMaxRight = 60;
    [SerializeField] private float enemyMaxTop = 40;
    [SerializeField] private float enemyMaxBottom = -5;
    [SerializeField] private float bossSpawnSecond = 30;
    [SerializeField] private float spawnDelayAfterBoss = 20;

    [Header("Game")]
    [SerializeField] private float gameDuration = 90f;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text gameTimerLabel;

    private int asteroidsCount = 0;
    private int shipsCount = 0;
    private int score = 0;
    private int asteroidsMissed = 0;
    private int shipsMissed = 0;
    private int gameTime;
    private bool shipDamaged = false;

    public int AsteroidsCount
    {
        get { return asteroidsCount; }
        set { asteroidsCount = value; }
    }

    public int ShipsCount
    {
        get { return shipsCount; }
        set { shipsCount = value; }
    }

    public float CountDown { get; set; }

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreLabel.text = string.Format("Score: {0}", score);
        }
    }

    public float SpawnDistance { get { return spawnDistance; } }
    public float FirstShootDistance { get { return firstShootDistance; } }
    public float SecondShootDistance { get { return secondShootDistance; } }
    public float EnemyMaxLeft { get { return enemyMaxLeft; } }
    public float EnemyMaxRight { get { return enemyMaxRight; } }
    public float EnemyMaxTop { get { return enemyMaxTop; } }
    public float EnemyMaxBottom { get { return enemyMaxBottom; } }

    private bool mothershipSpawned;

    private int enemyCount = 0;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("GameCountDown", 0, 1);
        CountDown = gameDuration;
        score = 0;
        StartCoroutine(Spawn());
        SoundManager.Instance.PlayBackground();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemy()
    {
        if (CountDown > 0)
        {
            //Enemy enemy = Instantiate(enemies[enemies.Count - 2]);
            Enemy enemy = Instantiate(enemies[Random.Range(0, enemies.Count - 1)]);
            Quaternion randAng = Quaternion.Euler(Random.Range(enemyMaxBottom, enemyMaxTop), Random.Range(enemyMaxLeft, enemyMaxRight),  0);
            enemy.transform.position = transform.position + randAng * Vector3.forward * spawnDistance;
        }
    }

    private IEnumerator Spawn()
    {
        while (CountDown > 0)
        {
            if (gameTime >= bossSpawnSecond && !mothershipSpawned) 
            {
                Instantiate(MotherShipPrefab);
                mothershipSpawned = true;
                SoundManager.Instance.PlayBoss();
                yield return new WaitForSeconds(spawnDelayAfterBoss);
            }

            SpawnEnemy();
            float spawnDelay = Random.Range(spawnDelayRange[0], spawnDelayRange[1]);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void GameCountDown()
    {
        gameTime++;
        if (--CountDown == 0)
        {
            CancelInvoke("GameCountDown");
            SceneController.Instance.FinalScore = Score;
            SceneController.Instance.FadeAndLoadScene("Score");
        };
        gameTimerLabel.text = NiceTime(CountDown);
    }

    public string NiceTime(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
