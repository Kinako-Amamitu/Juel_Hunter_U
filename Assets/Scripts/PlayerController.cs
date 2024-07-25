using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;     // �R���|�[�l���g�̎擾�p
    private Animator anim;      // �R���|�[�l���g�̎擾�p

    private Vector3 lookDirection = new Vector3(0, -1.0f,0);      // �L�����̌����̏��̐ݒ�p


    private void Start()
    {

        TryGetComponent(out rb);
        TryGetComponent(out anim);
    }

    private void Update()
    {
        if (!anim)
        {
            return;
        }

        // �L�����̌����Ă�������ƈړ��A�j���̓���
        SyncMoveAnimation();
    }


    /// <summary>
    /// �L�����̌����Ă�������ƈړ��A�j���̓���
    /// </summary>
    private void SyncMoveAnimation()
    {
            // ���K��
            lookDirection.Normalize();

            // �L�[���͂̒l��BlendTree�ł����Ă������ړ��A�j���p�̒l���m�F���A�ړ��A�j�����Đ�
            anim.SetFloat("Look X", lookDirection.x);
            anim.SetFloat("Look Y", lookDirection.y);

            // �L�[���͂̒l��BlendTree�Őݒ肵���ړ��A�j���p�̒l���m�F���A�ړ��A�j�����Đ�
            anim.SetFloat("Speed", lookDirection.sqrMagnitude);
    }

    /// <summary>
    /// �v���C���[�̐i�s�����̎擾�p
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLookDirection()
    {

        return transform.forward;
    }
}
