using System.Collections;
using RPG.Combat;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponSO _weaponSO;
    [SerializeField] private float _reSpawnTime = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        other.GetComponent<Fighter>().EquipWeapon(_weaponSO);
        StartCoroutine(IE_HideForSeconds(_reSpawnTime));
    }

    private IEnumerator IE_HideForSeconds(float seconds)
    {
        ShowPickup(false);
        yield return new WaitForSeconds(seconds);
        ShowPickup(true);
    }

    private void ShowPickup(bool canShow)
    {
        GetComponent<Collider>().enabled = canShow;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(canShow);
        }
    }
}