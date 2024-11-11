using System.Collections;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IRayCastable
{
    [SerializeField] private WeaponSO _weaponSO;
    [SerializeField] private float _healthToRestore = 0;
    [SerializeField] private float _reSpawnTime = 5.0f;

    public ECursorType eCursorType => ECursorType.PICK_UP;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        PickUp(other.gameObject);
    }

    private void PickUp(GameObject gameObject)
    {
        if (_weaponSO != null)
        {
            gameObject.GetComponent<Fighter>().EquipWeapon(_weaponSO);
        }
        if (_healthToRestore > 0)
        {
            gameObject.GetComponent<Health>().Heal(_healthToRestore);
        }
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

    public bool HandleRayCast(PlayerController playerController)
    {
        if (Input.GetMouseButtonDown(0))
        {
            PickUp(playerController.gameObject);
        }

        return true;
    }
}