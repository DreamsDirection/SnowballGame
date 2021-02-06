using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Throw(Vector2 direct)
    {
        rb.AddForce(direct);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Hippo")
        {
            if (--collision.gameObject.GetComponent<HippoController>().Health == 0)
            {
                GameController.singltone.GameOver();
            }
        }
        else if(collision.transform.tag == "Enemy")
        {
            GameController.singltone.EnenmyHit(collision.gameObject.GetComponent<EnemyController>().Tier);
            collision.gameObject.GetComponent<EnemyController>().GoOutFromGame();
        }
        gameObject.SetActive(false);
    }
}
