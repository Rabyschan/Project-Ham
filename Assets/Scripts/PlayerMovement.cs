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
    [SerializeField]private bool isGrounded = false; // ���鿡 ��Ҵ��� Ȯ��
    private bool climbStart = false; // <YSA> �Ĺ��� ��� ������ ������ ���� Ȯ��
    private bool isClimbing = false;

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private PlayerAnimator playerAnimator; // <YSA> �÷��̾� �Ķ���� Ȱ��ȭ�� �����ϴ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.useGravity = true;
        playerAnimator.playerCapsuleCollider.direction = 2;

        // Ŀ�� ��� ���� ����
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //playerRigidbody.velocity = Vector3.zero;
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
        if (!isClimbing)
        {
            Vector3 moveDistance = transform.forward * playerInput.frontBack * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
            playerAnimator.WalkAnim(playerInput.frontBack);
        }
        else
        {
            Vector3 upDistance = transform.up * playerInput.frontBack * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + upDistance);
            playerAnimator.ClimbAnim(playerInput.frontBack);
        }
    }

    // <YSA> �¿� �̵�
    private void Move_LeftRight()
    {
        if (!isClimbing)
        {
            Vector3 moveDistance = transform.right * playerInput.leftRight * sideSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
            playerAnimator.SideMoveAnim(playerInput.leftRight);
        }
    }

    // ȸ�� (���콺 �̵��� �������� ȸ��)
    private void MouseRotate()
    {
        //if (!climbStart)
        //{
            // ȸ�� ó�� (���콺 �̵��� ���� ȸ��)
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + mouseX, 0);
        //}
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
        // <YSA> �Ĺ��� ����� ��
        if (climbStart)
        {
            // <YSA> �÷��̾��� ĸ���ݶ��̴��� Y������ ����
            playerAnimator.playerCapsuleCollider.direction = 1;
            playerRigidbody.useGravity = false;
            playerRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            // <YSA> Climb Sub-State Machine Ȱ��ȭ (true)
            playerAnimator.SetBool("ClimbStart", true);
            isGrounded = false;
        }
        // <YSA> IsJumping, ClimbStart �Ķ���� ��Ȱ��ȭ (false) /IsClimbing �Ķ���� Ȱ��ȭ (true)
        playerAnimator.ClimbStartAnim();
    }

    // �ٴڿ� ��Ҵ��� Ȯ�� (Trigger ���) // <YSA> �Ĺ��� ��Ҵ��� Ȯ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) // �ٴ� �±׸� ���մϴ�.
        {
            Debug.Log("�ٴ��Դϴ�");
            isGrounded = true; // �ٴڿ� ������ true�� ����
            playerAnimator.isClimbStartZone = false;
            climbStart = false;
            isClimbing = false;
            playerRigidbody.useGravity = true;
            playerRigidbody.constraints = RigidbodyConstraints.None;
        }
        else if (other.CompareTag("Plants")) // <YSA> �Ĺ��±����� Ȯ��
        {
            climbStart = true; // <YSA> �Ĺ��� ������ true�� ����
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
            isClimbing = true;
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