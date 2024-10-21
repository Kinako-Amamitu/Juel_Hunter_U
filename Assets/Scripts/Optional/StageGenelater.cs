using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenelater : MonoBehaviour
{
    [SerializeField] GameObject[] stageList;

    //se
    public AudioClip stageEnter;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

        //AudioComponentを取得
        audioSource = GetComponent<AudioSource>();

        for (int i=0;i<NetworkManager.Instance.stageCullentClear;i++)
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
        audioSource.PlayOneShot(stageEnter);
        GameGenerator.UpdateStageScene(stageNum);
    }

    public void Title()
    {
        audioSource.PlayOneShot(stageEnter);
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Title", Color.black, 0.5f);
    }
}
