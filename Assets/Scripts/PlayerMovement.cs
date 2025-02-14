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

    private _PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private PlayerAnimator playerAnimator; // <YSA> �÷��̾� �Ķ���� Ȱ��ȭ�� �����ϴ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�

    // <YSA> PlayerInput ��ũ��Ʈ�� ����� frontBack, leftRight �Է� ���θ� �޾ƿ�
    public Vector2 InputMove => new Vector2(playerInput.leftRight, playerInput.frontBack);

    // <YSA> inputMove�� 0�� �ƴϸ� IsMoving �Լ��� ��ȯ
    private bool IsMoving => InputMove != Vector2.zero;

    private void Awake()
    {
        playerInput = GetComponent<_PlayerInput>();
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
        if (!isGrounded || isClimbing) return;

        isClimbing = true;
        playerAnimator.SetBool("ClimbStart", true);
        //isGrounded = false;

        Debug.Log("IsClimbing Ȱ��ȭ"); // Ȯ�ο� �α�
        playerAnimator.SetBool("IsClimbing", true); // <--- ���Ⱑ ����Ǵ��� Ȯ��

        //playerRigidbody.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);
        // <YSA> �÷��̾��� ĸ���ݶ��̴��� Y������ ����
        playerAnimator.playerCapsuleCollider.direction = 1;
        // <YSA> ������ٵ� �߷� ��Ȱ��ȭ
        playerRigidbody.useGravity = false;
        //�¿� ����
        playerRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        //playerAnimator.SetBool("ClimbStart", true);

        // <YSA> IsJumping, ClimbStart �Ķ���� ��Ȱ��ȭ (false) /IsClimbing �Ķ���� Ȱ��ȭ (true)
        playerAnimator.ClimbStartAnim();

        // �ε巴�� �ö󰡱�
        StartCoroutine(ClimbCoroutine());
    }
    
    //<SYJ> climb() ȣ�� �� ClimbCoroutine�� ������ �ε巴�� ���� �̵��ϴ� ���
    private IEnumerator ClimbCoroutine()
    {
        float climbHeight = 0.1f; // ��ǥ ����
        float climbSpeed = 0.5f; // �̵� �ӵ�
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * climbHeight;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * climbSpeed;
            yield return null;
        }

        transform.position = targetPosition;
        isGrounded = false;
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

    //<SYJ>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wire")) // Wire�� �浹 ����
        {
            if (!isClimbing) // �̹� ��� ���̸� ���� �� ��
            {
                Debug.Log("Wire Ʈ���� ����, Climb() ����");
                Climb();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wire")) // Wire���� ����� ��
        {
            Debug.Log("Wire���� ���, ClimbEnd() ����");
            ClimbEnd();
        }
    }

    
    // <YSA> �ٴڰ� �΋H���� �� / ������ �΋H���� ��
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (!isClimbing) // ��� ���� �ƴ� ���� Ground ó��
            {
                Ground();
            }
        }
        /*
        else if (collision.collider.CompareTag("Wire"))
        {
            if (!isClimbing && !climbStart) // �̹� ��� ���̶�� Climb() ���� �� ��
            {
                Debug.Log("���� �浹 ����, Climb() ����");
                climbStart = true;
                Climb();
            }
        }
        */
    }
    
    /*
    // <YSA> �������� ����� ��
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Wire"))
        {
            
            ClimbEnd();
            
            // <YSA> ������ �ݶ��̴� ��Ȱ��ȭ => �ö󰡸� ��Ȱ��ȭ�ϴ� ��ũ��Ʈ �ʿ�

            climbStart = false; //wire���� ����� �ٽ� Climb �����ϵ��� �ʱ�ȭ
        }
    }
    */
    
    //<SYJ> ClimbEnd()�� ����� �����ϴ°�
    private void ClimbEnd()
    {
        if (!isClimbing) return; //�̹� ����� ����� ���¶�� ���� �� ��

        Debug.Log("ClimbEnd �����"); // Ȯ�ο� �α�

        isClimbing = false; //��� ����
        isGrounded = true;

        playerAnimator.SetBool("IsClimbing", false);
        playerAnimator.SetBool("ClimbStart", false);

        playerAnimator.playerCapsuleCollider.direction = 2;
        playerRigidbody.useGravity = true;

        playerRigidbody.constraints = RigidbodyConstraints.None;
    }
    
}