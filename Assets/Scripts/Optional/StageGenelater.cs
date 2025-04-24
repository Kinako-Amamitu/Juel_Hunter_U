////////////////////////////////////////////////////////////////
///
/// ステージセレクト画面を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenelater : MonoBehaviour
{
    //ステージリスト
    [SerializeField] GameObject[] stageList;

    //se
    public AudioClip stageEnter;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

        //AudioComponentを取得
        audioSource = GetComponent<AudioSource>();

        //前ステージクリアで次のステージを解放させる
        for (int i=0;i<NetworkManager.Instance.stageCullentClear;i++)
        {
            stageList[i].SetActive(true);
        }
    }

    /// <summary>
    /// ステージをせんたくする
    /// </summary>
    /// <param name="stageNum"></param>
    public void StageSelector(int stageNum)
    {
        audioSource.PlayOneShot(stageEnter);
        GameGenerator.UpdateStageScene(stageNum);
    }

    /// <summary>
    /// タイトルへ
    /// </summary>
    public void Title()
    {
        audioSource.PlayOneShot(stageEnter);
        //画面遷移
        Initiate.DoneFading();
        Initiate.Fade("Title", Color.black, 0.5f);
    }
}
