using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    [SerializeField] private GameObject _targetToDestroy = null;

    private void Update()
    {
        if (GetComponent<ParticleSystem>().IsAlive()) return;
        if (_targetToDestroy != null) Destroy(_targetToDestroy);
        else Destroy(gameObject);
    }
}