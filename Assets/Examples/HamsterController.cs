using UnityEngine;

public class HamsterController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float sideSpeed = 0.8f;

    [SerializeField] HamsterAnimationController animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] _PlayerInput input;
    [SerializeField] bool isGrounded;

    private Vector2 InputMove => new Vector2(input.leftRight, input.frontBack);
    private bool    IsMoving  => InputMove != Vector2.zero;

    private void Awake()
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        Move();
        animator.SetMove(IsMoving);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
            Climb();

        else if (collision.collider.CompareTag("Ground"))
            Land();
            
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
            Land();
    }

    private void Climb()
    {
        if (isGrounded == false)
            return;

        isGrounded = false;

        animator.PlayClimb();
    }

    private void Land()
    {
        if (isGrounded)
            return;

        isGrounded = true;

        animator.PlayLand();
    }

    private void Move()
    {
        if (IsMoving == false) return;

        Vector3 right = transform.right;
        Vector3 up = transform.up;
        Vector3 moveDirection;

        if (isGrounded)
            moveDirection = (InputMove.y * transform.forward * moveSpeed + InputMove.x * right * sideSpeed) * Time.deltaTime;
        else
            moveDirection = (InputMove.y * up * moveSpeed + InputMove.x * right * sideSpeed) * Time.deltaTime;

        rb.MovePosition(rb.position + moveDirection);
    }
}
