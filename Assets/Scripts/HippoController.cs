using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HippoController : MonoBehaviour
{
    [Range(0,3)]
    public int Health;
    public float MoveSpeed;
    public float AttackInterval;
    float Strange;
    public float StrangeMin;
    public float StrangeMax;
    public float StrangeLerpTime;

    public GameObject SnowballPrefab;

    List<SnowballController> L_Snowball = new List<SnowballController>();
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Strange = StrangeMin;

        for (int i = 0; i < 10; i++)
        {
            GameObject ball = Instantiate(SnowballPrefab, transform);
            L_Snowball.Add(ball.GetComponent<SnowballController>());
            ball.SetActive(false);
        }
    }

    Vector2 moveDirect = Vector2.zero;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && attackTimer <= 0) ThrowSnowball();
        MoveUpdate();
        StrangeLerp();
        attackTimer -= Time.deltaTime;
    }
    void ThrowSnowball()
    {
        for (int i = 0; i < L_Snowball.Count; i++)
        {
            if (L_Snowball[i].gameObject.activeSelf == false)
            {
                SnowballController ball = L_Snowball[i];
                ball.gameObject.SetActive(true);
                ball.gameObject.layer = 6;
                ball.transform.position = transform.position + Vector3.right * 2;
                ball.Throw((Vector2.right + (Vector2.up / 2)) * Strange);
                break;
            }
        }
        attackTimer = AttackInterval;
    }

    bool increase = true;
    void StrangeLerp()
    {
        if (increase)
        {
            Strange += (StrangeMax / StrangeLerpTime) * Time.deltaTime;
            if (Strange >= StrangeMax) increase = false;
        }
        else
        {
            Strange -= (StrangeMax / StrangeLerpTime) * Time.deltaTime;
            if (Strange <= StrangeMin) increase = true;
        }
        //Debug.Log(Strange);
    }


    void MoveUpdate()
    {
        moveDirect.x = Input.GetAxis("Horizontal");
        rb.velocity = moveDirect * MoveSpeed; 

    }
    float attackTimer = 0;
    


    
}
