using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameLog;
    [SerializeField] private Sprite[] HpBars;
    [SerializeField] private Image HpBarStatus;

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
        if (logTimer > 0f)
        {
            logTimer -= Time.deltaTime;
            if (logTimer < 0f)
            {
                ClearGameLog();
            }
        }
        UpdateHpBar();
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

    void UpdateHpBar()
    {
        switch (playerScriptRef.health)
        {
            case 0:
                HpBarStatus.sprite = HpBars[0];
                break;
            case 1:
                HpBarStatus.sprite = HpBars[1];
                break;
            case 2:
                HpBarStatus.sprite = HpBars[2];
                break;
            case 3:
                HpBarStatus.sprite = HpBars[3];
                break;
            case 4:
                HpBarStatus.sprite = HpBars[4];
                break;
            case 5:
                HpBarStatus.sprite = HpBars[5];
                break;
            case 6:
                HpBarStatus.sprite = HpBars[6];
                break;
        }
    }
}