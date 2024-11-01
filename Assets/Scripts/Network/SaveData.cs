using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData 
{

    public string authToken { get; set; } //Apiトークン
    public string userName { get; set; } //ユーザ名
    public int userID { get; set; } //ユーザID

    public int stageClearNumber { get; set; } //そのユーザーのステージクリア状況

    public int stage_ID { get; set; } //ステージのID
    public int score { get; set; } //スコア
}
