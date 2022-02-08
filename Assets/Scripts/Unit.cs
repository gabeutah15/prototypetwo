using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    [HideInInspector]
    public UnitInput unitInput;
    [HideInInspector]
    public UnitMovement unitMovement;
    [HideInInspector]
    public MapPainter mapPainter;
    [HideInInspector]
    public UnitInventory unitInventory;

    public Stack<Command> commands;
    public Stack<Command> redoCommands;

    [HideInInspector]
    public Data data;
    [HideInInspector]
    public Tile yourPaintTileType;
    [HideInInspector]
    public Tile opponentPaintTileType;


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        unitInput = GetComponent<UnitInput>();
        unitMovement = GetComponent<UnitMovement>();
        mapPainter = GetComponent<MapPainter>();
        unitInventory = GetComponent<UnitInventory>();

        commands = new Stack<Command>();
        redoCommands = new Stack<Command>();


        data = FindObjectOfType<Data>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
