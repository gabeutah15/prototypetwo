using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class MoveUnitCommand : Command
{
    Unit unit;
    MovementDirections movementDirection;
    MovementDirections reverseDirection;

    public MoveUnitCommand(ref Unit _unit, MovementDirections _movementDirection)
    {
        unit = _unit;
        reverseDirection = GetOppositeDirection(_movementDirection);
        movementDirection = _movementDirection;

    }

    MovementDirections GetOppositeDirection(MovementDirections direction)
    {
        switch(direction)
        {
            case MovementDirections.UpRight:
                return MovementDirections.DownLeft;
            case MovementDirections.Right:
                return MovementDirections.Left;
            case MovementDirections.DownRight:
                return MovementDirections.UpLeft;
            case MovementDirections.DownLeft:
                return MovementDirections.UpRight;
            case MovementDirections.Left:
                return MovementDirections.Right;
            case MovementDirections.UpLeft:
                return MovementDirections.DownRight;
            default:
                return MovementDirections.None;
        }
    }

    public override bool Execute()
    {
        if(unit.unitMovement.CanMove(movementDirection, unit, false))
        {
            unit.unitMovement.Move(movementDirection, true);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool Undo()
    {
        if(unit.unitMovement.CanMove(reverseDirection, unit, false))
        {
            unit.unitMovement.Move(reverseDirection, false);
            return true;
        }
        else
        {
            return false;
        }
        //should there be undos you can't perform? like if the previous tile has been turned into a collision tile?
    }
}
