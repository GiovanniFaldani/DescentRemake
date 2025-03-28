using UnityEngine;

public class SuperManager : MonoBehaviour
{
    [SerializeField] private int[] topScores = {0,0,0,0};  // keep track of top 4 scores

    public static SuperManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        LoadHighScores();
    }

    public void UpdateHighScores(int newScore) 
    { 
        for(int i = 0; i < topScores.Length; i++)
        {
            if (newScore > topScores[i])
            {
                // save new top score and shift all others down
                int temp = topScores[i];
                topScores[i] = newScore;
                for(int j = i+1; j < topScores.Length; j++)
                {
                    topScores[j] = temp;
                    if (j < topScores.Length-1)
                        temp = topScores[j + 1];
                }
                break;
            }
        }
    }

    public void SaveHighScores()
    {
        for (int i = 0; i < topScores.Length; i++) {
            PlayerPrefs.SetInt("TopScore" + i, topScores[i]);
            PlayerPrefs.Save();
        }
    }

    public void LoadHighScores()
    {
        for (int i = 0; i < topScores.Length; i++)
        {
            topScores[i] = PlayerPrefs.GetInt("TopScore" + i, 0);
        }
    }
}
