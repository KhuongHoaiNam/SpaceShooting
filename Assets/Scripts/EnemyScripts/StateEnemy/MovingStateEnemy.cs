using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStateEnemy : Istate
{
    private readonly EnemyBase m_enemyBase;
    public MovingStateEnemy(EnemyBase enemyBase)
    {
        m_enemyBase = enemyBase;
    }
    public void EnterState()
    {
        m_enemyBase.EnterMovingOnGame();
    }

    public void ExitState()
    {
        m_enemyBase.ExitMovingOnGame();
    }

    public void UpdateState()
    {
        m_enemyBase.UpdateMovingOnGame();
    }
}
