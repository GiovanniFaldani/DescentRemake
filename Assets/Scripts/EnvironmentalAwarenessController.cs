using UnityEngine;

public class EnvironmentalAwarenessController : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0;
            if (gameObject.GetComponentInParent<Animator>().GetBehaviour<BossChaseBehaviour>())
            {
                gameObject.GetComponentInParent<Animator>().GetBehaviour<BossChaseBehaviour>().Target = other.gameObject;
            }
            else
            {
                gameObject.GetComponentInParent<Animator>().GetBehaviour<ChaseBehaviour>().Target = other.gameObject;
            }
                gameObject.GetComponentInParent<Animator>().SetTrigger("TargetAcquired");
            b_PlayerIsOutOfCollider = false;
            CancelInvoke("LoseTarget");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            b_PlayerIsOutOfCollider = true;
            InvokeRepeating("LoseTarget", 0.2f, 0.2f);
        }
    }
    void LoseTarget()
    {
        timer += 0.2f;
        if (timer >= TargetLoseTime)
        {
            gameObject.GetComponentInParent<Animator>().GetBehaviour<ChaseBehaviour>().Target = null;
            gameObject.GetComponentInParent<Animator>().SetTrigger("TargetIsLost");
            timer = 0;
        }
    }

}
