using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] KeyTypes keyType;

    // Assign from inspector the wall this key unlocks
    [SerializeField] GameObject AssociatedWall;

    [SerializeField] private GameObject[] keyMeshes;

    private void Start()
    {
        //keyMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach(GameObject keyMesh in keyMeshes)
        {
            keyMesh.SetActive(false);
        }

        switch (keyType)
        {
            case KeyTypes.Yellow:
                keyMeshes[0].SetActive(true);
                break;
            case KeyTypes.Blue:
                keyMeshes[1].SetActive(true);
                break;
            case KeyTypes.Exit:
                keyMeshes[2].SetActive(true);
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
