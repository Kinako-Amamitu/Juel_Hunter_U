////////////////////////////////////////////////////////////////
///
/// ユーザーのレスポンス
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class RegistUserResponse
{
 [JsonProperty("user_id")]
 public int UserID { get; set; }

    [JsonProperty("token")]
public string Authtoken { get; set; }
}
