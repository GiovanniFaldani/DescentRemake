using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DummyEnemy : MonoBehaviour, IDamageable
{
    // Score awarded on kill
    [SerializeField] private int score = 300;
    // Enemy HP
    [SerializeField] private int health = 5;
    // mesh reference for flicker
    private MeshRenderer mesh;

    private void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        if(health <= 0)
        {
            Destroy(this.gameObject);
            GameManager.Instance.AddScore(score);
        }
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
            mesh.enabled = !mesh.enabled;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
