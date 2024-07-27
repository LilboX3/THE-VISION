using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event Action<GameState> OnGameStateChanged;

    private CombatManager combatManager;

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

    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Movement:
                break;
            case GameState.CombatStart:
                Debug.Log("STARTING COMBAT!!!");
                combatManager.OpenCombatScreen();
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Victory:
                combatManager.CloseCombatScreen();
                break;
            case GameState.Loss:
                combatManager.CloseCombatScreen();
                break;
            default:
                throw new NotImplementedException();
                //press Alt+enter to generate automatically
        }
        OnGameStateChanged?.Invoke(newState); //Only invoke if someones subscribe
        Debug.Log("Updated gamestate to: " + newState);
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
