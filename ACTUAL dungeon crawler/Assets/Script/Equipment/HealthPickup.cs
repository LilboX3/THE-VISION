using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickupCallback
{
    public int restoreAmount;

    public void PickupCallback(PlayerController player)
    {
        player.RestoreHealth(restoreAmount);
    }
}