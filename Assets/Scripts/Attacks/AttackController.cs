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
        // Spawn and attach to socket
        GameObject fireBreath = Instantiate(fireBreathPrefab, spawnPoint.position, rotation * Quaternion.Euler(0,90,0)); // fixes prefab rotation -_-
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

    public void ShootArcaneMissile(Vector3 defaultDirection, Transform spawnPoint)
    {
        // fetch closest target in view and set source, detach from parent transform
        GameObject arcaneMissile = Instantiate(arcaneMissilePrefab, spawnPoint);
        if (source == DamageSources.Player)
            arcaneMissile.GetComponent<ArcaneMissile>().SetDefaultDirection(defaultDirection).SetTarget(FindTarget()).SetSource(source);
        else
            arcaneMissile.GetComponent<ArcaneMissile>().SetDefaultDirection(defaultDirection).SetTarget(GetPlayer()).SetSource(source);
        arcaneMissile.transform.parent = null;
    }

    public void PlaceMine(Transform spawnPoint)
    {
        // Spawn and detach transform to be safe
        GameObject mine = Instantiate(minePrefab, spawnPoint);
        mine.GetComponent<Mine>().SetSource(source);
        mine.transform.parent = null;
    }

    // lists "Enemy" in current view and returns the closest to the player
    public GameObject FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject target = null;
        float distFromPlayer = float.MaxValue;
        foreach(GameObject enemy in enemies)
        {
            float tempDist = Vector3.Distance(this.gameObject.transform.position, enemy.transform.position);
            if (IsVisible(enemy.GetComponentInChildren<Renderer>()) && tempDist < distFromPlayer)
            {
                target = enemy;
                distFromPlayer = tempDist;
            }
        }
        return target;
    }

    // returns player gameobject
    public GameObject GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    // returns if an object is visibile in the main camera
    private bool IsVisible(Renderer renderer)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            return true;
        else
            return false;
    }
}
