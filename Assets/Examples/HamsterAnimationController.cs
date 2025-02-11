using UnityEngine;

public class HamsterAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void SetMove(bool isMove)
    {
        animator.SetBool("IsMoving", isMove);
    }

    public void PlayClimb()
    {
        animator.SetTrigger("Climb");
    }

    public void PlayLand()
    {
        animator.SetTrigger("Land");
    }
}
