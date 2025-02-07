using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// <YSA>

// �÷��̾� ĳ���͸� ����� �Է¿� ���� �����̴� ��ũ��Ʈ
// ������� �Է°��� PlayerInput���� �޾ƿ� ����
public class PlayerAnimator : MonoBehaviour
{
    /* <YSA> �÷��̾� ��ũ��Ʈ �Ķ����
     * IsWalking : W, S (��, ��)
     * IsJumping : ����
     * Left : A (��)
     * Right : D (��)
     * ClimbStart : Climb Sub State Machine ����
     * IsClimbing : �Ŵ޷��� ���� ����
    */

    /* <YSA> �ߺ� �Ķ���� �߰� ����
     * IsJumping(True)
     * => <if> ClimbStart(true) : Climb Sub State Machine ����
     * 
     * IsClimbing(true)
     * => <if> IsWalking(true) : Climbing(�ö󰡴� �ִϸ��̼�) ����
     */

    private Animator playerAnimator; //�÷��̾� ĳ������ �ִϸ�����

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    //SetBool("�Ķ����",����)�� ����� �Ķ���� ���� ����

    private void Update()
    {
        //if ()
        //{
        //playerAnimator.SetBool("IsWalking",true);
        //playerAnimator.SetBool("IsJumping", true);
        //playerAnimator.SetBool("Left", true);
        //playerAnimator.SetBool("Right", true);
        //playerAnimator.SetBool("ClimbStart", true);
        //playerAnimator.SetBool("IsClimbing", true);
        //}
    }
}
