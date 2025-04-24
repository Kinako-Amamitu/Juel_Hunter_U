////////////////////////////////////////////////////////////////
///
/// ポーズ機能を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pose : MonoBehaviour
{
    //Unityからアタッチ
    [SerializeField] GameObject Panel;

    //ゲームマネージャーを使う
    GameGenerator gameGenerator;

    // Start is called before the first frame update
    void Start()
    {
        gameGenerator = GameObject.Find("GameGenerator").GetComponent<GameGenerator>();
    }

    //ポーズ機能
    public void Posejoin()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameGenerator.Pose();
        }
        
    }
}
