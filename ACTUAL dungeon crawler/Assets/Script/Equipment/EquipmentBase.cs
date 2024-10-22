using UnityEngine;

public abstract class EquipmentBase : MonoBehaviour, IPickupCallback
{
    public float SelfDestructTime = 4f;

    public abstract void DoBattleEquipmentEffect(PlayerController player, EnemyController enemy);

    public void Break(PlayerController player)
    {
        player.RemoveEquipment(this);

        StartCoroutine(this.SelfDestructAfter(SelfDestructTime));
    }

    public virtual void PickupCallback(PlayerController player)
    {
        player.PickUpEquipment(this);
    }
}