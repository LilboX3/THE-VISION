using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject playerObject;

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

    private void CombatLoop()
    {
        while (true)
        {
            playerController.PlayerTurn();
            if (PlayerWon())
            {
                State = GameState.Victory;
                break;
            }
            //enemyController.EnemyTurn();
            if (EnemyWon())
            {
                State = GameState.Loss;
                break;
            }
        }
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (State)
        {
            case GameState.Movement:
                playerController.EnableMovement();
                Debug.Log("moving is allowed");
                break;
            case GameState.CombatStart:
                playerController.DisableMovement();
                combatManager.OpenCombatScreen();
                CombatLoop(); //coroutine sollt erst weitergehn und nach state checken, wenn combat vorbei ist
                break;
            case GameState.Victory:
                combatManager.CloseCombatScreen();
                break;
            case GameState.Loss:
                combatManager.CloseCombatScreen();
                break;

        }
        Debug.Log("Updated gamestate to: " + newState);
    }
    
    private bool PlayerWon()
    {
        return playerController.IsPlayerDead();
    }

    private bool EnemyWon()
    {
        //return enemyController.IsEnemyDead();
        return true;
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
