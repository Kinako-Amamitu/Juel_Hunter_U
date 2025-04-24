////////////////////////////////////////////////////////////////
///
/// 敵の自動生成を管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 敵のプレハブにアタッチするクラス
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    //Unityから取得
    [SerializeField] GameObject Enemy; //敵のオブジェクト取得

    // 起動した時1回だけ実行
    void Start()
    {
        //敵を一定間隔生成するように設定
        InvokeRepeating("Spawn", 3.0f,3.0f);
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    private void Spawn()
    {
        //敵をこのスクリプトに付けた場所から生成
        Instantiate(Enemy, transform.position,Quaternion.identity);
    }
}
