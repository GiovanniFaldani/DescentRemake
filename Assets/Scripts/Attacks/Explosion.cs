using UnityEngine;

public class Explosion : MonoBehaviour
{
    private int damage;
    [SerializeField] private float lifetime = 0.5f;
    private DamageSources source;

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

    private void OnTriggerEnter(Collider collider)
    {
        // deal damage here with IDamageable.TakeDamage
        if (source == DamageSources.Player && (collider.CompareTag("Enemy") || collider.CompareTag("Wall")) && collider.gameObject.GetComponent<IDamageable>() != null)
        {
            collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage); 
        }
        else if (source == DamageSources.Enemy && (collider.CompareTag("Enemy") || collider.CompareTag("Wall")) && collider.gameObject.GetComponent<IDamageable>() != null)
        {
            collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }

    public Explosion SetDamage(int newDamage)
    {
        this.damage = newDamage;
        return this;
    }

    public Explosion SetSource(DamageSources newSource)
    {
        this.source = newSource;
        return this;
    }
}
