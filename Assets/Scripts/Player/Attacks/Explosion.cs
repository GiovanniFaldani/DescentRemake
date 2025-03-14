using UnityEngine;

public class Explosion : MonoBehaviour
{
    private int damage;
    [SerializeField] private float lifetime = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Lifetime();
    }

    private void Lifetime()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // deal damage here with IDamageable.TakeDamage
    }

    public void SetDamage(int newDamage)
    {
        this.damage = newDamage;
    }
}
