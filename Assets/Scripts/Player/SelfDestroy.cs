using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if(ps && !ps.IsAlive())
        {
            DestroySelfAnimEvent();
        }
    }
    public void DestroySelfAnimEvent()
    {
        Destroy(gameObject);
    }
}
