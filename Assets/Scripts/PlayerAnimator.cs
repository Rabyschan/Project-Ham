using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// <YSA>

// �÷��̾� ĳ���͸� ����� �Է¿� ���� �ִϸ��̼��� �����ϴ� ��ũ��Ʈ
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
    private PlayerMovement playerMovement;
    AnimatorStateInfo stateInfo;

    bool isJumping = false;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    //GetBool("�Ķ����")�� ����� ���� Ȯ��
    //SetBool("�Ķ����",����)�� ����� �Ķ���� ���� ����

    private void Update()
    {
        stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        //isJumping = playerMovement.isJumping;
        JumpAnim(playerMovement.isJumping);
        //playerAnimator.SetBool("IsJumping", false);
        //{
        //playerAnimator.SetBool("IsWalking",true);
        //playerAnimator.SetBool("IsJumping", true);
        //playerAnimator.SetBool("Left", true);
        //playerAnimator.SetBool("Right", true);
        //playerAnimator.SetBool("ClimbStart", true);
        //playerAnimator.SetBool("IsClimbing", true);
        //}
    }

    private void WalkAnim()
    {

    }
    
    public void JumpAnim(bool isJumping)
    {
        playerAnimator.SetBool("IsJumping", true);

        // ���� �ִϸ��̼��� ���� ��
        if (stateInfo.IsName("Jump"))
        {
            playerAnimator.SetBool("IsJumping", false);  // �ִϸ��̼� �������� false�� ����
        }
    }

    private void LeftMoveAnim()
    {

    }
}
