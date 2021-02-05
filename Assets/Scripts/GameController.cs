using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    public List<EnemyController> L_Enemy = new List<EnemyController>();
    public List<SnowballController> L_Snowball = new List<SnowballController>();


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

        for(int i = 0; i < L_Snowball.Count; i++)
        {
            L_Snowball[i].gameObject.SetActive(false);
        }

        
    }
}
