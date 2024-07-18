using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObject : MonoBehaviour
{
    [SerializeField]
    public new Renderer renderer;

    [SerializeField]
    public bool isTouch = false;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter(Collision other)
    {
        //ボールにぶつかったとき
        if (other.gameObject.tag == "Ball")
        {
            Debug.Log("ボールにぶつかった！");
        }
        else
        {
            Debug.Log("ボールじゃないところにぶつかった！");
            Destroy(gameObject);
        }
       
    }
}