using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// <YSA>

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
// 사용자의 입력값은 PlayerInput에서 받아올 예정
public class PlayerAnimator : MonoBehaviour
{
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

    private Animator playerAnimator; //플레이어 캐릭터의 애니메이터

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    //SetBool("파라미터",상태)을 사용해 파라미터 상태 변경

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
