using UnityEngine;

public class Equipment_MagicMissile : EquipmentBase
{
    public float damage = 10f;

    public int uses = 5;


    public override void DoBattleEquipmentEffect(PlayerController player, EnemyController enemy)
    {
        var thisElement = ElementType.Magic;

        var modifier = Element.getDamageModifierAttackedBy(enemy.element, thisElement);

        enemy.TakeDamage(damage * modifier);

        if (uses-- < 1)
            Break(player);
    }

}