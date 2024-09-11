using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageSelect()
    {
        //‰æ–Ê‘JˆÚ
        Initiate.DoneFading();
        Initiate.Fade("StageSelect", Color.white, 1.0f);
    }
}
