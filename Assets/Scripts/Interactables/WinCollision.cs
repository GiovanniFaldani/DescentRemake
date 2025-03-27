using UnityEngine;

public class WinCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (GameManager.Instance.IsBossDead())
            {
                GameManager.Instance.WinGame();
            }
            else
            {
                GameManager.Instance.playerUI.GetComponent<PlayerUI>().PrintToGameLog(
                    "You must defeat the boss to leave!", 5f);
            }
        }
    }
}
