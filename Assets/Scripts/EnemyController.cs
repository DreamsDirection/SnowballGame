using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int Health;

    public float MoveSpeed;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveLeft(Random.Range(1,2)));
    }

    void Update()
    {
        
    }

    IEnumerator MoveLeft(float time)
    {
        while (time > 0)
        {
            Move(Vector2.left * MoveSpeed);
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (Random.Range(0, 100) < 20) StartCoroutine(Idle(2));
        else StartCoroutine(MoveRight(Random.Range(1, 2)));
    }
    IEnumerator MoveRight(float time)
    {
        while (time > 0)
        {
            Move(Vector2.right * MoveSpeed);
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (Random.Range(0, 100) < 20) StartCoroutine(Idle(2));
        else StartCoroutine(MoveLeft(Random.Range(1, 2)));
    }
    IEnumerator Idle(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (Random.Range(0, 100) < 50) StartCoroutine(MoveRight(Random.Range(1, 2)));
        else StartCoroutine(MoveLeft(Random.Range(1, 2)));
    }
    void Move(Vector2 dir)
    {
        rb.velocity = rb.velocity + (dir - rb.velocity);
    }
}
