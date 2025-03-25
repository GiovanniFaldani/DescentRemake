using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ShieldRefill : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] int addedShield;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameManager.Instance.playerUI.GetComponent<PlayerUI>().PrintToGameLog(
                "Regained " + addedShield.ToString() + " Shield!", 5f);
            GameManager.Instance.AddScore(score);
            GameManager.Instance.AddPlayerShield(addedShield);
            Destroy(this.gameObject);
        }
    }
}
