using UnityEngine;

public class NormalEnemyShootBehaviour : StateMachineBehaviour
{
    AttackController AttackController_ref;
    float timer;
    [SerializeField] float AttackTime; 
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        AttackController_ref = animator.gameObject.GetComponent<AttackController>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer < AttackTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            AttackController_ref.BreatheFire(animator.transform.forward, animator.gameObject.transform, animator.transform.rotation * Quaternion.Euler(0, -90, 0));
            timer = 0;
            animator.SetTrigger("HasAttacked");
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
