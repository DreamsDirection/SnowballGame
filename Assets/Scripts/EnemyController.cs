using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyTiers Tier;     //Enemy type 1,2,3
    public float MoveSpeed;     
    public float Strange;       
    public GameObject SnowballPrefab;   

    [HideInInspector]
    public bool GoAway = false; 

    Rigidbody2D rb;
    List<SnowballController> L_Snowball = new List<SnowballController>();   

    Vector2 MoveDirect = Vector2.zero;
    void Start()
    {
        Init();
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
            rb.velocity = MoveDirect;
            //Debug.Log(rb.velocity);

            //Move away logic
            if (GoAway && Camera.main.WorldToScreenPoint(transform.position).x > Screen.width * 1.05f)
            {
                GoAway = false;
                GameController.singltone.NextEnemy();
                gameObject.SetActive(false);
            }

        }
    }
    void AnimationUpdate()
    {

    }

    public void Attack()
    {
        //find unusable ball in pull
        for (int i = 0; i < L_Snowball.Count; i++)
        {
            if (L_Snowball[i].gameObject.activeSelf == false)
            {
                SnowballController ball = L_Snowball[i];
                ball.gameObject.SetActive(true);
                ball.transform.position = transform.position + Vector3.left * 2;
                ball.gameObject.layer = 7;
                ball.Throw((Vector2.left + Vector2.up / 4) * Strange);
                break;
            }
        }
    }

    void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        //StartCoroutine(MoveLeft(Random.Range(1, 2)));
        for (int i = 0; i < 10; i++)
        {
            GameObject ball = Instantiate(SnowballPrefab, transform);
            L_Snowball.Add(ball.GetComponent<SnowballController>());
            ball.SetActive(false);
        }
        StartCoroutine(RandomDirect());
    }

    float center = Screen.width / 2;
    float weightRight;
    float weightLeft;
    IEnumerator RandomDirect()
    {
        while (true)
        {
            //Random move logic
            weightRight = Camera.main.WorldToScreenPoint(transform.position).x - center;
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
                yield return new WaitForSeconds(Random.Range(2f,3f));
            }
            else
            yield return new WaitForSeconds(Random.Range(1f, 2f));

        }
    }

    public void GoInGame()
    {
        GoAway = false;
        StopAllCoroutines();
        StartCoroutine(RandomDirect());
    }

    public void GoOutFromGame()
    {
        StopAllCoroutines();
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