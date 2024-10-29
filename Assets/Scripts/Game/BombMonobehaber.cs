using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMonobehaber : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    Bullet bullet;


    private void Start()
    {

        bullet = GetComponent<Bullet>();



    }

    void OnDrawGizmos()
    {
        //Å@CircleCastÇÃÉåÉCÇâ¬éãâª
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position , 1.0f );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Rail(right)" ||
            collision.gameObject.tag == "Rail(left)"||
            collision.gameObject.tag == "Rail(down)" ||
            collision.gameObject.tag == "Rail(down)(slow)" ||
            collision.gameObject.tag == "Rail(up)"||
            collision.gameObject.tag == "Enemy")
        {

            bullet.Explosion(bomb);
            
        }
    }
}
