using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    //Default Values
    float musicVolume = 0.4f;
    float sfxVolume = 0.6f;

    float timePlaying;
    int deaths;
    bool countTime = false;

    private void Awake()
    {
        if (FindObjectsOfType<Settings>().Length > 1)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            if (PlayerPrefs.HasKey("musicVolume"))
            {
                musicVolume = PlayerPrefs.GetFloat("musicVolume");
            }
            else
            {
                PlayerPrefs.SetFloat("musicVolume", musicVolume);
            }

            if (PlayerPrefs.HasKey("sfxVolume"))
            {
                sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            }
            else
            {
                PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
            }
        }
    }

    private void Update()
    {
        if(countTime)
        {
            timePlaying += Time.deltaTime;
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);

        FindObjectOfType<AudioManager>().UpdateMusicVolumes(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("sfxVlume", volume);
    }

    public void ResetTime()
    {
        timePlaying = 0;
    }

    public float GetTime()
    {
        return timePlaying;
    }

    public int GetDeaths()
    {
        return deaths;
    }

    public void AddDeath()
    {
        if(deaths > 99)
        {
            //Cap on deaths
            deaths = 99;
        }
        deaths++;
    }

    public void SetCountTime(bool countTime)
    {
        this.countTime = countTime;
    }

    public void ToggleCountTime()
    {
        countTime = !countTime;
    }
}
