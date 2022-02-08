using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemCommand : Command
{
    Unit unit;
    Pickup pickup;

    public PickupItemCommand(ref Unit _unit, Pickup _pickup)
    {
        unit = _unit;
        pickup = _pickup;
    }

    public override bool Execute()
    {
        if(unit.unitInventory.AddPickup(pickup))
        {
            pickup.TurnOff();
            return true;
        }
        return false;
    }

    public override bool Undo()
    {
        if(unit.unitInventory.RemovePickup(pickup))
        {
            pickup.TurnOn();
            return true;
        }
        return false;
    }

}
