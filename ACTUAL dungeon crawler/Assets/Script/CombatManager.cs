using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject _combatScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCombatScreen()
    {
        _combatScreen.SetActive(true);
    }

    public void CloseCombatScreen()
    {
        _combatScreen.SetActive(false);
    }
}
