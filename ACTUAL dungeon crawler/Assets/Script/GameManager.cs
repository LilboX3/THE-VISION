using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject playerObject;
    public GameObject enemyObject;

    private CombatManager combatManager;
    private PlayerController playerController;

    public GameState State;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        combatManager = GetComponent<CombatManager>();
        playerController = playerObject.GetComponent<PlayerController>();
        UpdateGameState(GameState.Movement);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        Debug.Log("Updated gamestate to: " + newState);
        switch (State)
        {
            case GameState.Movement:
                playerController.EnableMovement();
                break;
            case GameState.CombatStart:
                playerController.DisableMovement();
                combatManager.SetEnemy(enemyObject);
                combatManager.OpenCombatScreen();
                combatManager.StartCombat();
                break;
            case GameState.Victory:
                combatManager.CloseCombatScreen();
                break;
            case GameState.Loss:
                combatManager.CloseCombatScreen();
                break;

        }
    }
  
}

public enum GameState
{
    //in combat etc.
    Movement,
    CombatStart,
    Victory,
    Loss
}
