using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject _combatScreen;
    public GameObject playerObject;
    public GameObject enemyObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CombatLoop()
    {
        while (true)
        {
            //playerController.PlayerTurn();
            if (PlayerWon())
            {
                GameManager.Instance.UpdateGameState(GameState.Victory);
                break;
            }
            //enemyController.EnemyTurn();
            if (EnemyWon())
            {
                GameManager.Instance.UpdateGameState(GameState.Loss);
                break;
            }
        }
    }
    private bool PlayerWon()
    {
        //return playerController.IsPlayerDead();
        return true;
    }

    private bool EnemyWon()
    {
        //return enemyController.IsEnemyDead();
        return true;
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
