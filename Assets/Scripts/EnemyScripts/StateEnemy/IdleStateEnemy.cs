using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateEnemy : Istate
{

    private readonly EnemyBase enemyBase;
    public IdleStateEnemy(EnemyBase spaceShip)
    {
        this.enemyBase = spaceShip;
    }
    public void EnterState()
    {
        enemyBase.EnterIdleState();
    }

    public void ExitState()
    {
        enemyBase.ExitIdleState();
    }

    public void UpdateState()
    {
        enemyBase.UpdateIdleState();
    }

  
}
