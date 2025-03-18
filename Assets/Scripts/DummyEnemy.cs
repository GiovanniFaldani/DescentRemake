using UnityEngine;

public class DummyEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 5;

    void IDamageable.TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Registered " + damage + " damage!");
    }

    void Update()
    {
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
