using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public HippoController Player;
    public float HippoSpeed;
    public float HippoAttakInterval;
    public float HippoStrange;
    public float HippoStrangeLerpTime;

    public List<EnemyController> L_Enemy = new List<EnemyController>();
    public float EnemySpeed;
    public float EnemyAttakInterval;


    public static GameController singltone;
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        StrangeLerp();
    }

    bool increase = true;
    void StrangeLerp()
    {
        if (increase)
        {
            HippoStrange += Time.deltaTime;
            if (HippoStrange >= HippoStrangeLerpTime) increase = false;
        }
        else
        {
            HippoStrange -= Time.deltaTime;
            if (HippoStrange <= 0) increase = true;
        }
        Debug.Log(HippoStrange);
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

        Player.MoveSpeed = HippoSpeed;
    }
}
