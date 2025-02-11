using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<CAY> // <YSA>

// <YSA> PlayerInput 스크립트를 사용해 frontBack, leftRight 입력 여부를 받아옴

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float sideSpeed = 0.8f; // <YSA> 좌우 움직임 속도
    public float rotateSpeed = 180f; // 카메라 회전 속도
    public float jumpForce = 1f; // 점프 힘

    // 바닥 체크용
    public bool isGrounded = false; // 지면에 닿았는지 확인
    private bool climbStart = false; // <YSA> 식물에 닿아 오르길 시작할 건지 확인

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private PlayerAnimator playerAnimator; // <YSA> 플레이어 파라미터 활성화를 관리하는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디

    // <YSA> PlayerInput 스크립트를 사용해 frontBack, leftRight 입력 여부를 받아옴
    public Vector2 InputMove => new Vector2(playerInput.leftRight, playerInput.frontBack);

    // <YSA> inputMove가 0이 아니면 IsMoving 함수를 반환
    private bool IsMoving => InputMove != Vector2.zero;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.useGravity = true;
        playerAnimator.playerCapsuleCollider.direction = 2;

        // 커서 잠금 상태 설정
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        // <YSA> 물리 엔진 갱신 주기마다 캐릭터의 움직임과 마우스 회전 상태를 업데이트한다.
        Move_FrontBack();
        Move_LeftRight();
        MouseRotate();
        Jump();
    }

    // <YSA> 상하 이동
    private void Move_FrontBack()
    {
        if (!IsMoving) return;
        
        Vector3 moveDirection;
        if (isGrounded)
        {
            moveDirection = InputMove.y * transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            moveDirection = InputMove.y * transform.up * moveSpeed * Time.deltaTime;
        }
        playerRigidbody.MovePosition(playerRigidbody.position + moveDirection);
    }

    // <YSA> 좌우 이동
    private void Move_LeftRight()
    {
        if (!IsMoving) return;

        if (isGrounded)
        {
            Vector3 moveDirection = InputMove.x * transform.right * sideSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDirection);
        }
    }

    // 회전 (마우스 이동을 기준으로 회전)
    private void MouseRotate()
    {
        // 회전 처리 (마우스 이동에 의한 회전)
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + mouseX, 0);
    }

    // <YSA> 점프
    private void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // <YSA> 점프 파라미터 활성화 (true)
            playerAnimator.SetBool("IsJumping", true);
            // 점프는 리지드바디에 힘을 더해줍니다.
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // <YSA> 지면 확인 변수 false로 설정
        }
        // <YSA> 점프 파라미터 비활성화 (false)
        playerAnimator.JumpAnim();
    }

    // <YSA> 타고 오르기
    private void Climb()
    {
        if (!isGrounded) return;

        playerAnimator.SetBool("ClimbStart", true);
        isGrounded = false;
        //playerRigidbody.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);
        // <YSA> 플레이어의 캡슐콜라이더를 Y축으로 변경
        playerAnimator.playerCapsuleCollider.direction = 1;
        // <YSA> 리지드바디 중력 비활성화
        playerRigidbody.useGravity = false;
        //
        playerRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        //playerAnimator.SetBool("ClimbStart", true);

        // <YSA> IsJumping, ClimbStart 파라미터 비활성화 (false) /IsClimbing 파라미터 활성화 (true)
        playerAnimator.ClimbStartAnim();
    }

    // <YSA> 바닥일 때
    private void Ground()
    {
        if (isGrounded) return;

        isGrounded = true;

        playerAnimator.playerCapsuleCollider.direction = 2;

        playerRigidbody.useGravity = true;

        playerRigidbody.constraints = RigidbodyConstraints.None;
        playerAnimator.SetBool("IsClimbing", false);
        playerAnimator.SetBool("ClimbStart", false);
    }

    // <YSA> 바닥과 부딫혔을 때 / 전선과 부딫혔을 때
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Ground();
        }
        else if (collision.collider.CompareTag("Wire"))
        {
            Climb();
        }
    }

    // <YSA> 전선에서 벗어났을 때
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Wire"))
        {
            Ground();
            // <YSA> 전선의 콜라이더 비활성화 => 올라가면 비활성화하는 스크립트 필요
        }
    }
    /* ontrigger
    // 바닥에 닿았는지 확인 (Trigger 사용) // <YSA> 식물과 닿았는지 확인
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) // 바닥 태그를 비교합니다.
        {
            isGrounded = true; // 바닥에 닿으면 true로 설정
            playerAnimator.isClimbStartZone = false;
            climbStart = false;
            playerRigidbody.useGravity = true;
            playerRigidbody.constraints = RigidbodyConstraints.None;
        }
        else if (other.CompareTag("Plants")) // <YSA> 식물태그인지 확인
        {
            climbStart = true; // <YSA> 식물에 닿으면 true로 설정
            playerAnimator.frontBackAxis = 0;
            playerAnimator.isClimbStartZone = true;
            playerRigidbody.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Plants"))
        {
            //playerRigidbody.useGravity = true;
            playerAnimator.SetBool("ClimbStart", false);
            playerAnimator.isClimbStartZone = false;
            climbStart = false;
        }
    }
    
     바닥에서 떨어졌을 때 (선택사항: 지면을 벗어난 상태에서)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground")) // 바닥 태그를 비교합니다.
        {
            isGrounded = false; // 지면에서 떨어지면 false로 설정
        }
    }
    */
}