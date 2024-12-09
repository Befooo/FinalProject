using GameDevTV.Inventories;
using RPG.Control;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class ClickablePickup : MonoBehaviour, IRayCastable
{
    Pickup pickUp;

    private void Awake()
    {
        pickUp = GetComponent<Pickup>();
    }

    public ECursorType eCursorType => pickUp.CanBePickedUp() ? ECursorType.PICK_UP : ECursorType.FULL_PICK_UP;

    public bool HandleRayCast(PlayerController playerController)
    {
        if (Input.GetMouseButtonDown(0))
        {
            pickUp.PickupItem();
        }

        return true;
    }
}