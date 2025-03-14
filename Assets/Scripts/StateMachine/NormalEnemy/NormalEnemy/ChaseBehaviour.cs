using Unity.VisualScripting;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    float timer;
    public GameObject Target;
    Rigidbody rb;
    [SerializeField] float EnemySpeed = 5;
    [SerializeField] float ShootingRange = 10;
    [SerializeField] float RotationTime = 5;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        rb = animator.gameObject.GetComponent<Rigidbody>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Target == null)
        {
            animator.SetTrigger("TargetIsLost");
            return;
        }
        timer += Time.deltaTime;
        Vector3 Direction = (Target.transform.position - rb.position).normalized;
        Quaternion LookDirection = Quaternion.LookRotation((Target.transform.position - animator.gameObject.transform.position).normalized);
        animator.gameObject.transform.rotation = Quaternion.Slerp(animator.gameObject.transform.rotation, LookDirection, RotationTime * Time.deltaTime);
        rb.MovePosition(rb.position + (Direction * EnemySpeed * Time.deltaTime));
        Debug.LogError(Vector3.Distance(animator.transform.position, Target.transform.position));
        if (Vector3.Distance(animator.transform.position, Target.transform.position) <= ShootingRange)
        {
            animator.SetBool("bIsInRange", true);
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
