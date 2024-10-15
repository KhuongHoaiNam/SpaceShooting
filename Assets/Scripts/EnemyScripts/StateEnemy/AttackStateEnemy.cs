using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateEnemy : Istate
{

    private readonly EnemyBase enemyBase;
    public AttackStateEnemy(EnemyBase spaceShip)
    {
        this.enemyBase = spaceShip;
    }
    public void EnterState()
    {
        enemyBase.EnterAttackState();
    }

    public void ExitState()
    {
        enemyBase.ExitAttackStates();
    }

    public void UpdateState()
    {
        enemyBase.UpdateAttackState();
    }

  
}
