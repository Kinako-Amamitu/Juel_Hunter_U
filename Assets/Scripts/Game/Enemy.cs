using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] Vector2 velocity;

    public AudioClip ninja; //死亡音
    AudioSource audioSource; //SE入力にオーディオソースを使用する

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = velocity;

        //AudioComponentを取得
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Black_hole")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Blue"||collision.gameObject.tag=="Red"
            ||collision.gameObject.tag=="Green"||collision.gameObject.tag=="Purple"||collision.gameObject.tag=="Yellow")
        {
           
           
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

    }
}
