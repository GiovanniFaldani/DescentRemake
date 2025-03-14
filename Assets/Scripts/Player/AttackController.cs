using NUnit.Framework.Constraints;
using UnityEngine;
    
public class AttackController : MonoBehaviour
{
    [SerializeField] private GameObject fireBreathPrefab;
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private GameObject arcaneMissilePrefab;
    [SerializeField] private GameObject minePrefab;

    [Tooltip("The entity using the attacks")]
    [SerializeField] private DamageSources source;

    public void BreatheFire(Vector3 direction, Transform spawnPoint, Quaternion rotation)
    {
        // Spawn in front of player and attach to socket
        GameObject fireBreath = Instantiate(fireBreathPrefab, spawnPoint.position, rotation * Quaternion.Euler(0,90,0));
        fireBreath.GetComponent<FireBreath>().SetSource(source);
        fireBreath.transform.parent = this.gameObject.transform;
    }

    public void ShootFireBall(Vector3 direction, Transform spawnPoint)
    {
        // Inherit firing direction and source from parent component, detach from parent transform before moving
        GameObject fireBall = Instantiate(fireBallPrefab, spawnPoint);
        fireBall.GetComponent<FireBall>().SetDirection(direction).SetSource(source);
        fireBall.transform.parent = null;
    }

    public void ShootArcaneMissile(Transform spawnPoint)
    {

    }

    public void PlaceMine(Transform spawnPoint)
    {

    }
}
