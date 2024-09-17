using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenelater : MonoBehaviour
{
    [SerializeField] GameObject[] stageList;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<NetworkManager.Instance.stageCullentClear;i++)
        {
            stageList[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageSelector(int stageNum)
    {
        GameGenerator.UpdateStageScene(stageNum);
    }
}
