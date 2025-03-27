using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DummyEnemy : MonoBehaviour, IDamageable
{
    // Score awarded on kill
    [SerializeField] private int score = 300;
    // Enemy HP
    [SerializeField] private int health = 5;
    // mesh reference for flicker
    [SerializeField] private GameObject mesh;

    // Attack controller reference
    private AttackController ac;

    // attack timer
    [SerializeField] private float attackTimer = 1f;

    private void Start()
    {
        //mesh = GetComponentInChildren<MeshRenderer>();
        ac = GetComponentInChildren<AttackController>();
    }

    void Update()
    {
        if(health <= 0)
        {
            Destroy(this.gameObject);
            GameManager.Instance.AddScore(score);
        }
        //attackTimer -= Time.deltaTime;
        //if (attackTimer <= 0)
        //{
        //    Attack();
        //    attackTimer = 1f;
        //}
    }

    void IDamageable.TakeDamage(int damage)
    {
        if (health > 1)
        {
            StartCoroutine(FlickerOnHit());
        }
        health -= damage;
        Debug.Log("Registered " + damage + " damage!");
    }

    private IEnumerator FlickerOnHit()
    {
        for (int i = 0; i < 4; i++)
        {
            mesh.SetActive(!mesh.activeSelf);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Attack()
    {
        ac.BreatheFire(this.transform.forward, this.transform, this.transform.rotation);
    }
}
