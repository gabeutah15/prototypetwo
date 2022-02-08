using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : UnitInput
{
    float xInput;
    float yInput;
    MovementDirections movementDirection;
    Command moveCommand = null;
    float timeBetweenMoves = 0.5f;
    float timeSinceLastMove = 0.0f;
    public event Action OnMoved = delegate { };
    public event Action OnUndoMove = delegate { };
    public event Action OnRedoMove = delegate { };



    public override ref Command HandleInput(ref bool choseMove)
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if(xInput < 0)
        {
            if(yInput > 0)
            {
                movementDirection = MovementDirections.UpLeft;
            }
            else if(yInput < 0)
            {
                movementDirection = MovementDirections.DownLeft;
            }
            else
            {
                movementDirection = MovementDirections.Left;
            }
        }
        else if(xInput > 0)
        {
            if(yInput > 0)
            {
                movementDirection = MovementDirections.UpRight;
            }
            else if(yInput < 0)
            {
                movementDirection = MovementDirections.DownRight;
            }
            else
            {
                movementDirection = MovementDirections.Right;
            }
        }
        else//this section not really necessary as moving straight up and down should not be permitted on hexagon grid
        {
            choseMove = false;

            //if(yInput > 0)
            //{
            //    movementDirection = MovementDirections.Up;
            //    choseMove = false;
            //}
            //else if(yInput < 0)
            //{
            //    movementDirection = MovementDirections.Down;
            //    choseMove = false;
            //}
            //else
            //{
            //    movementDirection = MovementDirections.None;
            //    choseMove = false;
            //}
        }
        moveCommand = new MoveUnitCommand(ref thisUnit, movementDirection);
        return ref moveCommand;
    }

    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        timeSinceLastMove += Time.deltaTime;
        bool choseMove = true;
        moveCommand = (MoveUnitCommand)HandleInput(ref choseMove);
        if((timeSinceLastMove > timeBetweenMoves) && choseMove)
        {
            if(moveCommand.Execute())
            {
                timeSinceLastMove = 0;
                OnMoved();
                thisUnit.commands.Push(moveCommand);
                thisUnit.redoCommands.Clear();
            }
        }

        if(Input.GetKeyDown(KeyCode.R))//redo
        {
            if(thisUnit.redoCommands.Count > 0)
            {
                Command redoCommand = thisUnit.redoCommands.Peek();

                switch(redoCommand)
                {
                    case MoveUnitCommand MUC:
                        if(redoCommand.Execute())
                        {
                            thisUnit.commands.Push(thisUnit.redoCommands.Pop());
                            OnRedoMove();
                        }
                        break;
                    case PickupItemCommand PIC:
                        if(redoCommand.Execute())
                        {
                            //pickups happen with a move so you need to undo twice if picked something up
                            //would probably be better if pickup command was part of move or vice versa?
                            //or if pickup command had a move command contained in it or something?
                            //or actaully it's undo could itself popoff a command from the unit
                            thisUnit.commands.Push(thisUnit.redoCommands.Pop());

                            if(thisUnit.redoCommands.Count > 0)
                            {
                                Command redoCommandPrevious = thisUnit.redoCommands.Peek();

                                if(redoCommandPrevious.Execute())
                                {
                                    thisUnit.commands.Push(thisUnit.redoCommands.Pop());
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }


                //thisUnit.commands.Pop().Undo();//undo doens't reall need a bool?
            }
        }

        if(Input.GetKeyDown(KeyCode.E))//undo
        {
            if(thisUnit.commands.Count > 0)
            {
                Command undoCommand = thisUnit.commands.Peek();//this will undo whatever the last command was
                //if pickups are automatically picked up on move though
                //then should be part of a move command?

                switch(undoCommand)
                {
                    case MoveUnitCommand MUC:
                        if(undoCommand.Undo())
                        {
                            thisUnit.redoCommands.Push(thisUnit.commands.Pop());
                            OnUndoMove();
                        }
                        break;
                    case PickupItemCommand PIC:
                        if(undoCommand.Undo())
                        {
                            //pickups happen with a move so you need to undo twice if picked something up
                            //would probably be better if pickup command was part of move or vice versa?
                            //or if pickup command had a move command contained in it or something?
                            //or actaully it's undo could itself popoff a command from the unit
                            thisUnit.redoCommands.Push(thisUnit.commands.Pop());

                            if(thisUnit.commands.Count > 0)
                            {
                                Command undoCommandPrevious = thisUnit.commands.Peek();

                                if(undoCommandPrevious.Undo())
                                {
                                    thisUnit.redoCommands.Push(thisUnit.commands.Pop());
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }


                //thisUnit.commands.Pop().Undo();//undo doens't reall need a bool?
            }
        }
    }
}
