using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerStateEnemy: Istate
{
    private readonly EnemyBase m_EnemyBase;
    public SpawnerStateEnemy(EnemyBase enemyBase)
    {
        m_EnemyBase = enemyBase;
    }
    public void EnterState()
    {
        m_EnemyBase.EnterSpawnerState();
    }

    public void ExitState()
    {
        m_EnemyBase?.ExitSpawnerState();

    }

    public void UpdateState()
    {
        m_EnemyBase.UpdateSpawnerState();

    }


}
