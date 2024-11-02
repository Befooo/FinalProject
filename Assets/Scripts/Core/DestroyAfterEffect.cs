using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    private void Update()
    {
        if (GetComponent<ParticleSystem>().IsAlive()) return;
        Destroy(gameObject);
    }
}