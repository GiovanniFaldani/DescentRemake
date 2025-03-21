using UnityEngine;

public class Egg : MonoBehaviour
{
    // score this egg is worth
    [SerializeField] private int score;
    // toggle for egg model
    [SerializeField] private EggTypes eggType;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(score);
            Destroy(this.gameObject);
        }
    }

}
