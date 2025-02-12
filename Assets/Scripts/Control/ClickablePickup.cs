using GameDevTV.Inventories;
using RPG.Control;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class ClickablePickup : MonoBehaviour, IRayCastable
{
    Pickup pickup;

    private void Awake()
    {
        pickup = GetComponent<Pickup>();
    }


    public CursorType GetCursorType()
    {
        if (pickup.CanBePickedUp())
        {
            return CursorType.Pickup;
        }
        else
        {
            return CursorType.FullPickup;
        }
    }

    public bool HandleRayCast(PlayerController playerController)
    {
        if (Input.GetMouseButtonDown(0))
        {
            pickup.PickupItem();
        }

        return true;
    }
}