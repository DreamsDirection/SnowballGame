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
        rb.velocity = direct;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.position = transform.position + Vector3.left * 2;
        if (collision.transform.tag == "Hippo")
        {
            HippoController player = collision.gameObject.GetComponent<HippoController>();
            Debug.Log(name + " on collision enter(Hippo)");
            player.Health--;
            GameController.singltone.uIController.UpdateHealth();
        }
        else if (collision.transform.tag == "Enemy" && collision.gameObject.GetComponent<EnemyController>().GoAway == false)
        {
            Debug.Log(name + " On collision enter(Enemy)");
            collision.gameObject.GetComponent<EnemyController>().GetHit();
            GameController.singltone.EnemyHit(collision.gameObject.GetComponent<EnemyController>().Tier,collision.gameObject.transform.position);
        }
        gameObject.SetActive(false);
    }
}
