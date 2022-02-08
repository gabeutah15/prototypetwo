using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class UnitMovement : MonoBehaviour
{
    [SerializeField]
    private Tilemap walkableTileMap;
    [SerializeField]
    private Tilemap collisionTileMap;
    [SerializeField]
    private Tilemap triggerTileMap;
    private MapPainter mapPainter;
    private Tile yourTilePaintType;
    [SerializeField]
    private GameObject LineRendererGameObjectPrefab = null;
    Stack<GameObject> renderedLines;
    //so position is just on the transform, it's not like a tile identity, but should it be?

    public void Move(MovementDirections direction, bool addLine)
    {
        Vector3 startingPointLine = transform.position + new Vector3(0, 0, -1);
        transform.position += ConvertDirectionToMovementVector(direction);
        Vector3 endingPointLine = transform.position + new Vector3(0, 0, -1);
        if(addLine)
        {
            //should i add these to the move command itself and just turn them on and off? maybe do yet another command like pickups?
            GameObject LineRendererGameObject = Instantiate(LineRendererGameObjectPrefab);
            renderedLines.Push(LineRendererGameObject);
            LineRenderer lineRendererInstance = LineRendererGameObject.GetComponent<LineRenderer>();
            //lineRendererInstance.gameObject.SetActive(true);
            lineRendererInstance.positionCount = 2;
            lineRendererInstance.SetPosition(0, startingPointLine);
            lineRendererInstance.SetPosition(1, endingPointLine);
        }
        else
        {
            Destroy(renderedLines.Pop());
        }
        mapPainter.PaintCurrentTile();
    }


    public bool CanMove(MovementDirections direction, Unit unit, bool isAIMove)//later do a canattack type consideration
    {
        Vector3Int destinationGridPosition = walkableTileMap.WorldToCell(transform.position + ConvertDirectionToMovementVector(direction));
        if(!walkableTileMap.HasTile(destinationGridPosition) && !triggerTileMap.HasTile(destinationGridPosition))
        {
            return false;
        }
        if(collisionTileMap.HasTile(destinationGridPosition))
        {
            return false;
        }
        Tile DestinationTileType = (Tile)walkableTileMap.GetTile(destinationGridPosition);
        if(DestinationTileType == unit.opponentPaintTileType)
        {
            if(unit.unitInventory)
            {
                if(unit.unitInventory.PayCoin())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        if(isAIMove)//don't have ai move on to their own paint tpye for their own moves? what if they get themselves surrounded though?
        {//they should be able to move out if there is available space beyond their own, i guess that's not lieklyl though
            if(DestinationTileType == unit.yourPaintTileType)
            {
                return false;
            }
        }

        return true;
    }

    public static Vector3 ConvertDirectionToMovementVector(MovementDirections direction)
    {
        switch(direction)
        {
            case MovementDirections.None:
                return Vector3.zero;
            case MovementDirections.UpRight:
                return new Vector3(0.5f, 0.5f);
            case MovementDirections.Right:
                return new Vector3(1, 0);
            case MovementDirections.DownRight:
                return new Vector3(0.5f, -0.5f);
            case MovementDirections.DownLeft:
                return new Vector3(-0.5f, -0.5f);
            case MovementDirections.Left:
                return new Vector3(-1, 0);
            case MovementDirections.UpLeft:
                return new Vector3(-0.5f, 0.5f);
            default:
                return new Vector3(0, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mapPainter = GetComponent<MapPainter>();
        renderedLines = new Stack<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
