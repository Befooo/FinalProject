using RPG.Combat;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponSO _weaponSO;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        other.GetComponent<Fighter>().EquipWeapon(_weaponSO);
        Destroy(gameObject);
    }

}