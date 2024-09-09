using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 3.0f,3.0f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void Spawn()
    {
        Instantiate(Enemy, transform.position,Quaternion.identity);
    }
}
