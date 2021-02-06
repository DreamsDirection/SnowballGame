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

    
    
    public void GameOver()
    {

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
}
