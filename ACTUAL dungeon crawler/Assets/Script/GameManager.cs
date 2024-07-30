using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private CombatManager combatManager;
    [SerializeField]
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
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case GameState.Movement:
                break;
            case GameState.CombatStart:
                Debug.Log("STARTING COMBAT!!!");
                playerController.DisableMovement();
                combatManager.OpenCombatScreen();
                State = GameState.PlayerTurn;
                break;
            case GameState.PlayerTurn:
                playerController.PlayerTurn();
                CheckWinner();
                break;
            case GameState.EnemyTurn:
                //enemyController.EnemyTurn();
                CheckWinner();
                break;
            case GameState.Victory:
                playerController.EnableMovement();
                combatManager.CloseCombatScreen();
                break;
            case GameState.Loss:
                combatManager.CloseCombatScreen();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        Debug.Log("Updated gamestate to: " + newState);
    }

    public void CheckWinner()
    {

    }
}

public enum GameState
{
    //in combat etc.
    Movement,
    CombatStart,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Loss
}
