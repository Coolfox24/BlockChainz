using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField] float objectivesNeeded = 2;
    [SerializeField] float deathDelay = 2f;
    [SerializeField] float winDelay = 1f;

    [SerializeField] Tile blueTile;
    [SerializeField] Tile redTile;
    [SerializeField] Tilemap blueTileMap;
    [SerializeField] Tilemap redTileMap;

    private bool blueIsBlue = true;
    private bool isLevelOver = false;
    private int currentObjectivesComplete;
    AudioManager audioManager;
    TextMeshProUGUI timeText;
    Settings gameSettings;
 
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlayMusic("level");
        gameSettings = FindObjectOfType<Settings>();

        GameObject.Find("DeathCount").GetComponent<TextMeshProUGUI>().text = gameSettings.GetDeaths().ToString("00");
        timeText = GameObject.Find("CurTime").GetComponent<TextMeshProUGUI>();
        timeText.text = gameSettings.GetTime().ToString("0000");
        gameSettings.SetCountTime(true);
    }


    private void Update()
    {
        timeText.text = gameSettings.GetTime().ToString("0000");
    }

    public void LoseLevel()
    {
        //Debug.Log("Level Lost")
        isLevelOver = true;
        var players = FindObjectsOfType<PlayerController>();
        gameSettings.AddDeath();

        foreach(PlayerController p in players)
        {
            p.StopActions();
        }
        audioManager.PlayClip("die", 0);
        StartCoroutine(LoadLevel(deathDelay, 0));
    }

    IEnumerator LoadLevel(float delay, int additionalIndex)
    {
        gameSettings.SetCountTime(false);

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + additionalIndex);
    }

    public bool IsLevelOver()
    {
        return isLevelOver;
    }

    public void AddObjective()
    {
        currentObjectivesComplete++;
        Debug.Log("Current Obj : " + currentObjectivesComplete);
        if (currentObjectivesComplete == objectivesNeeded)
        {
            isLevelOver = true;
            audioManager.PlayClip("win", 0);
            //Last level will be win screen - takes you back to start menu
            var players = FindObjectsOfType<PlayerController>();
            foreach (PlayerController p in players)
            {
                p.StopActions();
            }

            StartCoroutine(LoadLevel(winDelay, 1));
        }
    }

    public void ReduceObjective()
    {
        currentObjectivesComplete--;
        Debug.Log("Current Obj : " + currentObjectivesComplete);
    }

    public void ToggleBarriers()
    {
        if(blueIsBlue)
        {
            //swap blue to red
            ToggleLayer(blueTileMap, redTile);
            blueTileMap.gameObject.layer = LayerMask.NameToLayer("Red");
            //swap red to blue
            ToggleLayer(redTileMap, blueTile);
            redTileMap.gameObject.layer = LayerMask.NameToLayer("Blue");

            blueIsBlue = false;
        }
        else
        {
            //swap blue to blue
            ToggleLayer(blueTileMap, blueTile);
            blueTileMap.gameObject.layer = LayerMask.NameToLayer("Blue");
            //swap red to red
            ToggleLayer(redTileMap, redTile);
            redTileMap.gameObject.layer = LayerMask.NameToLayer("Red");

            blueIsBlue = true;
        }
    }

    private void ToggleLayer(Tilemap map, Tile newTile)
    {
        audioManager.PlayClip("toggle", 0);
        foreach (var tile in map.cellBounds.allPositionsWithin)
        {
            Vector3Int position = new Vector3Int(tile.x, tile.y, tile.z);
            if (map.HasTile(position))
            {
                map.SetTile(position, newTile);
            }
        }
    }
}
