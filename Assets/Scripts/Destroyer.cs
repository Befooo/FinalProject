using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private Transform _targetToDestroy = null;

    public void DestroyTarget()
    {
        Destroy(_targetToDestroy.gameObject);
    }
}