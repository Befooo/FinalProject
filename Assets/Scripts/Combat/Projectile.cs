using RPG.Core;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Health _targetHealth;
    private float _damage = 0;
    [SerializeField] private float _speed = 1;
    [SerializeField] private GameObject _hitEffect = null;
    [SerializeField] private bool _isHoming;

    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    private void Update()
    {
        if (_targetHealth == null) return;

        if (_isHoming) transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private Vector3 GetAimLocation()
    {
        if (!_targetHealth.TryGetComponent(out Collider capsuleCollider)) return _targetHealth.transform.position;

        return _targetHealth.transform.position + Vector3.up * capsuleCollider.bounds.size.y / 2;
    }

    public void SetTargetHealth(Health targetHealth, float damage)
    {
        this._targetHealth = targetHealth;
        this._damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != _targetHealth) return;

        if (_targetHealth.IsDead) return;
        _targetHealth.TakeDamage(_damage);

        if (_hitEffect != null) Instantiate(_hitEffect, GetAimLocation(), Quaternion.identity);

        Destroy(gameObject);
    }
}