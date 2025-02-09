using System;
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
    #region �Ķ���� ����
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
    #endregion

    private Animator playerAnimator; //�÷��̾� ĳ������ �ִϸ�����
    AnimatorStateInfo stateInfo;

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ

    public float frontBackAxis = 0f; // �յ� �Է°� ����
    public float leftRightAxis = 0f; // �¿� �Է°� ����

    private bool climbStart = false;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    // GetCurrentAnimatorStateInfo�� ����� ���̾� 0�� ���� Ȯ��
    private void Update()
    {
        stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
    }

    // SetBool("�Ķ����",����)�� ����� �ܺ� �Ķ���� Ȱ��ȭ ����
    public void SetBool(string paramName, bool value)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool(paramName, value);
        }
    }

    // �������� ���� �� IsWalking�� false�� ����
    public void WalkAnim(float frontBack)
    {
        // ������ �Է°�
        if (frontBack > 0 && frontBackAxis <= 0)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        // �ڷ� �Է°�
        else if (frontBack < 0 && frontBackAxis >= 0)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        // �Է°��� ���� ��
        else if (frontBack == 0 && frontBackAxis != 0)
        {
            playerAnimator.SetBool("IsWalking", false);
        }
        // ������ �Է°��� ����
        frontBackAxis = frontBack;
    }

    // ���� �ִϸ��̼��� Jump�� �� IsJumping�� false�� ����
    public void JumpAnim()
    {
        if (stateInfo.IsName("Jump"))
        {
            playerAnimator.SetBool("IsJumping", false);
        }
    }

    // �������� ���� �� Left�� Right�� false�� ����
    public void SideMoveAnim(float leftRight)
    {
        // �������� �Է°�
        if (leftRight > 0 && leftRightAxis <= 0)
        {
            playerAnimator.SetBool("Right", true);
        }
        // �������� �Է°�
        else if (leftRight < 0 && leftRightAxis >= 0)
        {
            playerAnimator.SetBool("Left", true);
        }
        // �Է°��� ���� ��
        else if (leftRight == 0 && leftRightAxis != 0)
        {
            playerAnimator.SetBool("Right", false);
            playerAnimator.SetBool("Left", false);
        }
        // ������ �Է°��� ����
        leftRightAxis = leftRight;
    }

    // ���� �ִϸ��̼� ���°� ClimbStart�϶�
    // IsJumping, ClimbStart �Ķ���� ��Ȱ��ȭ (false) /IsClimbing �Ķ���� Ȱ��ȭ (true)
    public void ClimbStartAnim()
    {
        if (stateInfo.IsName("ClimbStart"))
        {
            playerAnimator.SetBool("IsJumping", false);
            playerAnimator.SetBool("ClimbStart", false);
            playerAnimator.SetBool("IsClimbing", true);
        }
    }
}
