using UnityEngine;

// This singleton class is used to keep track of game state and player score
public class GameManager : MonoBehaviour
{
    // Total score obtained by the player 
    [SerializeField] private int score;
    [SerializeField] GameObject playerRef;

    // Singleton behavior
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int scoreToAdd)
    {
        this.score += scoreToAdd;
    }

    public void AddPlayerShield(int addedShield)
    {
        playerRef.GetComponent<Player>().AddShield(addedShield);
    }

    public void AddPlayerEnergy(int addedEnergy)
    {
        playerRef.GetComponent<Player>().AddEnergy(addedEnergy);
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
    }
}
