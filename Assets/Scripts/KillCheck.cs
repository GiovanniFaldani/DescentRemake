using UnityEngine;

public class KillCheck : MonoBehaviour
{
    public void OnDestroy()
    {
        GameManager.Instance.KillBoss();
    }
}
