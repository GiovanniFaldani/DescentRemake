using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    float timer = 0;
    [SerializeField] float IdleRotationWaitingTime;
    [Range (0f, 359.9f)] public float YRotationAngle;
    float CurrentRotationSpeed;
    [SerializeField, Range(0.1f,1f)] float RotationSpeed;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (animator.gameObject.transform.rotation.y != YRotationAngle)
        {
            float YAngle = Mathf.SmoothDampAngle(animator.gameObject.transform.eulerAngles.y, YRotationAngle,ref CurrentRotationSpeed, RotationSpeed);
            animator.gameObject.transform.rotation = Quaternion.Euler(animator.gameObject.transform.rotation.x, YAngle, animator.gameObject.transform.rotation.z);
        }
        if (timer >= IdleRotationWaitingTime)
        {
            YRotationAngle = Random.RandomRange(0f, 359.9f);
            timer = 0;
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
