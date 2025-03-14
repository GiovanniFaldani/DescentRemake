using UnityEngine;
    
public class AttackController : MonoBehaviour
{
    [SerializeField] private GameObject fireBreathPrefab;
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private GameObject arcaneMissilePrefab;
    [SerializeField] private GameObject minePrefab;

    public void BreatheFire(Vector3 direction, Transform spawnPoint)
    {

    }

    public void ShootFireBall(Vector3 direction, Transform spawnPoint)
    {
        Instantiate(fireBallPrefab, spawnPoint).GetComponent<FireBall>().SetDirection(direction);
    }

    public void ShootArcaneMissile(Transform spawnPoint)
    {

    }

    public void PlaceMine(Transform spawnPoint)
    {

    }
}
