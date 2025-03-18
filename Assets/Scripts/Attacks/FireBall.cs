using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] public int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    private DamageSources source;
    private Vector3 direction;

    [SerializeField] GameObject explosionPrefab;

    // Move in a straight line for a period of time
    void Update()
    {
        Move();
        Lifetime();
    }

    private void Move()
    {
        this.transform.position += direction.normalized * speed * Time.deltaTime;
    }

    private void Lifetime()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Explode();
        }
    }

    public FireBall SetDirection(Vector3 newDirection)
    {
        this.direction = newDirection;
        return this;
    }

    public FireBall SetSource(DamageSources newSource)
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
        else if (collider.CompareTag("Wall"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        // Inherit damage and firing source from parent component, detach from fireball transform before despawning
        GameObject explosion = Instantiate(explosionPrefab, this.transform);
        explosion.GetComponent<Explosion>().SetDamage(damage).SetSource(source);
        explosion.transform.parent = null;
        Destroy(this.gameObject);
    }
}
