using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject _combatScreen;
    public TextMeshProUGUI combatText;
    public TextMeshProUGUI enemyHealthText;
    public GameObject playerObject;
    public GameObject enemyObject;
    public GameObject continueButton;

    private PlayerController playerController;
    private EnemyController enemyController;

    private CombatState currentState;
    private bool inCombat = false;

    // Start is called before the first frame update
    void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inCombat)
        {
            if(currentState == CombatState.EnemyTurn)
            {
                /*float damageModifier = Element.getDamageModifierAttackedBy(playerController.element, enemyController.element);
                float damageAgainstPlayer = playerController.elementStat * damageModifier;
                playerController.TakeDamage(damageAgainstPlayer);*/
                playerController.TakeDamage(0.5f);
                currentState = CombatState.PlayerTurn;
            }
            else if (PlayerWon())
            {
                inCombat = false;
                Destroy(enemyObject);
                GameManager.Instance.UpdateGameState(GameState.Victory);
                continueButton.SetActive(true);
            }
            else if (EnemyWon())
            {
                inCombat = false;
                GameManager.Instance.UpdateGameState(GameState.Loss);
            }
        }
    }

    public void StartCombat()
    {
        inCombat = true;
        currentState = CombatState.PlayerTurn;
        Debug.Log("Combat manager is in combat too");
    }

    public void AttackEnemy()
    {
        if (currentState == CombatState.PlayerTurn)
        {
            float damageModifier = Element.getDamageModifierAttackedBy(enemyController.element, playerController.element);
            float damageAgainstEnemy = playerController.elementStat * damageModifier;
            enemyController.TakeDamage(damageAgainstEnemy);
            enemyHealthText.text = "Enemy: " + enemyController.health;
            currentState = CombatState.EnemyTurn;
        }
    }

    public void OpenCombatScreen()
    {
        combatText.text = "A " + enemyController.name + " appears!";
        enemyHealthText.text = "Enemy: " + enemyController.health;
        _combatScreen.SetActive(true);
    }

    public void CloseCombatScreen()
    {
        _combatScreen.SetActive(false);
    }

    public void ContinueMoving()
    {
        playerController.EnableMovement();
        continueButton.SetActive(false);
    }

    public void SetEnemy(GameObject enemy)
    {
        enemyObject = enemy;
        enemyController = enemyObject.GetComponent<EnemyController>();
    }

    private bool PlayerWon()
    {
        return enemyController.IsEnemyDead();
    }

    private bool EnemyWon()
    {
        return playerController.IsPlayerDead();
    }

    public enum CombatState
    {
        PlayerTurn,
        EnemyTurn,
        Victory,
        Loss
    }
}
