using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletPos;
    private float bulletspeed = 250;

    void Start()
    {
        bulletPos = transform.GetChild(0).gameObject;
    }

    void Update()
    {

        Shot();
        PlayerRotate();

    }

    void PlayerRotate()
    {
        if (Input.GetKey("right"))
        {
            transform.Rotate(0, 2, 0);
        }

        if (Input.GetKey("left"))
        {
            transform.Rotate(0, -2, 0);
        }

    }

    void Shot()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject ball = (GameObject)Instantiate(bullet, bulletPos.transform.position, Quaternion.identity);
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            ballRigidbody.AddForce(transform.forward * bulletspeed);
        }

    }
}
