////////////////////////////////////////////////////////////////
///
/// レンダラーを管理するスクリプト
/// 
/// Aughter:木田晃輔
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Vector2> points = new List<Vector2>();
        var lineRenderer = GetComponent<LineRenderer>();
        var transform = lineRenderer.transform;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            // LineRendererの頂点座標をローカル座標に変換して取得する
            Vector3 point = transform.InverseTransformPoint(transform.TransformPoint(lineRenderer.GetPosition(i)));
            points.Add(point);
        }
        // PolygonCollider2Dのパスを設定
        GetComponent<EdgeCollider2D>().SetPoints(points);
    }
}
