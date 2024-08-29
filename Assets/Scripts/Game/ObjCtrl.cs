using UnityEngine;
using System.Collections;

public class ObjCtrl : MonoBehaviour
{
    public GameObject obj;

    //��]�p
    Vector2 sPos;   //�^�b�`�������W
    Quaternion sRot;//�^�b�`�����Ƃ��̉�]
    float wid, hei, diag;  //�X�N���[���T�C�Y
    float tx, ty;    //�ϐ�

    void Start()
    {
        wid = Screen.width;
        hei = Screen.height;
        diag = Mathf.Sqrt(Mathf.Pow(wid, 2) + Mathf.Pow(hei, 2));
    }

    void Update()
    {

        if (Input.touchCount > 0)
        {
            //��]
            Touch t1 = Input.GetTouch(0);
            if (t1.phase == TouchPhase.Began)
            {
                sPos = t1.position;
                sRot = obj.transform.rotation;
            }
            else if (t1.phase == TouchPhase.Moved || t1.phase == TouchPhase.Stationary)
            {
                tx = (t1.position.x - sPos.x) / wid; //���ړ���(-1<tx<1)
                ty = (t1.position.y - sPos.y) / hei; //�c�ړ���(-1<ty<1)
                obj.transform.rotation = sRot;
                obj.transform.Rotate(new Vector3(0, 0, -90 * tx), Space.World);
            }
        }
    }
}