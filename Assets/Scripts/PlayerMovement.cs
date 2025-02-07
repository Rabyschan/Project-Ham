using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<CAY> // <YSA>

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �յ� �������� �ӵ�
    public float rotateSpeed = 180f; // �¿� ȸ�� �ӵ�
    public float jumpForce = 5f; // ���� ��

    // �ٴ� üũ��
    private bool isGrounded = false; // ���鿡 ��Ҵ��� Ȯ��

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
        // ���� ���� ���� �ֱ⸶�� ĳ������ �����Ӱ� ȸ�� ���¸� ������Ʈ�Ѵ�.
        Move();
        Rotate();

        // ���� ó�� (���鿡 ���� ����)
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
            Debug.Log("����");
        }
        else
        {
            Debug.Log("Not Grounded");
        }
    }

    // �̵�
    private void Move()
    {
        Vector3 moveDistance = transform.forward * playerInput.move * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // ȸ�� (���콺 �̵��� �������� ȸ��)
    private void Rotate()
    {
        // ȸ�� ó�� (���콺 �̵��� ���� ȸ��)
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + mouseX, 0);
    }

    // ����
    private void Jump()
    {
        // ������ ������ٵ� ���� �����ݴϴ�.
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground")) // �ٴ� �±׸� ���մϴ�.
        {
            isGrounded = false; // ���鿡�� �������� false�� ����
        }
    }
}