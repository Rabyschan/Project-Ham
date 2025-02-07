using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<CAY> // <YSA>

// <YSA> PlayerInput 스크립트를 사용해 frontBack, leftRight 입력 여부를 받아옴

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // <YSA> 카메라 회전 속도
    public float jumpForce = 5f; // 점프 힘

    // 바닥 체크용
    public bool isGrounded = false; // 지면에 닿았는지 확인
    public bool isJumping = false;

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();

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
        Vector3 moveDistance = transform.forward * playerInput.frontBack * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // <YSA> 좌우 이동
    private void Move_LeftRight()
    {
        Vector3 moveDistance = transform.right * playerInput.leftRight * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 회전 (마우스 이동을 기준으로 회전)
    private void MouseRotate()
    {
        // 회전 처리 (마우스 이동에 의한 회전)
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + mouseX, 0);
    }

    // 점프
    private void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // <YSA> 점프 유무를 저장하는 변수
            isJumping = true;
            // 점프는 리지드바디에 힘을 더해줍니다.
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // <YSA>지면 확인 변수 false로 설정
            isJumping = false;
        }
            
    }

    // 바닥에 닿았는지 확인 (Trigger 사용)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) // 바닥 태그를 비교합니다.
        {
            isGrounded = true; // 바닥에 닿으면 true로 설정
        }
    }

    // 바닥에서 떨어졌을 때 (선택사항: 지면을 벗어난 상태에서)
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Ground")) // 바닥 태그를 비교합니다.
    //    {
    //        isGrounded = false; // 지면에서 떨어지면 false로 설정
    //    }
    //}
}