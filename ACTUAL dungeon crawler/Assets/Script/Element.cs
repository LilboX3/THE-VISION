using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public enum ElementType
    {
        Melee,
        Magic,
        Sin,
        Prayer
    }

    public static float getDamageModifierAttackedBy(ElementType thisElement, ElementType otherElement)
    {
        switch (thisElement)
        {
            case ElementType.Melee:
                if (otherElement == ElementType.Prayer)
                    return 2;
                if (otherElement == ElementType.Magic)
                    return 1 / 2;
                break;

            case ElementType.Magic:
                if (otherElement == ElementType.Melee)
                    return 2;
                if (otherElement == ElementType.Sin)
                    return 1 / 2;
                break;

            case ElementType.Sin:
                if (otherElement == ElementType.Magic)
                    return 2;
                if (otherElement == ElementType.Prayer)
                    return 1 / 2;
                break;

            case ElementType.Prayer:
                if (otherElement == ElementType.Sin)
                    return 2;
                if (otherElement == ElementType.Melee)
                    return 1 / 2;
                break;
        }

        return 1;
    }
}
