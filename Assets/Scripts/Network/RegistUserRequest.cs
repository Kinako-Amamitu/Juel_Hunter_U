////////////////////////////////////////////////////////////////
///
/// ユーザーのリクエスト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class RegistUserRequest
{
    [JsonProperty("name")]
    public string Name { get; set; }
}
