using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<CAY> // <YSA>

// <YSA> PlayerInput ��ũ��Ʈ�� ����� frontBack, leftRight �Է� ���θ� �޾ƿ�

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �յ� �������� �ӵ�
    public float rotateSpeed = 180f; // <YSA> ī�޶� ȸ�� �ӵ�
    public float jumpForce = 5f; // ���� ��

    // �ٴ� üũ��
    public bool isGrounded = false; // ���鿡 ��Ҵ��� Ȯ��
    public bool isJumping = false;

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();

        // Ŀ�� ��� ���� ����
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        // <YSA> ���� ���� ���� �ֱ⸶�� ĳ������ �����Ӱ� ���콺 ȸ�� ���¸� ������Ʈ�Ѵ�.
        Move_FrontBack();
        Move_LeftRight();
        MouseRotate();
        Jump();
        
    }

    // <YSA> ���� �̵�
    private void Move_FrontBack()
    {
        Vector3 moveDistance = transform.forward * playerInput.frontBack * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // <YSA> �¿� �̵�
    private void Move_LeftRight()
    {
        Vector3 moveDistance = transform.right * playerInput.leftRight * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // ȸ�� (���콺 �̵��� �������� ȸ��)
    private void MouseRotate()
    {
        // ȸ�� ó�� (���콺 �̵��� ���� ȸ��)
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + mouseX, 0);
    }

    // ����
    private void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // <YSA> ���� ������ �����ϴ� ����
            isJumping = true;
            // ������ ������ٵ� ���� �����ݴϴ�.
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // <YSA>���� Ȯ�� ���� false�� ����
            isJumping = false;
        }
            
    }

    // �ٴڿ� ��Ҵ��� Ȯ�� (Trigger ���)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) // �ٴ� �±׸� ���մϴ�.
        {
            isGrounded = true; // �ٴڿ� ������ true�� ����
        }
    }

    // �ٴڿ��� �������� �� (���û���: ������ ��� ���¿���)
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Ground")) // �ٴ� �±׸� ���մϴ�.
    //    {
    //        isGrounded = false; // ���鿡�� �������� false�� ����
    //    }
    //}
}