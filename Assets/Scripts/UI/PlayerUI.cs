using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameLog;

    private Player playerScriptRef;
    private float logTimer = 0f;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScriptRef = playerObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (logTimer  > 0f)
        {
            logTimer -= Time.deltaTime;
            if (logTimer < 0f)
            {
                ClearGameLog();
            }
        }
        healthText.text = playerScriptRef.health.ToString();
        shieldText.text = playerScriptRef.shield.ToString();
        energyText.text = playerScriptRef.energyNumber.ToString();
        scoreText.text = GameManager.Instance.score.ToString();
    }

    public void PrintToGameLog(string message, float duration)
    {
        gameLog.text = message;
        logTimer = duration;
    }

    void ClearGameLog()
    {
        gameLog.text = "";
    }
}
