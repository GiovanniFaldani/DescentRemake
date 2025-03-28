using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    // score this egg is worth
    [SerializeField] private int score;
    // toggle for egg model
    [SerializeField] private EggTypes eggType;

    private MeshRenderer[] eggMeshes;

    void Start()
    {
        eggMeshes = GetComponentsInChildren<MeshRenderer>();

        //foreach(MeshRenderer eggMesh in eggMeshes)
        //{
        //    eggMesh.enabled = false;
        //}

        switch (eggType)
        {
            case EggTypes.Egg1:
                eggMeshes[0].enabled = true;
                break;
            case EggTypes.Egg2:
                eggMeshes[1].enabled = true;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameManager.Instance.playerUI.GetComponent<PlayerUI>().PrintToGameLog(
                "Saved one of your clan's eggs!", 5f);
            GameManager.Instance.eggsCollected += 1;
            GameManager.Instance.AddScore(score);
            Destroy(this.gameObject);
        }
    }

}
