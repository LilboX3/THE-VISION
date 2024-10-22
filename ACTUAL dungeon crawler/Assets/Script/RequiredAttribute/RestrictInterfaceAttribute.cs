using System;
using UnityEngine;
/// <summary>
/// Attribute that require implementation of the provided interface.
/// </summary>
public class RestrictAttribute : PropertyAttribute
{
    // Interface type.
    public Type RestrictedType { get; private set; }
    /// <summary>
    /// Requiring implementation of the <see cref="T:RequireInterfaceAttribute"/> interface.
    /// </summary>
    /// <param name="type">Interface type.</param>
    public RestrictAttribute(Type type)
    {
        RestrictedType = type;
    }
}