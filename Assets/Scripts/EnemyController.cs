using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyTiers Tier;
    public float MoveSpeed;
    public float AttackInterval;
    public GameObject SnowballPrefab;

    [HideInInspector]
    public bool GoAway = false;
    bool canAttack;

    Rigidbody2D rb;
    List<SnowballController> L_Snowball = new List<SnowballController>();

    Vector2 MoveDirect = Vector2.zero;
    void Start()
    {
        Init();
    }

    float timer;
    void Update()
    {
        Attack();
        MoveUpdate();


        
    }
    void MoveUpdate()
    {
        if (rb)
        {
            rb.velocity = MoveDirect;
            //Debug.Log(rb.velocity);
            if (GoAway && Camera.main.WorldToScreenPoint(transform.position).x > Screen.width * 1.1f)
            {
                GoAway = false;
                GameController.singltone.NextEnemy();
                gameObject.SetActive(false);
            }

        }
    }

    void Attack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && canAttack)
        {
            timer = AttackInterval;

            for (int i = 0; i < L_Snowball.Count; i++)
            {
                if (L_Snowball[i].gameObject.activeSelf == false)
                {
                    SnowballController ball = L_Snowball[i];
                    ball.gameObject.SetActive(true);
                    ball.transform.position = transform.position + Vector3.left * 2;
                    ball.gameObject.layer = 7;
                    ball.Throw(Vector2.left * 300);
                    break;
                }
            }
        }
    }

    void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        //StartCoroutine(MoveLeft(Random.Range(1, 2)));
        timer = AttackInterval;
        for (int i = 0; i < 10; i++)
        {
            GameObject ball = Instantiate(SnowballPrefab, transform);
            L_Snowball.Add(ball.GetComponent<SnowballController>());
            ball.SetActive(false);
        }
        StartCoroutine(RandomDirect());
    }

    float minValidPos = Screen.width / 2;
    float maxValidPos = Screen.width * 0.9f;
    float weightRight;
    float weightLeft;
    IEnumerator RandomDirect()
    {
        while (true)
        {
            weightRight = Camera.main.WorldToScreenPoint(transform.position).x - minValidPos;
            weightLeft = weightRight - (minValidPos * 0.9f);

            MoveDirect.x = -Random.Range(weightRight, weightLeft);
            MoveDirect.Normalize();

            Debug.Log("left " + weightRight);
            Debug.Log("Right " + weightLeft);
            Debug.Log("Direct " + MoveDirect);
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }

    public void GoInGame()
    {
        GoAway = false;
        canAttack = true;
        StopAllCoroutines();
        StartCoroutine(RandomDirect());
    }

    public void GoOutFromGame()
    {
        StopAllCoroutines();
        canAttack = false;
        GoAway = true;
        MoveDirect = Vector2.right;
    }
    
}

public enum EnemyTiers
{
    Easy,
    Medium,
    Hard
}