using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveFloorA : MonoBehaviour
{

    private Vector3 initialPosition;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //        transform.Translate(0.2f, 0f, 0f);
        //    transform.position += new Vector3(0.2f, 0f, 0f);
        //        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(5f, 0, 0), 0.2f);
        //        rb.AddForce(new Vector2(10, 0));
                rb.MovePosition(transform.position + new Vector3(0.05f, 0, 0));
    }
}