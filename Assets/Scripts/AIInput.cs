using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AIInput : UnitInput
{
    MovementDirections movementDirection;
    Command moveCommand = null;
    PlayerInput playerInput;
    bool isAlive = true;
    int numMovesOnOwnTerrainWithLowChanceEscape;
    int numPermittedMovesOnOwnTerrain = 15;//this can be like their lives?
    public event Action OnDied = delegate { };

    public override ref Command HandleInput(ref bool choseInput)
    {
        int numIterations = 0;
        while(!choseInput)//this will favor moving certain directions though
        {
            int rand = UnityEngine.Random.Range(0, 7);
            movementDirection = (MovementDirections)rand;//eventually do a canmove check on this and if cannot choose a new direction

            if(numIterations <= 100)
            {
                if(thisUnit.unitMovement.CanMove(movementDirection, thisUnit, true))//takes into account enemy tiles and paying and obstacles
                {
                    numMovesOnOwnTerrainWithLowChanceEscape--;//if could move on to clear terrain remove a lost from moving on own terrain
                    if(numMovesOnOwnTerrainWithLowChanceEscape < 0)
                        numMovesOnOwnTerrainWithLowChanceEscape = 0;
                    choseInput = true;
                }

                //add something to prefer moves, ie unpainted over paintd with their own color
                //actually it should just be a canmove variation for ai that returns false on their own color as well?
                //don't use normal canmove as that is needed for undos
                numIterations++;
            }
            else//probably not going to find it after so many tries, so start looking for next circle out
            {
                //STARTING TRYING moving on your own territory
                if(numIterations < 300)
                {
                    if(thisUnit.unitMovement.CanMove(movementDirection, thisUnit, false))//takes into account enemy tiles and paying and obstacles
                    {
                        for(int i = 1; i < 7; i++)
                        {
                            //foreach move on own territory, check if there is a clear space around it, and if so move in its direction
                            MovementDirections tempMovementDirection = (MovementDirections)i;
                            if(thisUnit.unitMovement.CanMove(tempMovementDirection, thisUnit, true))//takes into account enemy tiles and paying and obstacles
                            {
                                //numMovesOnOwnTerrain++;//move once on your own terrain and take this input, if cannot then just be stuck?
                                //should do this recursively
                                choseInput = true;
                                numMovesOnOwnTerrainWithLowChanceEscape--;//if could move on to clear terrain remove a lost from moving on own terrain
                                if(numMovesOnOwnTerrainWithLowChanceEscape < 0)
                                    numMovesOnOwnTerrainWithLowChanceEscape = 0;
                                break;
                            }
                        }
                    }
                    numIterations++;
                }
                else
                {//tried all tiles around itself for 100 tiles, then all around those around it for 100 more, so probably not gonna find
                    //a clear tile or a tile that moves toward a clear tile
                    if(thisUnit.unitMovement.CanMove(movementDirection, thisUnit, false))//takes into account enemy tiles and paying and obstacles
                    {
                        numMovesOnOwnTerrainWithLowChanceEscape++;
                        choseInput = true;
                    }
                }
            }

            if(numMovesOnOwnTerrainWithLowChanceEscape > numPermittedMovesOnOwnTerrain)
            {
                //enemy die
                //Debug.Log("Enemy Died");
                GetComponent<SpriteRenderer>().color = Color.red;
                isAlive = false;
                OnDied();
                break;
            }
        }


        moveCommand = new MoveUnitCommand(ref thisUnit, movementDirection);
        return ref moveCommand;
    }

    public bool ExecuteMove()
    {
        if(!isAlive)
            return false;

        bool choseMove = false;//not needed here, should have different override for handle input?
        moveCommand = (MoveUnitCommand)HandleInput(ref choseMove);//this is actually setting it to itself?

        if(choseMove)
        {
            if(moveCommand.Execute())
            {
                thisUnit.commands.Push(moveCommand);
                thisUnit.redoCommands.Clear();
                return true;
            }
        }

        return false;
    }

    public bool UndoMove()
    {
        if(!isAlive)
            return false;

        if(thisUnit.commands.Count > 0)
        {
            Command undoCommand = thisUnit.commands.Peek();

            if(undoCommand.Undo())
            {
                thisUnit.redoCommands.Push(thisUnit.commands.Pop());
                return true;
            }
        }
        return false;
    }

    public bool RedoMove()
    {
        if(!isAlive)
            return false;

        if(thisUnit.redoCommands.Count > 0)
        {
            Command redoCommand = thisUnit.redoCommands.Peek();

            if(redoCommand.Execute())
            {
                thisUnit.commands.Push(thisUnit.redoCommands.Pop());
                return true;
            }
        }
        return false;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        playerInput = FindObjectOfType<PlayerInput>();//not ideal
        playerInput.OnMoved += PlayerInput_OnMoved;
        playerInput.OnUndoMove += PlayerInput_OnUndoMove;
        playerInput.OnRedoMove += PlayerInput_OnRedoMove;
    }

    private void PlayerInput_OnRedoMove()
    {
        RedoMove();
    }

    private void PlayerInput_OnUndoMove()
    {
        UndoMove();
    }

    private void PlayerInput_OnMoved()
    {
        ExecuteMove();
    }

    private void OnDestroy()
    {
        playerInput.OnMoved -= PlayerInput_OnMoved;
        playerInput.OnUndoMove -= PlayerInput_OnUndoMove;
        playerInput.OnRedoMove -= PlayerInput_OnRedoMove;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
