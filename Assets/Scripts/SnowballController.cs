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
            switch (collision.gameObject.GetComponent<EnemyController>().Tier)
            {
                case EnemyTiers.Easy: { GameController.singltone.Score ++; } break;
                case EnemyTiers.Medium: { GameController.singltone.Score += 2; } break;
                case EnemyTiers.Hard: { GameController.singltone.Score += 3; } break;
            }
            collision.gameObject.GetComponent<EnemyController>().GoOutFromGame();
        }
        gameObject.SetActive(false);
    }
}
