////////////////////////////////////////////////////////////////
///
/// �|�[�Y�@�\���Ǘ�����X�N���v�g
/// 
/// Aughter:�ؓc�W��
///
////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pose : MonoBehaviour
{
    //Unity����A�^�b�`
    [SerializeField] GameObject Panel;

    //�Q�[���}�l�[�W���[���g��
    GameGenerator gameGenerator;

    // Start is called before the first frame update
    void Start()
    {
        gameGenerator = GameObject.Find("GameGenerator").GetComponent<GameGenerator>();
    }

    //�|�[�Y�@�\
    public void Posejoin()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameGenerator.Pose();
        }
        
    }
}
