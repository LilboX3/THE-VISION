using UnityEngine;

public class Equipment_MagicMissile : MonoBehaviour, IEquipment
{
    public float damage = 10f;

    public int uses = 5;

    public float SelfDestructTime = 4f;

    public void DoBattleEquipmentEffect(PlayerController player, EnemyController enemy)
    {
        var thisElement = ElementType.Magic;

        var modifier = Element.getDamageModifierAttackedBy(enemy.element, thisElement);

        enemy.TakeDamage(damage * modifier);

        if (uses-- < 1)
            Break(player);
    }

    public void Break(PlayerController player)
    {
        player.RemoveEquipment(this);

        StartCoroutine(this.SelfDestructAfter(SelfDestructTime));
    }
}