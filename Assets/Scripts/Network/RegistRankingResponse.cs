////////////////////////////////////////////////////////////////
///
/// ランキングレスポンス
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistRankingResponse  
{
    [JsonProperty("user_id")]
    public int UserID { get; set; }
    [JsonProperty("stage_id")]
    public int StageID { get; set; }
    [JsonProperty("score")]
    public int Score { get; set; }
}
