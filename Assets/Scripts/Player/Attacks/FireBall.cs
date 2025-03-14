using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    private Vector3 direction;

    [SerializeField] GameObject explosionPrefab;

    void Start()
    {
        
    }

    // Move in a straight line
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

    public void SetDirection(Vector3 newDirection)
    {
        this.direction = newDirection;
    }

    // explode in a radius on wall or enemy hit
    private void OnTriggerEnter(Collider collider)
    {
        // TODO add tag check for enemy or walls to explode
    }


    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, this.transform);
        explosion.GetComponent<Explosion>().SetDamage(damage);
        explosion.transform.parent = null;
        Destroy(this.gameObject);
    }
}
