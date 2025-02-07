using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<CAY> // <YSA>

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도
    public float jumpForce = 5f; // 점프 힘

    // 바닥 체크용
    private bool isGrounded = false; // 지면에 닿았는지 확인

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
        // 물리 엔진 갱신 주기마다 캐릭터의 움직임과 회전 상태를 업데이트한다.
        Move();
        Rotate();

        // 점프 처리 (지면에 있을 때만)
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
            Debug.Log("점프");
        }
        else
        {
            Debug.Log("Not Grounded");
        }
    }

    // 이동
    private void Move()
    {
        Vector3 moveDistance = transform.forward * playerInput.move * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 회전 (마우스 이동을 기준으로 회전)
    private void Rotate()
    {
        // 회전 처리 (마우스 이동에 의한 회전)
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + mouseX, 0);
    }

    // 점프
    private void Jump()
    {
        // 점프는 리지드바디에 힘을 더해줍니다.
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground")) // 바닥 태그를 비교합니다.
        {
            isGrounded = false; // 지면에서 떨어지면 false로 설정
        }
    }
}