using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLevelUpState : Istate
{
    private readonly SpaceShipBase SpaceShip;
    public ShipLevelUpState(SpaceShipBase spaceShip)
    {
        this.SpaceShip = spaceShip;
    }
    public void EnterState()
    {
        SpaceShip.stateShip = StateSpaceShip.shoting;
        SpaceShip.OnEnterLevelUpState();
    }

    public void ExitState()
    {
        SpaceShip.OnExitLevelUpState();
    }

    public void UpdateState()
    {
        SpaceShip.OnUpdateLevelUpState();
    }
}
