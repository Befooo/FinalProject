using RPG.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private Health _targetHealth;
    private float _damage = 0;
    [SerializeField] private float _speed = 1;
    [SerializeField] private GameObject _hitEffect = null;
    [SerializeField] private bool _isHoming;
    [SerializeField] private float _maxLifeTime = 10.0f;
    [SerializeField] private GameObject[] _destroyOnHit;
    [SerializeField] private float _lifeAfterImpact = 0.2f;
    [SerializeField] private UnityEvent _onHit;

    private GameObject _instigator;

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

    public void SetTargetHealth(Health targetHealth, GameObject instigator, float damage)
    {
        this._targetHealth = targetHealth;
        this._damage = damage;
        this._instigator = instigator;

        Destroy(gameObject, _maxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != _targetHealth) return;

        if (_targetHealth.IsDead) return;
        _targetHealth.TakeDamage(_instigator, _damage);

        _speed = 0;

        _onHit?.Invoke();
        if (_hitEffect != null) Instantiate(_hitEffect, GetAimLocation(), Quaternion.identity);

        foreach (GameObject toDestroy in _destroyOnHit)
        {
            Destroy(toDestroy);
        }

        Destroy(gameObject, _lifeAfterImpact);
    }
}