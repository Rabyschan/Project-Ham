using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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

    private _PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private PlayerMovement playerMovement; //�÷��̾� �̵��� �˷��ִ� ������Ʈ
    public CapsuleCollider playerCapsuleCollider; // <YSA> �÷��̾� ĳ������ ĸ�� �ݶ��̴�

    public float frontBackAxis = 0f; // �յ� �Է°� ����
    public float leftRightAxis = 0f; // �¿� �Է°� ����
    public float upDownAxis = 0f; // ���Ʒ� �Է°� ����



    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();

        playerInput = GetComponent<_PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
    }
    // GetCurrentAnimatorStateInfo�� ����� ���̾� 0�� ���� Ȯ��
    private void Update()
    {
        stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if (playerMovement == null) return;
        MovingAnim();
    }

    // SetBool("�Ķ����",����)�� ����� �ܺ� �Ķ���� Ȱ��ȭ ����
    public void SetBool(string paramName, bool value)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool(paramName, value);
        }
    }

    private void MovingAnim()
    {
        if (playerMovement.isGrounded)
        {
            WalkAnim(playerMovement.InputMove.y);
        }
        else
        {
            ClimbAnim(playerMovement.InputMove.y);
        }

        SideMoveAnim(playerMovement.InputMove.x);
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
            playerMovement.isJumping = false;
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

    /* ���� �ִϸ��̼� ���°� ClimbStart�϶�
     * IsJumping, ClimbStart, IsWalking �Ķ���� ��Ȱ��ȭ(false)
     * IsClimbing �Ķ���� Ȱ��ȭ(true) */
    public void ClimbStartAnim()
    {
        if (stateInfo.IsName("ClimbStart"))
        {
            playerAnimator.SetBool("IsClimbing", true);
        }
        else if (stateInfo.IsName("ClimbingIdle"))
        {
            playerAnimator.SetBool("ClimbStart", false);
        }
        else if (stateInfo.IsName("ClimbEnd"))
        {
            playerAnimator.SetBool("ClimbStart", false);
        }
    }

    public void ClimbAnim(float upDown)
    {
        // ���� �Է°�
        if (upDown > 0 && upDownAxis <= 0)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        // �Ʒ��� �Է°�
        else if (upDown < 0 && upDownAxis >= 0)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        // �Է°��� ���� ��
        else if (upDown == 0 && upDownAxis != 0)
        {
            playerAnimator.SetBool("IsWalking", false);
        }
        // ������ �Է°��� ����
        upDownAxis = upDown;
    }
}
