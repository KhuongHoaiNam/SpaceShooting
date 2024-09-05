using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Data Cac phi thuyen
[CreateAssetMenu(fileName = "DataShipGlobal", menuName = "GolobaDataScpaceShip")]
public class DataShipGlobal : ScriptableObject
{
    
}

//Dai dien cho moi phi thuyen
[Serializable]
public class SpaceShipGlobal
{
    public List<DataSpaceShip> datalevelInGame;
}