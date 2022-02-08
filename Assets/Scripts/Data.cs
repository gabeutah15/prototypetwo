using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Data : MonoBehaviour
{
    public Tile EnemyTileToPaint;
    public Tile PlayerTileToPaint;
    public Tile GreenTile;

    public Tilemap walkableTileMap;//maybe get this from data or something?
    [HideInInspector]
    public int NumEnemiesRemaining;
    [SerializeField]
    Text NumGreenTiles;
    private int NumGreenTilesInt;
    [SerializeField]
    Text NumPlayerTiles;
    [SerializeField]
    Text NumEnemyTiles;
    [SerializeField]
    Text ScoreNum;

    [SerializeField]
    Text TitlePlayerTiles;
    [SerializeField]
    Text TitleEnemyTiles;
    [SerializeField]
    Text TitleScoreNum;
    [SerializeField]
    Text TitleGreenTiles;
    [SerializeField]
    Text WinText;
    [SerializeField]
    Text LoseText;
    [SerializeField]
    Text TutorialText;
    [SerializeField]
    Image TutorialBackground;
    [SerializeField]
    Button TutorialStartButton;
    [SerializeField]
    Button RestartButton;

    public void StartButton()
    {
        TutorialText.enabled = false;
        TutorialBackground.enabled = false;
        TutorialStartButton.image.enabled = false;
        TutorialStartButton.GetComponentInChildren<Text>().enabled = false;
        TutorialStartButton.enabled = false;


    }

    private void Start()
    {
        NumEnemiesRemaining = FindObjectsOfType<Enemy>().Length;
        //NumGreenTilesInt = GetTileAmountSprite(null);
        NumGreenTilesInt = GetNumTilesWithThisSprite(GreenTile.sprite);
        NumGreenTiles.text = NumGreenTilesInt.ToString();

        TitleScoreNum.enabled = false;
        TitleEnemyTiles.enabled = false;
        TitlePlayerTiles.enabled = false;
        ScoreNum.enabled = false;
        NumEnemyTiles.enabled = false;
        NumPlayerTiles.enabled = false;
        NumGreenTiles.enabled = false;
        TitleGreenTiles.enabled = false;
        WinText.enabled = false;
        LoseText.enabled = false;
        RestartButton.image.enabled = false;
        RestartButton.GetComponentInChildren<Text>().enabled = false;
        RestartButton.enabled = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void EndGame()
    {
        TitleScoreNum.enabled = true;
        TitleEnemyTiles.enabled = true;
        TitlePlayerTiles.enabled = true;
        ScoreNum.enabled = true;
        NumEnemyTiles.enabled = true;
        NumPlayerTiles.enabled = true;
        NumGreenTiles.enabled = true;
        TitleGreenTiles.enabled = true;

        RestartButton.enabled = true;
        RestartButton.image.enabled = true;
        RestartButton.GetComponentInChildren<Text>().enabled = true;

        int numPlayerTiles = GetNumTilesWithThisSprite(PlayerTileToPaint.sprite);
        NumPlayerTiles.text = numPlayerTiles.ToString();
        int numEnemyTiles = GetNumTilesWithThisSprite(EnemyTileToPaint.sprite);
        NumEnemyTiles.text = numEnemyTiles.ToString();
        int numRemainingGreenTiles = NumGreenTilesInt - numPlayerTiles - numEnemyTiles;
        NumGreenTiles.text = numRemainingGreenTiles.ToString();

        int score = numRemainingGreenTiles - numEnemyTiles;
        if(score > 0) { WinText.enabled = true; } else { LoseText.enabled = true; }

        ScoreNum.text = score.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            EndGame();
        }
    }

    public int GetNumTilesWithThisSprite(Sprite targetSprite)
    {
        int count = 0;

        BoundsInt tileMapBounds = walkableTileMap.cellBounds;
        foreach(Vector3Int pos in tileMapBounds.allPositionsWithin)
        {
            Tile tile = walkableTileMap.GetTile<Tile>(pos);
            if(tile != null)
            {
                if(targetSprite)
                {
                    if(tile.sprite == targetSprite)
                    {
                        count += 1;
                    }

                }
                else
                {
                    count += 1;
                }
            }
        }

        Debug.Log(count);
        return count;
    }
}
