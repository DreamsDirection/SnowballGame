using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController singltone;  
    public GameSettings gameSettings;   
    public UIController uIController;   
    public HippoController Player;      

    public Transform EnemySpawnPoint;   
    public ParticleSystem WinGameParticles;     
    public int Score { get; private set; }  
    public int ScoreToWin { get; private set; } 
    public int EnemyOnScreenMax { get; private set; } 
    //[HideInInspector]
    public float EnemyAttackInterval { get; private set; } 

    public int[] EnemyRewards { get; private set; }

    List<EnemyController> L_EnemyEnable = new List<EnemyController>();
    List<EnemyController> L_EnemyDisable = new List<EnemyController>();

    
    private void Awake() //Инициализация синглтона и установка разрешения экрана
    {
        Debug.Log(name + " Awake()");
        if (!singltone) singltone = this;
        else Destroy(gameObject);

        Screen.SetResolution(1920, 1080, true);

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


    //Активирует случайного врага из пула и инициализирует его параметры
    public void NextEnemy()
    {

        Debug.Log(name + " Next enemy()");
        if (L_EnemyEnable.Count < EnemyOnScreenMax)
        {
            do
            {
                int r = Random.Range(0, L_EnemyDisable.Count - 1);
                L_EnemyDisable[r].gameObject.SetActive(true);
                L_EnemyDisable[r].transform.position = EnemySpawnPoint.position;
                L_EnemyDisable[r].GoInGame();
                L_EnemyEnable.Add(L_EnemyDisable[r]);
                L_EnemyDisable.RemoveAt(r);
            } while (L_EnemyEnable.Count < EnemyOnScreenMax);
        }
    }

    //Инкрементируем количество очков и проверяем условие
    //перемещяем врага в список неактивных и вызываем метод попадания
    public void EnemyHit(EnemyController enemy)
    {
        Debug.Log(name + " Enemy hit()");
        for(int i =0;i<L_EnemyEnable.Count; i++)
        {
            if(L_EnemyEnable[i] == enemy)
            {
                L_EnemyEnable.RemoveAt(i);
                L_EnemyDisable.Add(enemy);
                break;
            }
        }
        int reward = 0;
        switch (enemy.Tier)
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
        uIController.StartHitText(Camera.main.WorldToScreenPoint(enemy.transform.position), reward.ToString());
        enemy.GetHit();
    }

    
    //Устанавливает все значения в первоначальный вид и спавним врагов
    public void NewGame()
    {
        Debug.Log(name + " Retsart game()");
        Player.gameObject.SetActive(true);
        Player.Init();
        uIController.Init();
        Score = 0;
        for (int i = 0; i < L_EnemyEnable.Count; i++)
        {
            L_EnemyEnable[i].transform.position = EnemySpawnPoint.position;
            L_EnemyEnable[i].gameObject.SetActive(false);
        }
        NextEnemy();

    }

    //Убираем всех персонажей с сцены
    public void GameOver()
    {
        Player.gameObject.SetActive(false);
        for (int i = 0; i < L_EnemyEnable.Count; i++)
            L_EnemyDisable.Add(L_EnemyEnable[i]);
        L_EnemyEnable.Clear();
        for (int i = 0; i < L_EnemyDisable.Count; i++)
            L_EnemyDisable[i].gameObject.SetActive(false);
    }
    //Убираем всех персонажей с сцены и включаем визуальный эфект победы
    public void WinGame()
    {
        Player.gameObject.SetActive(false);
        for (int i = 0; i < L_EnemyEnable.Count; i++)
            L_EnemyDisable.Add(L_EnemyEnable[i]);
        L_EnemyEnable.Clear();
        for (int i = 0; i < L_EnemyDisable.Count; i++)
            L_EnemyDisable[i].gameObject.SetActive(false);
        WinGameParticles.Play();
    }

    //Установка всех необходимых значений и старт игры
    void init()
    {
        Debug.Log(name + " Init()");

        L_EnemyEnable.Clear();
        EnemyController[] enemy = FindObjectsOfType<EnemyController>();
        for(int i = 0; i < enemy.Length; i++)
        {
            L_EnemyDisable.Add(enemy[i]);
            enemy[i].transform.position = EnemySpawnPoint.position;
            enemy[i].gameObject.SetActive(false);
        }

        ScoreToWin = gameSettings.ScoreToWin;
        EnemyOnScreenMax = gameSettings.EnemyMaxOnScreen;
        EnemyAttackInterval = gameSettings.EnemyAttackInterval;
        EnemyRewards = gameSettings.EnemyRewards;




        NewGame();
    }
    float timer;

    //Атака случайного активного врага
    void EnemyAttackUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && L_EnemyEnable.Count > 0)
        {
            Debug.Log(name + " Enemy attack()");
            int r = Random.Range(0, L_EnemyEnable.Count);
            L_EnemyEnable[r].Attack();
            timer = EnemyAttackInterval;
        }
    }
}
