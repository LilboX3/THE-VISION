using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public ElementType element;
    public int elementStat;
    public float health;
    public string name;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnemyTurn()
    {

    }

    public bool IsEnemyDead()
    {
        return health <= 0;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
