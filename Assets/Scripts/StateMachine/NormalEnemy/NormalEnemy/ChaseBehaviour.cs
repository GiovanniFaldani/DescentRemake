using Unity.VisualScripting;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    float CheckObstaclesTimer;
    public GameObject Target;
    Rigidbody rb;
    [SerializeField] float EnemySpeed = 5;
    [SerializeField] float ShootingRange = 10;
    [SerializeField] float RotationTime = 5;
    Vector3 AvoidObstacleDirection;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckObstaclesTimer = 0;
        rb = animator.gameObject.GetComponent<Rigidbody>();
        if (Vector3.Distance(animator.transform.position, Target.transform.position) <= ShootingRange)
        {
            animator.SetBool("bIsInRange", true);
        }
        else
        {

            animator.SetBool("bIsInRange", false);
        }
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Target == null)
        {
            animator.SetTrigger("TargetIsLost");
            return;
        }


        //Aggiornamento direzione
        Vector3 Direction;
        if (AvoidObstacleDirection != Vector3.zero)
        {
            Direction = AvoidObstacleDirection;
        }
        else
        {

            Direction = (Target.transform.position - rb.position).normalized;
        }
            //Direction = (Direction + AvoidObstacleDirection).normalized;

            //Gestione degli ostacoli
            CheckObstaclesTimer += Time.deltaTime;
        if (CheckObstaclesTimer > 0.2f)
        {
            //Cambio direzione per evitare l'ostacolo
            //CheckForObstacles(animator);
            CheckObstaclesTimer = 0;
        }
        CheckForObstacles(animator);

        //Applicazione movimento
        rb.MovePosition(rb.position + (Direction * EnemySpeed * Time.deltaTime));
        if (Vector3.Distance(animator.transform.position, Target.transform.position) <= ShootingRange)
        {
            animator.SetBool("bIsInRange", true);
        }


        //Aggiornamento e applicazione rotazione
        Quaternion LookDirection = Quaternion.LookRotation(Direction);
        animator.gameObject.transform.rotation = Quaternion.Slerp(animator.gameObject.transform.rotation, LookDirection, RotationTime * Time.deltaTime);
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

    void CheckForObstacles(Animator animator)
    {
        

            //Crea un Raycast per controllare che tra nemico e Target che ci sia un'ostacolo
            RaycastHit HitObject;
            Vector3 RayOrigin = animator.transform.position + animator.transform.forward * 0.6f;
            //Debug.DrawLine(RayOrigin, Target.transform.position, new Color32(255, 0, 0, 255), 25);
            //Physics.Raycast(RayOrigin, Target.transform.position, out HitObject, 25);

        if (Physics.SphereCast(RayOrigin, 1, Target.transform.position, out HitObject, 2.5f))
        {
            Debug.LogError("Ostacolo rilevato");
            //Se è presente un'oggetto che non sia player di fronte a se
            if (!HitObject.rigidbody.CompareTag("Player"))
            {
                //controllo se la via a destra è libera
                RaycastHit HitObjectDx;
                Vector3 DXDirextion = (animator.transform.forward + animator.transform.right).normalized;
                Debug.DrawLine(RayOrigin, RayOrigin + DXDirextion * 5, new Color32(0, 255, 0, 255), 5);
                if (!Physics.Linecast(RayOrigin, RayOrigin + DXDirextion * 5, out HitObjectDx, 5))
                {
                    AvoidObstacleDirection = DXDirextion;
                    Debug.LogError("Direzione Scelta(DX1) : " + AvoidObstacleDirection);
                }
                else if (!Physics.Linecast(RayOrigin, RayOrigin + Vector3.right * 5, out HitObjectDx, 5))
                {
                    AvoidObstacleDirection = animator.transform.right;
                    Debug.LogError("Direzione Scelta(DX2) : " + AvoidObstacleDirection);
                }

                //controllo se la via a sinistra è libera
                RaycastHit HitObjectSX;
                Vector3 SXDirextion = (animator.transform.forward + (animator.transform.right * -1)).normalized;
                Debug.DrawLine(RayOrigin, RayOrigin + SXDirextion * 5, new Color32(0, 0, 255, 255), 5);
                if (!Physics.Linecast(RayOrigin, RayOrigin + SXDirextion * 5, out HitObjectSX, 5))
                {
                    AvoidObstacleDirection = SXDirextion;
                    Debug.LogError("Direzione Scelta(SX1) : " + AvoidObstacleDirection);
                }
                else if (!Physics.Linecast(RayOrigin, RayOrigin + Vector3.left * 5, out HitObjectSX, 5))
                {
                    AvoidObstacleDirection = animator.transform.right * -1;
                    Debug.LogError("Direzione Scelta(SX2) : " + AvoidObstacleDirection);
                }
            }
        }
        else
        {
            AvoidObstacleDirection = Vector3.zero;
        }
    }
}
