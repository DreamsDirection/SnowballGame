using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Throw(Vector2 direct, float strange)
    {
        rb.AddForce(direct * strange * Time.deltaTime);
    }
}
