using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnergyRefil : MonoBehaviour
{
    [SerializeField] int addedEnergy;

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
            GameManager.Instance.AddPlayerEnergy(addedEnergy);
            Destroy(this.gameObject);
        }
    }
}
