using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] public int damage;
    [SerializeField] private float lifetime;
    private DamageSources source;

    [SerializeField] GameObject explosionPrefab;

    // Move in a straight line for a period of time
    void Update()
    {
        Lifetime();
    }

    private void Lifetime()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Explode();
        }
    }

    public Mine SetSource(DamageSources newSource)
    {
        this.source = newSource;
        return this;
    }

    // explode in a radius on wall or enemy hit
    private void OnTriggerEnter(Collider collider)
    {
        // source and tag check for enemy, walls or player to explode
        if (source == DamageSources.Player && (collider.CompareTag("Wall") || collider.CompareTag("Enemy")))
        {
            Explode();
        }
        else if (source == DamageSources.Enemy && (collider.CompareTag("Wall") || collider.CompareTag("Player")))
        {
            Explode();
        }
    }

    public void Explode()
    {
        // Inherit damage and firing source from parent component, detach from mine transform before despawning
        GameObject explosion = Instantiate(explosionPrefab, this.transform);
        explosion.GetComponent<Explosion>().SetDamage(damage).SetSource(source);
        explosion.transform.parent = null;
        Destroy(this.gameObject);
    }
}
