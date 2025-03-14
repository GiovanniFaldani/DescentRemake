using UnityEngine;

// interface for all damageable objects to implement a TakeDamage method
public interface IDamageable
{ 
    public void TakeDamage(int damage);
}
