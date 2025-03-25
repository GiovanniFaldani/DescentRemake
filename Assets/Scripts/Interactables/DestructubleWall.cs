using UnityEngine;

public class DestructibleWall : MonoBehaviour, IDamageable
{
    [SerializeField] private int score;
    [SerializeField] private int health;
    private Material material;
    public void TakeDamage(int damage)
    {
        health -= damage;
        material.color = material.color - new Color(0.20f, 0.20f, 0.20f) * damage; // scale color down based on damage taken
    }

    void Start()
    {
        material = GetComponentInChildren<Renderer>().material;
    }

    void Update()
    {
        if (health <= 0)
        {
            GameManager.Instance.AddScore(score);
            GameManager.Instance.playerUI.GetComponent<PlayerUI>().PrintToGameLog(
                "A secret passage reveals itself!", 5f);
            Destroy(this.gameObject);
        }
    }
}
