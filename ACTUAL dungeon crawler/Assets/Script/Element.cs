using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public ElementType elementType { get; set; }
    public enum ElementType
    {
        Melee,
        Magic,
        Sin,
        Prayer
    }

    public double getDamageModifierAttackedBy(Element otherElement)
    {
        switch (this.elementType)
        {
            case ElementType.Melee:
                if (otherElement.elementType == ElementType.Prayer)
                    return 2;
                if (otherElement.elementType == ElementType.Magic)
                    return 1 / 2;
                break;

            case ElementType.Magic:
                if (otherElement.elementType == ElementType.Melee)
                    return 2;
                if (otherElement.elementType == ElementType.Sin)
                    return 1 / 2;
                break;

            case ElementType.Sin:
                if (otherElement.elementType == ElementType.Magic)
                    return 2;
                if (otherElement.elementType == ElementType.Prayer)
                    return 1 / 2;
                break;

            case ElementType.Prayer:
                if (otherElement.elementType == ElementType.Sin)
                    return 2;
                if (otherElement.elementType == ElementType.Melee)
                    return 1 / 2;
                break;
        }

        return 1;
    }
}
