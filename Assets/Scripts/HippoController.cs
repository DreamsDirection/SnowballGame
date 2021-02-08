using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using SimpleInputNamespace;
public class HippoController : MonoBehaviour
{
    [Range(0,3)]
    public int Health;
    public float MoveSpeed;
    public float AttackInterval;
    public float Strange { get; private set; }
    public float StrangeMin;
    public float StrangeMax;
    public float StrangeLerpTime;

    public GameObject SnowballPrefab;
    public Vector3 ThrowBallPositionOffset;

    List<SnowballController> L_Snowball = new List<SnowballController>();
    Rigidbody2D rb;
    SkeletonAnimation anim;
    LineRenderer lineRenderer;

    public Transform StartPos;
    public float attackTimer { get; private set; }
    Vector2 moveDirect = Vector2.zero;
    Vector3 ThrowDirect;
    bool strangeIncrease = true;
    public bool IsActive;
    public enum PlayerStates
    {
        Reload,
        Strange
    }
    public PlayerStates PlayerState = PlayerStates.Reload;
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject ball = Instantiate(SnowballPrefab, transform.position,Quaternion.identity);
            L_Snowball.Add(ball.GetComponent<SnowballController>());
            ball.SetActive(false);
        }
        Init();
    }
    public void Init()
    {
        Debug.Log(name + " Start()");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<SkeletonAnimation>();
        lineRenderer = GetComponent<LineRenderer>();
        Strange = StrangeMin;
        GameController.singltone.Player = this;
        attackTimer = AttackInterval;
        transform.position = StartPos.position;
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (IsActive)
        {
            if (SimpleInput.GetButtonDown("attack") | Input.GetKeyDown(KeyCode.Space) && PlayerState == PlayerStates.Strange)
            {
                PlayerState = PlayerStates.Reload;
                ThrowSnowball();
                attackTimer = AttackInterval;
            }
            if (PlayerState == PlayerStates.Strange)
            {
                StrangeLerp();
                TrajectoryRender();
            }
            else
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0) PlayerState = PlayerStates.Strange;
            }
            MoveUpdate();
            AnimationUpdate();
        }
    }
    void TrajectoryRender()
    {
        float time;
        Vector3[] points = new Vector3[100];
        lineRenderer.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            time = i * 0.03f;
            points[i] = (transform.position + ThrowBallPositionOffset) + ThrowDirect * time + Physics.gravity * time * time / 2f;
            if(points[i].y < -3.3f)
            {
                lineRenderer.positionCount = i+1;
                break;
            }
        }
        lineRenderer.SetPositions(points);
    }
    void StrangeLerp()
    {
        if (strangeIncrease)
        {
            Strange += (StrangeMax / StrangeLerpTime) * Time.deltaTime;
            if (Strange >= StrangeMax) strangeIncrease = false;
        }
        else
        {
            Strange -= (StrangeMax / StrangeLerpTime) * Time.deltaTime;
            if (Strange <= StrangeMin) strangeIncrease = true;
        }
        ThrowDirect = (transform.right + (Vector3.up / 3)).normalized * Strange;
        //Debug.Log(Strange);
    }
    
    void ThrowSnowball()
    {
        Debug.Log(name + " Throw ball()");
        GetComponent<SkeletonAnimation>().AnimationName = "Shoot";
        for (int i = 0; i < L_Snowball.Count; i++)
        {
            if (L_Snowball[i].gameObject.activeSelf == false)
            {
                SnowballController ball = L_Snowball[i];
                ball.gameObject.SetActive(true);
                ball.gameObject.layer = 6;
                ball.transform.position = transform.position + ThrowBallPositionOffset;
                ball.Throw(ThrowDirect);
                break;
            }
        }
        lineRenderer.positionCount = 0;
    }



    void MoveUpdate()
    {
        moveDirect.x = SimpleInput.GetAxis("Horizontal");
        rb.velocity = moveDirect * MoveSpeed; 
    }

    void AnimationUpdate()
    {
        if (rb.velocity.x > 0)
        {
            anim.AnimationName = "run";
            transform.localScale = new Vector3(0.5f, transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x < 0)
        {
            anim.AnimationName = "run";
            transform.localScale = new Vector3(-0.5f, transform.localScale.y, transform.localScale.z);

        }
        else
        {
            anim.AnimationName = "Idle";

        }
    }
    


    
}
