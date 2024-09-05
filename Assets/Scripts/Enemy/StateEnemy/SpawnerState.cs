using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerState : Istate
{

    private readonly EnemyBase enemyBase;
    public SpawnerState(EnemyBase spaceShip)
    {
        this.enemyBase = spaceShip;
    }
    public void EnterState()
    {
        enemyBase.EnterSpawnState();
    }

    public void ExitState()
    {
        enemyBase.ExitSpawnState();
    }

    public void UpdateState()
    {
        enemyBase.UpdateSpawnState();
    }

  
}
