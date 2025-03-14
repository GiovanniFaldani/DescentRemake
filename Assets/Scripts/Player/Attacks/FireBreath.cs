using UnityEngine;

public class FireBreath : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float duration;
    private DamageSources source;

    // Update is called once per frame
    void Update()
    {
        Duration();
    }

    private void Duration()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public FireBreath SetSource(DamageSources newSource)
    {
        this.source = newSource;
        return this;
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO add source and tag check for enemy or walls to explode
    }

}
