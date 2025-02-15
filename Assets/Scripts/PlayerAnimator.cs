using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

// <YSA>

// 플레이어 캐릭터를 사용자 입력에 따라 애니메이션을 실행하는 스크립트
// 사용자의 입력값은 PlayerInput에서 받아올 예정
public class PlayerAnimator : MonoBehaviour
{
    #region 파라미터 조건
    /* <YSA> 플레이어 스크립트 파라미터
     * IsWalking : W, S (앞, 뒤)
     * IsJumping : 점프
     * Left : A (좌)
     * Right : D (우)
     * ClimbStart : Climb Sub State Machine 실행
     * IsClimbing : 매달렸을 때의 조건
    */

    /* <YSA> 중복 파라미터 추가 조건
     * IsJumping(True)
     * => <if> ClimbStart(true) : Climb Sub State Machine 실행
     * 
     * IsClimbing(true)
     * => <if> IsWalking(true) : Climbing(올라가는 애니메이션) 실행
     */
    #endregion

    private Animator playerAnimator; //플레이어 캐릭터의 애니메이터
    AnimatorStateInfo stateInfo;

    private _PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private PlayerMovement playerMovement; //플레이어 이동을 알려주는 컴포넌트
    public CapsuleCollider playerCapsuleCollider; // <YSA> 플레이어 캐릭터의 캡슐 콜라이더

    public float frontBackAxis = 0f; // 앞뒤 입력값 저장
    public float leftRightAxis = 0f; // 좌우 입력값 저장
    public float upDownAxis = 0f; // 위아래 입력값 저장



    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();

        playerInput = GetComponent<_PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
    }
    // GetCurrentAnimatorStateInfo를 사용해 레이어 0의 상태 확인
    private void Update()
    {
        stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if (playerMovement == null) return;
        MovingAnim();
    }

    // SetBool("파라미터",상태)을 사용해 외부 파라미터 활성화 변경
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

    // 움직임이 없을 때 IsWalking을 false로 변경
    public void WalkAnim(float frontBack)
    {
        // 앞으로 입력값
        if (frontBack > 0 && frontBackAxis <= 0)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        // 뒤로 입력값
        else if (frontBack < 0 && frontBackAxis >= 0)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        // 입력값이 없을 때
        else if (frontBack == 0 && frontBackAxis != 0)
        {
            playerAnimator.SetBool("IsWalking", false);
        }
        // 마지막 입력값을 저장
        frontBackAxis = frontBack;
    }

    // 현재 애니메이션이 Jump일 때 IsJumping을 false로 변경
    public void JumpAnim()
    {
        if (stateInfo.IsName("Jump"))
        {
            playerAnimator.SetBool("IsJumping", false);
            playerMovement.isJumping = false;
        }
    }

    // 움직임이 없을 때 Left와 Right를 false로 변경
    public void SideMoveAnim(float leftRight)
    {
        // 우측으로 입력값
        if (leftRight > 0 && leftRightAxis <= 0)
        {
            playerAnimator.SetBool("Right", true);
        }
        // 좌측으로 입력값
        else if (leftRight < 0 && leftRightAxis >= 0)
        {
            playerAnimator.SetBool("Left", true);
        }
        // 입력값이 없을 때
        else if (leftRight == 0 && leftRightAxis != 0)
        {
            playerAnimator.SetBool("Right", false);
            playerAnimator.SetBool("Left", false);
        }
        // 마지막 입력값을 저장
        leftRightAxis = leftRight;
    }

    /* 현재 애니메이션 상태가 ClimbStart일때
     * IsJumping, ClimbStart, IsWalking 파라미터 비활성화(false)
     * IsClimbing 파라미터 활성화(true) */
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
        // 위로 입력값
        if (upDown > 0 && upDownAxis <= 0)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        // 아래로 입력값
        else if (upDown < 0 && upDownAxis >= 0)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        // 입력값이 없을 때
        else if (upDown == 0 && upDownAxis != 0)
        {
            playerAnimator.SetBool("IsWalking", false);
        }
        // 마지막 입력값을 저장
        upDownAxis = upDown;
    }
}
