using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Health _target = null;
    private float _damage = 0;
    [SerializeField] private float _speed = 1;

    private void Update()
    {
        if (_target == null) return;

        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void SetTarget(Health target, float damage)
    {
        this._target = target;
        this._damage = damage;
    }

    private Vector3 GetAimLocation()
    {
        if (!_target.TryGetComponent(out CapsuleCollider capsuleCollider)) return _target.transform.position;

        return _target.transform.position + Vector3.up * capsuleCollider.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Health health)) return;
        if (health != _target) return;

        _target.TakeDamage(_damage);
        Destroy(this.gameObject);
    }
}