////////////////////////////////////////////////////////////////
///
/// ���e�I�u�W�F�N�g���Ǘ�����X�N���v�g
/// 
/// Aughter:�ؓc�W��
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMonobehaber : MonoBehaviour
{
    //Unity����A�^�b�`
    [SerializeField] GameObject bomb; //�{���v���n�u

    //�e�̃N���X�g�p
    Bullet bullet;


    private void Start()
    {

        bullet = GetComponent<Bullet>();



    }

    /// <summary>
    /// ���C����
    /// </summary>
    void OnDrawGizmos()
    {
        //�@CircleCast�̃��C������
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position , 1.0f );
    }

    /// <summary>
    /// �g���K�[�ڐG����
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Rail(right)" ||
            collision.gameObject.tag == "Rail(left)"||
            collision.gameObject.tag == "Rail(down)" ||
            collision.gameObject.tag == "Rail(down)(slow)" ||
            collision.gameObject.tag == "Rail(up)"||
            collision.gameObject.tag == "Enemy")
        {//���[�����G�ɂ�������

            //����
            bullet.Explosion(bomb);
            
        }
    }
}
