using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using SimpleInputNamespace;
public class HippoController : MonoBehaviour
{
    public int Health { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackInterval { get; private set; }
    public float Strange { get; private set; }
    public float StrangeMin { get; private set; }
    public float StrangeMax { get; private set; }
    public float StrangeLerpTime { get; private set; }
    public float attackTimer { get; private set; }

    public GameObject SnowballPrefab { get; private set; }
    public Transform StartPos;
    public Vector3 ThrowBallPositionOffset;

    List<SnowballController> L_Snowball = new List<SnowballController>();
    Rigidbody2D rb;
    SkeletonAnimation anim;
    LineRenderer lineRenderer;
    public GameSettings gameSettings;

    Vector2 moveDirect = Vector2.zero;
    Vector3 ThrowDirect;
    bool strangeIncrease = true;
    public enum PlayerStates
    {
        Reload,
        Strange
    }
    public PlayerStates PlayerState = PlayerStates.Reload;
    void Start()
    {
        SnowballPrefab = gameSettings.SnowBallPrefab;
        for (int i = 0; i < 2; i++)
        {
            GameObject ball = Instantiate(SnowballPrefab, transform.position,Quaternion.identity);
            L_Snowball.Add(ball.GetComponent<SnowballController>());
            ball.SetActive(false);
        }
        Init();
    }
    //Установка всех первоначальных значений
    public void Init()
    {
        MoveSpeed = gameSettings.HippoMoveSpeed;
        AttackInterval = gameSettings.HippoAttackInterval;
        StrangeMin = gameSettings.HippoStrangeMin;
        StrangeMax = gameSettings.HippoStrangeMax;
        StrangeLerpTime = gameSettings.HippoStrangeLerpSpeed;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<SkeletonAnimation>();
        lineRenderer = GetComponent<LineRenderer>();
        Strange = StrangeMin;
        GameController.singltone.Player = this;
        attackTimer = AttackInterval;
        transform.position = StartPos.position;
        lineRenderer.positionCount = 0;
        Health = 3;
    }

   
    void Update()
    {
        if (SimpleInput.GetButtonDown("attack") | Input.GetKeyDown(KeyCode.Space) && PlayerState == PlayerStates.Strange)
        {
            PlayerState = PlayerStates.Reload;
            Strange = StrangeMin;
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
    //Динамически отрисовываем линию траектории полёта снежка
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
    //Динамически меняем силу броска
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
        ThrowDirect = (transform.forward + (Vector3.up / 3)).normalized * Strange;
        //Debug.Log(Strange);
    }
    
    //Бросаем любой неактивный снежок вперёд
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

    //Устанавливаем анимации в зависимости от вектора движения и скоростиы
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
    public void TakeHit()
    {
        Health--;
    }
    


    
}
