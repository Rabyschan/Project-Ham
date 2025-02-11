using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<CAY> // <YSA>

// <YSA> PlayerInput ��ũ��Ʈ�� ����� frontBack, leftRight �Է� ���θ� �޾ƿ�

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �յ� �������� �ӵ�
    public float sideSpeed = 0.8f; // <YSA> �¿� ������ �ӵ�
    public float rotateSpeed = 180f; // ī�޶� ȸ�� �ӵ�
    public float jumpForce = 1f; // ���� ��

    // �ٴ� üũ��
    public bool isGrounded = false; // ���鿡 ��Ҵ��� Ȯ��
    private bool climbStart = false; // <YSA> �Ĺ��� ��� ������ ������ ���� Ȯ��

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private PlayerAnimator playerAnimator; // <YSA> �÷��̾� �Ķ���� Ȱ��ȭ�� �����ϴ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�

    // <YSA> PlayerInput ��ũ��Ʈ�� ����� frontBack, leftRight �Է� ���θ� �޾ƿ�
    public Vector2 InputMove => new Vector2(playerInput.leftRight, playerInput.frontBack);

    // <YSA> inputMove�� 0�� �ƴϸ� IsMoving �Լ��� ��ȯ
    private bool IsMoving => InputMove != Vector2.zero;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.useGravity = true;
        playerAnimator.playerCapsuleCollider.direction = 2;

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

    // <YSA> �¿� �̵�
    private void Move_LeftRight()
    {
        if (!IsMoving) return;

        if (isGrounded)
        {
            Vector3 moveDirection = InputMove.x * transform.right * sideSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDirection);
        }
    }

    // ȸ�� (���콺 �̵��� �������� ȸ��)
    private void MouseRotate()
    {
        // ȸ�� ó�� (���콺 �̵��� ���� ȸ��)
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + mouseX, 0);
    }

    public bool isJumping;
    public bool isClimbing;

    // <YSA> ����
    private void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump")&& !isJumping)
        {
            isJumping = true;
            // <YSA> ���� �Ķ���� Ȱ��ȭ (true)
            playerAnimator.SetBool("IsJumping", true);
            // ������ ������ٵ� ���� �����ݴϴ�.
            //playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //isJumping = false; // <YSA> ���� Ȯ�� ���� false�� ����
        }
        // <YSA> ���� �Ķ���� ��Ȱ��ȭ (false)
        playerAnimator.JumpAnim();
    }

    public void OnJump()
    {
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // <YSA> Ÿ�� ������
    private void Climb()
    {
        if (!isGrounded) return;

        playerAnimator.SetBool("ClimbStart", true);
        //isGrounded = false;
        isClimbing = true;
        //playerRigidbody.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);
        // <YSA> �÷��̾��� ĸ���ݶ��̴��� Y������ ����
        playerAnimator.playerCapsuleCollider.direction = 1;
        // <YSA> ������ٵ� �߷� ��Ȱ��ȭ
        playerRigidbody.useGravity = false;
        //
        playerRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        //playerAnimator.SetBool("ClimbStart", true);

        // <YSA> IsJumping, ClimbStart �Ķ���� ��Ȱ��ȭ (false) /IsClimbing �Ķ���� Ȱ��ȭ (true)
        playerAnimator.ClimbStartAnim();
    }
    public void OnClimb()
    {

    }

    // <YSA> �ٴ��� ��
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

    // <YSA> �ٴڰ� �΋H���� �� / ������ �΋H���� ��
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

    // <YSA> �������� ����� ��
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Wire"))
        {
            Ground();
            // <YSA> ������ �ݶ��̴� ��Ȱ��ȭ => �ö󰡸� ��Ȱ��ȭ�ϴ� ��ũ��Ʈ �ʿ�
        }
    }
}