using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyController : MonoBehaviour
{
    public EnemyTiers Tier;     //Enemy type 1,2,3
    public Transform ThrowBallPosition;
    public float MoveSpeed { get; private set; }
    public float Strange { get; private set; }
    public GameObject SnowballPrefab { get; private set; }
    public bool GoAway { get; private set; }

    Rigidbody2D rb;
    List<SnowballController> L_Snowball = new List<SnowballController>();   

    Vector2 MoveDirect = Vector2.zero;

    SkeletonAnimation anim;
    Camera cam;
    public GameSettings gameSettings;

    float moveSpeed;     
    float screenWidth;
    float center;
    float weightRight;
    float weightLeft;

    void Start()
    {
        SnowballPrefab = gameSettings.SnowBallPrefab;
        for (int i = 0; i < 2; i++)
        {
            GameObject ball = Instantiate(SnowballPrefab, transform.position, Quaternion.identity);
            L_Snowball.Add(ball.GetComponent<SnowballController>());
            ball.SetActive(false);
        }
        Init();
    }
    //Установка первоначальных значений
    public void Init()
    {
        StopAllCoroutines();
        MoveSpeed = gameSettings.EnemyMoveSpeed;
        Strange = gameSettings.EnemyStrange;
        Debug.Log(name + " Init()");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<SkeletonAnimation>();
        cam = Camera.main;
        screenWidth = Screen.width;
        center = screenWidth / 2;
        moveSpeed = Random.Range(MoveSpeed - 1f, MoveSpeed + 1f);
        StartCoroutine(RandomDirect());
    }

    void Update()
    {
        MoveUpdate();
        AnimationUpdate();
    }
    void MoveUpdate()
    {
        if (rb)
        {
            rb.velocity = MoveDirect * moveSpeed + Vector2.down;
            //Debug.Log(rb.velocity);

            //Move away logic
            if (GoAway && cam.WorldToScreenPoint(transform.position).x > screenWidth * 1.05f)
            {
                GoAway = false;
                GameController.singltone.NextEnemy();
                gameObject.SetActive(false);
            }

        }
        
    }

    //Установка стороны взгляда персонажа в зависимости от вектора движения
    void AnimationUpdate()
    {
        if (rb.velocity.x > 0)
            transform.localScale = new Vector3(0.5f, transform.localScale.y, transform.localScale.z);
        else if (rb.velocity.x < 0)
            transform.localScale = new Vector3(-0.5f, transform.localScale.y, transform.localScale.z);
    }
    //Бросаем любой неактивный снежок
    public void Attack()
    {
        Debug.Log(name + " Attack()");
        //find unusable ball in pull
        for (int i = 0; i < L_Snowball.Count; i++)
        {
            if (L_Snowball[i].gameObject.activeSelf == false)
            {
                SnowballController ball = L_Snowball[i];
                ball.gameObject.SetActive(true);
                ball.transform.position = ThrowBallPosition.position;
                ball.gameObject.layer = 7;
                ball.Throw((Vector2.left + Vector2.up / 4) * Strange);
                break;
            }
        }
    }

    //Рандомизация вектора движения в зависимости от отдалённости от края или центра с случайным промежутком
    //включаем анимацию
    IEnumerator RandomDirect()
    {
        while (true)
        {
            Debug.Log(name + " Random direct numerator");
            //Random move logic
            weightRight = cam.WorldToScreenPoint(transform.position).x - center;
            weightLeft = weightRight - (center * 0.95f);

            MoveDirect.x = -Random.Range(weightRight, weightLeft);
            MoveDirect.Normalize();
            //Random move logic end

            //Debug.Log("left " + weightRight);
            //Debug.Log("Right " + weightLeft);
            //Debug.Log("Direct " + MoveDirect);

            //Random idle chance
            if (Random.Range(0, 100) < 5)
            {
                MoveDirect = Vector2.zero;
                anim.AnimationName = "Idle";
                yield return new WaitForSeconds(Random.Range(2f,3f));
            }
            else
            {
                anim.AnimationName = "run";
                yield return new WaitForSeconds(Random.Range(1f, 2f));
            }

        }
    }

    //Установка ствртовых значений и вывод на сцену
    public void GoInGame()
    {
        Init();
        Debug.Log(name + " Go in game()");
        GoAway = false;
        MoveDirect = Vector2.left;
        StartCoroutine(RandomDirect());
    }

    //Вывод за сцену
    public void GoOutFromGame()
    {
        Debug.Log(name + " Go out from game()");
        StopAllCoroutines();
        GoAway = true;
        MoveDirect = Vector2.right;
    }
    //Включаем анимацию попадания и выводим за сцену
    public void GetHit()
    {
        StopAllCoroutines();
        MoveDirect = Vector2.zero;
        if(gameObject.activeSelf)
        StartCoroutine(AnimHit());
    }
    //Анимация попадания и полседующий вывод за сцену
    IEnumerator AnimHit()
    {
        anim.AnimationName = "gets_hit";
        yield return new WaitForSeconds(0.3f);
        anim.AnimationName = "run";
        GoOutFromGame();

    }

}

public enum EnemyTiers
{
    Easy,
    Medium,
    Hard
}