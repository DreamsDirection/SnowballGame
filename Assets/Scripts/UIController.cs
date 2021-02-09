using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleInputNamespace;

public class UIController : MonoBehaviour
{
    public Text Game_Time;
    public Text Game_Score;
    public Text Pause_Score;
    public Text GameOver_Score;
    public Text GameOver_PlayTime;
    public Text WinGame_Time;
    public Text HitAnimatedText;

    public Image Game_StrangeBar;
    public Image Game_ReloadBar;
    public Vector2 Game_StrangeBarOffset;
    public Vector2 Game_ReloadBarOffset;

    public GameObject B_Pause;
    public GameObject PausePanel;
    public GameObject GameOverPanel;
    public GameObject WinGamePanel;
    public Image[] HealthImage = new Image[3];
    public Image[] StarsImage = new Image[3];


    float timer;
    HippoController player => GameController.singltone.Player;
    Camera cam;
    void Start()
    {
        Init();
    }
    public void Init()
    {
        GameController.singltone.uIController = this;
        cam = Camera.main;
        timer = 0;
        UpdateHealth();
        StarsImage[0].gameObject.SetActive(false);
        StarsImage[1].gameObject.SetActive(false);
        StarsImage[2].gameObject.SetActive(false);
        HitAnimatedText.gameObject.SetActive(false);
        PausePanel.SetActive(false);
        WinGamePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        B_Pause.SetActive(true);
    }




    void Update()
    {
        UpdateUI();
        InputUpdate();
        timer += Time.deltaTime;
    }
    void InputUpdate()
    {
        if (SimpleInput.GetButtonUp("pause") | Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 0;
            PausePanel.SetActive(true);
            Pause_Score.text = GameController.singltone.Score.ToString();
            B_Pause.gameObject.SetActive(false);
        }
        if (SimpleInput.GetButtonUp("return") | Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
            PausePanel.SetActive(false);
            B_Pause.gameObject.SetActive(true);
        }
        if (SimpleInput.GetButtonUp("restart"))
        {
            GameController.singltone.NewGame();
            Time.timeScale = 1;
            PausePanel.SetActive(false);
            GameOverPanel.SetActive(false);
            WinGamePanel.SetActive(false);
            B_Pause.gameObject.SetActive(true);
            timer = 0;
        }
    }
    void UpdateUI()
    {
        if(GameController.singltone && player && player.gameObject.activeSelf)
        {
            if (player.attackTimer < 0)
            {
                Game_ReloadBar.gameObject.SetActive(false);
                Game_StrangeBar.transform.parent.gameObject.SetActive(true);
                Game_StrangeBar.transform.parent.position = cam.WorldToScreenPoint(player.transform.position) + (Vector3)Game_StrangeBarOffset;
                Game_StrangeBar. fillAmount = (player.Strange - player.StrangeMin) / (player.StrangeMax - player.StrangeMin);
            }
            else
            {
                Game_StrangeBar.transform.parent.gameObject.SetActive(false);
                Game_ReloadBar.gameObject.SetActive(true);
                Game_ReloadBar.transform.position = cam.WorldToScreenPoint(player.transform.position) + (Vector3)Game_ReloadBarOffset;
                Game_ReloadBar.fillAmount = player.attackTimer / player.AttackInterval;
            }
            Game_Time.text = "Time- " + Mathf.Round(timer).ToString();
            Game_Score.text = "Score- " + GameController.singltone.Score.ToString();
        }
    }

    int HippoHealth => player.Health;
    public void UpdateHealth()
    {
        switch (HippoHealth)
        {
            case 0:
                {
                    GameOverPanel.SetActive(true);
                    GameOverPanel.GetComponent<Animation>().Play();
                    GameOver_PlayTime.text = "Play time- " + Mathf.Round(timer).ToString();
                    GameOver_Score.text = "Score- " + GameController.singltone.Score;
                    Game_ReloadBar.gameObject.SetActive(false);
                    Game_StrangeBar.gameObject.SetActive(false);
                    HealthImage[0].gameObject.SetActive(false);
                    HealthImage[1].gameObject.SetActive(false);
                    HealthImage[2].gameObject.SetActive(false);

                    GameController.singltone.GameOver();
                }break;
            case 1:
                {
                    HealthImage[0].gameObject.SetActive(true);
                    HealthImage[1].gameObject.SetActive(false);
                    HealthImage[2].gameObject.SetActive(false);
                }
                break;
            case 2:
                {
                    HealthImage[0].gameObject.SetActive(true);
                    HealthImage[1].gameObject.SetActive(true);
                    HealthImage[2].gameObject.SetActive(false);
                }
                break;
            case 3:
                {
                    HealthImage[0].gameObject.SetActive(true);
                    HealthImage[1].gameObject.SetActive(true);
                    HealthImage[2].gameObject.SetActive(true);
                }
                break;
        }
    }

    public void WinGame()
    {
        Game_ReloadBar.gameObject.SetActive(false);
        Game_StrangeBar.gameObject.SetActive(false);
        WinGamePanel.SetActive(true);
        WinGamePanel.GetComponent<Animation>().Play();
        WinGame_Time.text = "Play time- " + Mathf.Round(timer).ToString();
        switch (HippoHealth)
        {
            case 1:
                {
                    StarsImage[0].gameObject.SetActive(true);
                    StarsImage[1].gameObject.SetActive(false);
                    StarsImage[2].gameObject.SetActive(false);
                }
                break;
            case 2:
                {
                    StarsImage[0].gameObject.SetActive(true);
                    StarsImage[1].gameObject.SetActive(true);
                    StarsImage[2].gameObject.SetActive(false);
                }
                break;
            case 3:
                {
                    StarsImage[0].gameObject.SetActive(true);
                    StarsImage[1].gameObject.SetActive(true);
                    StarsImage[2].gameObject.SetActive(true);
                }
                break;
        }
        GameController.singltone.WinGame();
    }
    public void StartHitText(Vector3 position, string text)
    {
        HitAnimatedText.gameObject.SetActive(true);
        HitAnimatedText.GetComponent<Text>().enabled = true;
        //HitAnimatedText.rectTransform.anchoredPosition.Set(position.x / Screen.width, position.y / Screen.width);
        HitAnimatedText.rectTransform.anchorMax = new Vector2(position.x / Screen.width, (position.y + 200) / Screen.width);
        HitAnimatedText.rectTransform.anchorMin = new Vector2(position.x / Screen.width, (position.y + 200) / Screen.width);
        HitAnimatedText.text = "+" + text;
        HitAnimatedText.GetComponent<Animation>().Play();
    }

   

}
