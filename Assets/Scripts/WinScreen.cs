using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalDeathCount;
    [SerializeField] TextMeshProUGUI finalTime;

    // Start is called before the first frame update
    void Start()
    {
        Settings gameSettings = FindObjectOfType<Settings>();

        //Play win screen music here
        AudioManager am = FindObjectOfType<AudioManager>();
        am.PlayMusic("menu");
        am.PlayClip("winScreen", 0);

        finalDeathCount.text = gameSettings.GetDeaths().ToString("00");
        finalTime.text = gameSettings.GetTime().ToString("0000");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
