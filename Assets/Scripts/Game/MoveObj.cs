////////////////////////////////////////////////////////////////
///
/// オブジェクトの動きを管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveFloorA : MonoBehaviour
{
    //物質
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
                rb.MovePosition(transform.position + new Vector3(0.05f, 0, 0));
    }
}