////////////////////////////////////////////////////////////////
///
/// �����L���O���N�G�X�g
/// 
/// Aughter:�ؓc�W��
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class RegistRankingRequest
{

    [JsonProperty("user_id")]
    public int UserID { get; set; }
    [JsonProperty("score")]
    public int Score { get; set; }
    [JsonProperty("stage_id")]
    public int StageID { get; set; }
}
