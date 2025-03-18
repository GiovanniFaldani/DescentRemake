using UnityEngine;

public class FireBreath : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float duration;
    private DamageSources source;

    void Update()
    {
        Duration();
    }

    private void Duration()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public FireBreath SetSource(DamageSources newSource)
    {
        this.source = newSource;
        return this;
    }

    private void OnTriggerEnter(Collider collider)
    {
        // deal damage here with IDamageable.TakeDamage
        if (source == DamageSources.Player && (collider.CompareTag("Enemy") || collider.CompareTag("Wall")) && collider.gameObject.GetComponent<IDamageable>() != null)
        {
            collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        }
        else if (source == DamageSources.Enemy && (collider.CompareTag("Player") || collider.CompareTag("Wall")) && collider.gameObject.GetComponent<IDamageable>() != null)
        {
            collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }

}
