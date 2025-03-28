using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    float timer;
    [SerializeField] float PatrolWaitingTime;
    [SerializeField] float EnemySpeed;
    [SerializeField] float MaxDistance;
    Rigidbody rb;
    Vector3 Destination;
    float NewYRotation;
    float CurrentRotationSpeed;
    [SerializeField, Range(0.1f, 1f)] float RotationSpeed;
    bool b_HasTurned = false;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        NewYRotation = animator.gameObject.transform.eulerAngles.y;
        rb = animator.gameObject.GetComponent<Rigidbody>();

        RaycastHit HitObject;
        Vector3 RayOrigin = animator.transform.position + animator.transform.forward * 0.6f;
        Debug.DrawLine(RayOrigin, animator.transform.position + animator.transform.forward * MaxDistance, new Color32(252, 3, 3, 255), MaxDistance);
        Physics.Raycast(RayOrigin, animator.transform.forward, out HitObject, MaxDistance);
        float dist = Vector3.Distance(animator.transform.position, HitObject.point);
        float rnd = Random.Range(0.1f, dist - 2);
        Destination = animator.transform.position + animator.gameObject.transform.forward * rnd;

    }
    
    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Esegue la rotazione fino a raggiungere la rotazione di destinazione
        if (!b_HasTurned)
        {
            float YAngle = Mathf.SmoothDampAngle(animator.gameObject.transform.eulerAngles.y, NewYRotation, ref CurrentRotationSpeed, RotationSpeed);
            animator.gameObject.transform.rotation = Quaternion.Euler(animator.gameObject.transform.rotation.x, YAngle, animator.gameObject.transform.rotation.z);
            if (animator.gameObject.transform.eulerAngles.y > NewYRotation - 5 && animator.gameObject.transform.eulerAngles.y < NewYRotation + 5)
            {
                animator.gameObject.transform.rotation = Quaternion.Euler(animator.gameObject.transform.rotation.x, NewYRotation, animator.gameObject.transform.rotation.z);
                b_HasTurned = true;
            }
        }
        else 
        {
            //Cerca una nuova destinazione
            if (Destination == Vector3.zero)
            {
                RaycastHit HitObject;
                Vector3 RayOrigin = animator.transform.position + animator.transform.forward * 0.6f;
                Debug.DrawLine(RayOrigin, animator.transform.position + animator.transform.forward * MaxDistance, new Color32(252, 3, 3, 255), MaxDistance);
                Physics.Raycast(RayOrigin, animator.transform.forward, out HitObject, MaxDistance);
                if (HitObject.point != null)
                {
                    float dist = Vector3.Distance(animator.transform.position, HitObject.point);
                    float rnd = Random.Range(0.1f, dist - 2);
                    Destination = animator.transform.position + animator.gameObject.transform.forward * rnd;
                }
                else
                {
                    float rnd = Random.Range(0.1f, MaxDistance);
                    Destination = animator.transform.position + animator.gameObject.transform.forward * rnd;
                }
            }
            //Raggiunge la nuova destinazione
            else
            {

                if (animator.transform.position != Destination)
                {
                    Vector3 Direction = (Destination - rb.position).normalized;
                    rb.MovePosition(rb.position + (Direction * EnemySpeed * Time.deltaTime));
                }
                //Una volta raggiunta la destinazione attende e cerca una nuova rotazione
                else
                {
                    timer += Time.deltaTime;
                    if (timer >= PatrolWaitingTime)
                    {
                        float RandomAngle = Random.Range(90, 270);
                        NewYRotation = animator.gameObject.transform.eulerAngles.y + RandomAngle;
                        if (NewYRotation > 360)
                        {
                            NewYRotation -= 360;
                        }
                        timer = 0;
                        b_HasTurned = false;
                        Destination = Vector3.zero;
                    }
                }
            }
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
