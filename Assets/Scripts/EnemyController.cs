using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyController : MonoBehaviour
{
    public EnemyTiers Tier;     //Enemy type 1,2,3
    public float MoveSpeed;     
    float moveSpeed;     
    public float Strange;       
    public GameObject SnowballPrefab;
    public Transform ThrowBallPosition;

    [HideInInspector]
    public bool GoAway = false; 

    Rigidbody2D rb;
    List<SnowballController> L_Snowball = new List<SnowballController>();   

    Vector2 MoveDirect = Vector2.zero;

    public SkeletonAnimation anim { get; private set; }
    Camera cam;
    float screenWidth;
    float center;
    float weightRight;
    float weightLeft;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject ball = Instantiate(SnowballPrefab, transform.position, Quaternion.identity);
            L_Snowball.Add(ball.GetComponent<SnowballController>());
            ball.SetActive(false);
        }
        Init();
    }
    public void Init()
    {
        StopAllCoroutines();
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
    void AnimationUpdate()
    {
        if (rb.velocity.x > 0)
            transform.localScale = new Vector3(0.5f, transform.localScale.y, transform.localScale.z);
        else if (rb.velocity.x < 0)
            transform.localScale = new Vector3(-0.5f, transform.localScale.y, transform.localScale.z);
    }

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

    public void GoInGame()
    {
        Init();
        Debug.Log(name + " Go in game()");
        GoAway = false;
        MoveDirect = Vector2.left;
        StartCoroutine(RandomDirect());
    }

    public void GoOutFromGame()
    {
        Debug.Log(name + " Go out from game()");
        StopAllCoroutines();
        GoAway = true;
        MoveDirect = Vector2.right;
    }
    public void GetHit()
    {
        StopAllCoroutines();
        MoveDirect = Vector2.zero;
        StartCoroutine(AnimHit());
    }
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