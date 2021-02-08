using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Score;
    public int ScoreToWin;

    public int EnemyOnScreenMax;
    //[HideInInspector]
    public int enemyOnScreen;
    public float EnemyAttackInterval;

    public int[] EnemyRewards = new int[3];

    public List<EnemyController> L_Enemy = new List<EnemyController>();

    public Transform EnemySpawnPoint;
    public static GameController singltone;
    public UIController uIController;
    public HippoController Player;
    public ParticleSystem WinGameParticles;
    
    private void Awake()
    {
        Debug.Log(name + " Awake()");
        if (!singltone) singltone = this;
        else Destroy(gameObject);
        enemyOnScreen = 0;

    }
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyAttackUpdate();
    }

    public void NextEnemy()
    {

        Debug.Log(name + " Next enemy()");
        if (enemyOnScreen < EnemyOnScreenMax)
        {
            float i = 1000;
            do
            {
                int r = Random.Range(0, L_Enemy.Count - 1);
                if (L_Enemy[r].gameObject.activeSelf == false)
                {
                    L_Enemy[r].gameObject.SetActive(true);
                    L_Enemy[r].transform.position = EnemySpawnPoint.position;
                    L_Enemy[r].GoInGame();
                    enemyOnScreen++;

                }
                i--;
                if (i <= 0)
                {
                    Debug.Log(name + " Endless while");
                    break;
                }
            } while (enemyOnScreen < EnemyOnScreenMax);
        }
    }

    public void EnemyHit(EnemyTiers tier,Vector3 position)
    {
        Debug.Log(name + " Enemy hit()");
        int reward = 0;
        switch (tier)
            {
                case EnemyTiers.Easy: { reward= EnemyRewards[0]; }
            break;
                case EnemyTiers.Medium: { reward = EnemyRewards[1]; }
            break;
                case EnemyTiers.Hard: { reward = EnemyRewards[2]; }
            break;
        }
        Score += reward;
        if (Score >= ScoreToWin) uIController.WinGame();
        uIController.StartHitText(Camera.main.WorldToScreenPoint(position),reward.ToString());

        enemyOnScreen--;
    }

    
    
    public void NewGame()
    {
        Debug.Log(name + " Retsart game()");
        Player.Health = 3;
        Player.Init();
        Player.IsActive = true;
        uIController.Init();
        Score = 0;
        for (int i = 0; i < L_Enemy.Count; i++)
        {
            L_Enemy[i].transform.position = EnemySpawnPoint.position;
            L_Enemy[i].gameObject.SetActive(false);
        }
        enemyOnScreen = 0;
        NextEnemy();

    }
    public void GameOver()
    {
        Player.Init();
        for(int i = 0; i< L_Enemy.Count; i++)
        {
            L_Enemy[i].gameObject.SetActive(false);
        }
    }
    public void WinGame()
    {
        Player.IsActive = false;
        Player.Init();
        for (int i = 0; i < L_Enemy.Count; i++)
        {
            L_Enemy[i].gameObject.SetActive(false);
        }
        WinGameParticles.Play();
    }
    void init()
    {
        Debug.Log(name + " Init()");


        EnemyController[] enemy = FindObjectsOfType<EnemyController>();
        for(int i = 0; i < enemy.Length; i++)
        {
            L_Enemy.Add(enemy[i]);
            enemy[i].transform.position = EnemySpawnPoint.position;
            enemy[i].gameObject.SetActive(false);
        }
        NewGame();
        //SnowballController[] balls = FindObjectsOfType<SnowballController>();
        //for(int i = 0; i++)

    }
    float timer;
    void EnemyAttackUpdate()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && enemyOnScreen > 0)
        {
            Debug.Log(name + " Enemy attack()");
            int i = 10000;
            while (true)
            {
                int r = Random.Range(0, L_Enemy.Count);
                if (L_Enemy[r].gameObject.activeSelf && !L_Enemy[r].GoAway)
                {
                    L_Enemy[r].Attack();
                    timer = EnemyAttackInterval;
                    break;
                }
                i--;
                if(i < 0)
                {
                    Debug.Log(name + " Endless while(EnemyAttackUpdate()");
                    break;
                }
            }
        }
    }
}
