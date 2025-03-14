using UnityEngine;

public class EnvironmentalAwarenessController : MonoBehaviour
{
    GameObject Target;
    [SerializeField] float TargetLoseTime;
    bool b_PlayerIsOutOfCollider;
    bool b_PlayerIsNotInLineOfSight;
    bool b_IsLookingAnObstacle;
    float timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            RaycastHit HitObject;
            Vector3 RayOrigin = transform.position + transform.forward * 0.6f;
            Debug.DrawLine(RayOrigin, Target.transform.position, new Color32(252, 3, 3, 255), 25);
            Physics.Raycast(RayOrigin, Target.transform.position, out HitObject, 25);
            if (!HitObject.collider.CompareTag("Player"))
            {
                timer += Time.deltaTime;
                if (timer >= TargetLoseTime)
                {
                    Target = null;
                    gameObject.GetComponent<Animator>().GetBehaviour<ChaseBehaviour>().Target = null;
                    gameObject.GetComponent<Animator>().SetTrigger("TargetIsLost");
                    timer = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0;
            Target = other.gameObject;
            gameObject.GetComponent<Animator>().GetBehaviour<ChaseBehaviour>().Target = other.gameObject;
            gameObject.GetComponent<Animator>().SetTrigger("TargetAcquired");
            b_PlayerIsOutOfCollider = false;
        }
        else
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            b_PlayerIsOutOfCollider = true;
        }
    }
}
