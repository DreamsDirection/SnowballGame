using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int Score;
    public int ScoreToWin;

    public int EnemyOnScreenMax;
    [HideInInspector]
    public int enemyOnScreen;

    public int Enemy1Reward;
    public int Enemy2Reward;
    public int Enemy3Reward;

    public List<EnemyController> L_Enemy = new List<EnemyController>();


    public static GameController singltone;
    public UIController uIController;
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
        enemyOnScreen = 0;
        for(int i = 0; i< L_Enemy.Count; i++)
        {
            if (L_Enemy[i].gameObject.activeSelf == true) enemyOnScreen++; 
        }
        if (enemyOnScreen < EnemyOnScreenMax)
        {
            float i = 1000;
            do
            {
                int r = Random.Range(0, L_Enemy.Count - 1);
                if (L_Enemy[r].gameObject.activeSelf == false)
                {
                    L_Enemy[r].gameObject.SetActive(true);
                    L_Enemy[r].GoInGame();
                    enemyOnScreen++;

                }
                i--;
                if (i <= 0)
                {
                    Debug.Log("Endless while");
                    break;
                }
            } while (enemyOnScreen < EnemyOnScreenMax);
        }
    }

    public void EnenmyHit(EnemyTiers tier)
    {
        switch(tier)
            {
                case EnemyTiers.Easy: { Score += Enemy1Reward; }
            break;
                case EnemyTiers.Medium: { Score += Enemy2Reward; }
            break;
                case EnemyTiers.Hard: { Score += Enemy3Reward; }
            break;
        }
    }

    
    
    public void GameOver()
    {
        init();
    }
    public void WinGame()
    {

    }
    public void RestartGame()
    {

    }

    void init()
    {
        if (!singltone) singltone = this;
        else Destroy(gameObject);


        EnemyController[] enemy = FindObjectsOfType<EnemyController>();
        for(int i = 0; i < enemy.Length; i++)
        {
            L_Enemy.Add(enemy[i]);
            enemy[i].gameObject.SetActive(false);
        }
        NextEnemy();
        //SnowballController[] balls = FindObjectsOfType<SnowballController>();
        //for(int i = 0; i++)
    }
    float timer;
    void EnemyAttackUpdate()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && enemyOnScreen > 0)
        {
            while (true)
            {
                int r = Random.Range(0, L_Enemy.Count);
                if (L_Enemy[r].gameObject.activeSelf && !L_Enemy[r].GoAway)
                {
                    L_Enemy[r].Attack();
                    timer = 5;
                    break;
                }
            }
        }
    }
}
