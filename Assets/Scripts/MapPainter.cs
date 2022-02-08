using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapPainter : MonoBehaviour
{
    private Tile paintableTile;
    //private Tile previousTile;//this will need to be passed in for commands
    public Tilemap walkableTilemap;

    public void PaintCurrentTile()
    {
        Vector3Int thisTile = walkableTilemap.WorldToCell(transform.position);
        //previousTile = (Tile)walkableTilemap.GetTile(thisTile);//does this work?
        walkableTilemap.SetTile(thisTile, paintableTile);
    }

    private void Start()
    {
        Unit unit = GetComponent<Unit>();

        switch(unit)
        {
            case Player p:
                paintableTile = unit.data.PlayerTileToPaint;
                unit.yourPaintTileType = unit.data.PlayerTileToPaint;
                unit.opponentPaintTileType = unit.data.EnemyTileToPaint;
                break;
            case Enemy e:
                paintableTile = unit.data.EnemyTileToPaint;
                unit.yourPaintTileType = unit.data.EnemyTileToPaint;
                unit.opponentPaintTileType = unit.data.PlayerTileToPaint;
                break;
            default:
                break;
        }
    }
}
