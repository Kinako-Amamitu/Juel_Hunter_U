using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pose : MonoBehaviour
{
    [SerializeField] GameObject Panel;

    GameGenerator gameGenerator;
    // Start is called before the first frame update
    void Start()
    {
        gameGenerator = GameObject.Find("GameGenerator").GetComponent<GameGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Posejoin()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameGenerator.Pose();
        }
        
    }
}
