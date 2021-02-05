using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HippoController : MonoBehaviour
{
    public float MoveSpeed;
    [Range(0,3)]
    public int Health;

    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    Vector2 moveDirect = Vector2.zero;
    void Update()
    {
        moveDirect.x = Input.GetAxis("Horizontal");
        rb.velocity = moveDirect * MoveSpeed; 
    }
    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Snowball")
        {
            if(--Health == 0)
            {
                GameController.singltone.GameOver();
            }
        }
    }
}
