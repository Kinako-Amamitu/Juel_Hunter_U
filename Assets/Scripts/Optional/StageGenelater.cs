using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenelater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageSelector()
    {
        //‰æ–Ê‘JˆÚ
        Initiate.DoneFading();
        Initiate.Fade("Stage1", Color.black, 0.5f);
    }
}
