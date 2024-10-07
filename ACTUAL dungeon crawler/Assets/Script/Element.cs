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
                    return 0.5f;
                break;

            case ElementType.Magic:
                if (otherElement == ElementType.Melee)
                    return 2;
                if (otherElement == ElementType.Sin)
                    return 0.5f;
                break;

            case ElementType.Sin:
                if (otherElement == ElementType.Magic)
                    return 2;
                if (otherElement == ElementType.Prayer)
                    return 0.5f;
                break;

            case ElementType.Prayer:
                if (otherElement == ElementType.Sin)
                    return 2;
                if (otherElement == ElementType.Melee)
                    return 0.5f;
                break;
        }

        return 1;
    }

    public static ElementType GetCounterOf(ElementType element)
    {
        switch (element)
        {
            case ElementType.Melee:
                return ElementType.Prayer;
            case ElementType.Magic:
                return ElementType.Melee;
            case ElementType.Sin:
                return ElementType.Magic;
            case ElementType.Prayer:
                return ElementType.Sin;
            default:
                throw new System.ArgumentException("just... HOW THE FUCK did you even get here");
        }
    }

    public static string ClueMessageBasedOnElement(ElementType element)
    {
        string message = "";
        switch (element)
        {
            case ElementType.Melee:
                message = "It looks like it's about to punch you ...";
                break;
            case ElementType.Magic:
                message = "It's pulsing with arcane aenergy ...";
                break;
            case ElementType.Sin:
                message = "You feel a dark aura around you ...";
                break;
            case ElementType.Prayer:
                message = "It's radiating a bright light ...";
                break;
            default:
                message = "WHAT. this shouldnt be possible (but im impressed)";
                break;
        }
        return message;
    }
}
