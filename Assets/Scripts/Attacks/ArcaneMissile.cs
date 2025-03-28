using UnityEngine;

public class ArcaneMissile : MonoBehaviour
{
    [SerializeField] public int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    private DamageSources source;
    private Vector3 defaultDirection;
    private GameObject target = null;



    void Update()
    {
        Move();
        Lifetime();
    }

    private void Move()
    {
        if (target == null)
        {
            // Move in a straight line for a period of time
            this.transform.position += defaultDirection.normalized * speed * Time.deltaTime;
        }
        else
        {
            // Keep following target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }

    private void Lifetime()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public ArcaneMissile SetDefaultDirection(Vector3 newDirection)
    {
        this.defaultDirection = newDirection;
        return this;
    }

    public ArcaneMissile SetSource(DamageSources newSource)
    {
        this.source = newSource;
        return this;
    }

    public ArcaneMissile SetTarget(GameObject newTarget)
    {
        this.target = newTarget;
        return this;
    }

    private void OnTriggerEnter(Collider collider)
    {
        // deal damage here with IDamageable.TakeDamage
        if (source == DamageSources.Player && (collider.CompareTag("Enemy") || collider.CompareTag("Wall")) && collider.gameObject.GetComponent<IDamageable>() != null)
        {
            collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (source == DamageSources.Enemy && (collider.CompareTag("Player") || collider.CompareTag("Wall")) && collider.gameObject.GetComponent<IDamageable>() != null)
        {
            collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
