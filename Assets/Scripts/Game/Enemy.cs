using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] Vector2 velocity;

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Black_hole")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Blue"||collision.gameObject.tag=="Red"
            ||collision.gameObject.tag=="Green"||collision.gameObject.tag=="Pink"||collision.gameObject.tag=="Yellow")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

    }
   
}
