using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] KeyTypes keyType;

    // Assign from inspector the wall this key unlocks
    [SerializeField] GameObject AssociatedWall;

    private MeshRenderer[] keyMeshes;

    private void Start()
    {
        keyMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer keyMesh in keyMeshes)
        {
            keyMesh.enabled = false;
        }

        switch (keyType)
        {
            case KeyTypes.Yellow:
                keyMeshes[0].enabled = true;
                break;
            case KeyTypes.Blue:
                keyMeshes[1].enabled = true;
                break;
            case KeyTypes.Exit:
                keyMeshes[2].enabled = true;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            // increase score
            GameManager.Instance.AddScore(score);

            // log message of door opening to UI
            GameManager.Instance.playerUI.GetComponent<PlayerUI>().PrintToGameLog(
                "A " + keyType.ToString().ToLower() + " door opens somewhere...", 5f);

            // collect key, add score and open associated door
            AssociatedWall.SetActive(false);

            Destroy(this.gameObject);
        }
    }
}
