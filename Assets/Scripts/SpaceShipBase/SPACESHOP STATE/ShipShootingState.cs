using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShootingState : Istate
{
    private readonly SpaceShipBase SpaceShip;
    public ShipShootingState(SpaceShipBase spaceShip)
    {
        this.SpaceShip = spaceShip;
    }
    public void EnterState()
    {
        SpaceShip.stateShip = StateSpaceShip.shoting;
        SpaceShip.OnEnterShooting();
    }

    public void ExitState()
    {
        SpaceShip.OnExitShooting();
    }

    public void UpdateState()
    {
        SpaceShip.OnUpgradeShooting();
    }
}
