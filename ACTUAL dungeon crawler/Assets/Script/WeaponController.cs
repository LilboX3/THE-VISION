using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int damageModifier;
    public Weapon weaponType;
    public Weapon secondaryType;
    public string weaponName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public enum Weapon
    {
        Prayer,
        Melee,
        Magic,
        Sin,
        None
    }
}
