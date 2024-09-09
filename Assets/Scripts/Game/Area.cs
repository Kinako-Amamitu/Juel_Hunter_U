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
            // LineRenderer�̒��_���W�����[�J�����W�ɕϊ����Ď擾����
            Vector3 point = transform.InverseTransformPoint(transform.TransformPoint(lineRenderer.GetPosition(i)));
            points.Add(point);
        }
        // PolygonCollider2D�̃p�X��ݒ�
        GetComponent<EdgeCollider2D>().SetPoints(points);
    }
}
