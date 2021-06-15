using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI volText;
    [SerializeField] TextMeshProUGUI sfxText;
    [SerializeField] Slider volSlider;
    [SerializeField] Slider sfxSlider;

    [SerializeField] GameObject options;

    Settings settings;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().PlayMusic("menu");
        settings = FindObjectOfType<Settings>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowOptions()
    {
        options.SetActive(true);
        volText.text = Mathf.Floor(settings.GetMusicVolume() * 100).ToString();
        sfxText.text = Mathf.Floor(settings.GetSFXVolume() * 100).ToString();

        volSlider.value = settings.GetMusicVolume();
        sfxSlider.value = settings.GetSFXVolume();
    }

    public void MusicSliderChange()
    {
        volText.text = Mathf.Floor(volSlider.value * 100).ToString();
        settings.SetMusicVolume(volSlider.value);
    }

    public void SFXSliderChanged()
    {
        sfxText.text = Mathf.Floor(sfxSlider.value * 100).ToString();
        settings.SetSFXVolume(sfxSlider.value);
    }

    public void HideOptions()
    {
        options.SetActive(false);
    }
}
