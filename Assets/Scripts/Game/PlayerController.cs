using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameGenerator gameGenerator;
    [SerializeField] string playerTag;
    [SerializeField] int playerNum;


    


    private void Start()
    {


        

    }

    private void Update()
    {
        /* �}�E�X�N���b�N�����m */
        if (Input.GetMouseButtonDown(0))
        {

            // �^�b�v�����ꏊ����Ray���쐬
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast���쐬
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            //Ray�������ɏՓ˂������Ƃ����m & �Փ˂����Ώۂ��������g���𔻕�
            if (hit2d && hit2d.transform.gameObject.tag == playerTag)
            {
                
                gameGenerator.FireBullet(playerNum);



            }

        }
    }

    private void FixedUpdate()
    {
        
    }
    /// <summary>
    /// �v���C���[�̐i�s�����̎擾�p
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLookDirection()
    {
        Vector3 direction = new Vector3(-Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad));
        return direction;
    }

}
