using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<CAY> // <YSA>

// <YSA> PlayerInput ��ũ��Ʈ�� ����� frontBack, leftRight �Է� ���θ� �޾ƿ�

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �յ� �������� �ӵ�
    public float sideSpeed = 0.5f; // <YSA> �¿� ������ �ӵ�
    public float rotateSpeed = 180f; // ī�޶� ȸ�� �ӵ�
    public float jumpForce = 5f; // ���� ��

    // �ٴ� üũ��
    private bool isGrounded = false; // ���鿡 ��Ҵ��� Ȯ��
    private bool climbStart = false;

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private PlayerAnimator playerAnimator; // <YSA> �÷��̾� �Ķ���� Ȱ��ȭ�� �����ϴ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<PlayerAnimator>();
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
        ClimbStart();
    }

    // <YSA> ���� �̵�
    private void Move_FrontBack()
    {
        if (!climbStart)
        {
            Vector3 moveDistance = transform.forward * playerInput.frontBack * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        }
        else
        {
            //playerRigidbody.MovePosition(playerRigidbody.position);
        }

        playerAnimator.WalkAnim(playerInput.frontBack);
    }

    // <YSA> �¿� �̵�
    private void Move_LeftRight()
    {
        Vector3 moveDistance = transform.right * playerInput.leftRight * sideSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        playerAnimator.SideMoveAnim(playerInput.leftRight);
    }

    // ȸ�� (���콺 �̵��� �������� ȸ��)
    private void MouseRotate()
    {
        // ȸ�� ó�� (���콺 �̵��� ���� ȸ��)
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + mouseX, 0);
    }

    // <YSA> ����
    private void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // <YSA> ���� �Ķ���� Ȱ��ȭ (true)
            playerAnimator.SetBool("IsJumping", true);
            // ������ ������ٵ� ���� �����ݴϴ�.
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // <YSA> ���� Ȯ�� ���� false�� ����
        }
        // <YSA> ���� �Ķ���� ��Ȱ��ȭ (false)
        playerAnimator.JumpAnim();
    }

    // <YSA> Ÿ�� ������
    private void ClimbStart()
    {
        // <YSA> �Ĺ��� ����� ��/ ������ ������ ��
        if (climbStart && Input.GetButtonDown("Jump"))
        {
            // <YSA> Climb Sub-State Machine Ȱ��ȭ (true)
            playerAnimator.SetBool("ClimbStart", true);
        }
        // <YSA> IsJumping, ClimbStart �Ķ���� ��Ȱ��ȭ (false) /IsClimbing �Ķ���� Ȱ��ȭ (true)
        playerAnimator.ClimbStartAnim();
    }

    // �ٴڿ� ��Ҵ��� Ȯ�� (Trigger ���) // <YSA> �Ĺ��� ��Ҵ��� Ȯ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) // �ٴ� �±׸� ���մϴ�.
        {
            isGrounded = true; // �ٴڿ� ������ true�� ����
        }
        else if (other.CompareTag("Plants")) // <YSA> �Ĺ��±����� Ȯ��
        {
            climbStart = true; // <YSA> �Ĺ��� ������ true�� ����
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