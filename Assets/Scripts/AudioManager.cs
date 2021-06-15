using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    private struct AudioStorage
    {
        public string clipName;
        public AudioClip clip;
    }

    [SerializeField] AudioStorage[] clips;
    [SerializeField] AudioStorage[] bgm;

    Dictionary<string, AudioClip> clipDict;
    Dictionary<string, AudioClip> musicDict;

    Queue<AudioSource> musicPlayers;
    Settings gameSettings;

    private void Awake()
    {
        if(FindObjectsOfType<AudioManager>().Length > 1)
        {
            //Destroy
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            clipDict = new Dictionary<string, AudioClip>();
            musicDict = new Dictionary<string, AudioClip>();
            musicPlayers = new Queue<AudioSource>();
            foreach(AudioStorage aS in clips)
            {
                clipDict.Add(aS.clipName, aS.clip);
            }

            foreach(AudioStorage aS in bgm)
            {
                musicDict.Add(aS.clipName, aS.clip);
            }
        }
    }

    private void OnEnable()
    {
        gameSettings = FindObjectOfType<Settings>();
    }

    public void PlayClip(string clipName, float pitch)
    {
        if(!clipDict.ContainsKey(clipName))
        {
            Debug.Log("No clip named : " + clipName);
            return;
        }
        
        AudioClip clip = clipDict[clipName];
        GameObject audioPlayer = new GameObject("OneShotAudio");
        Destroy(audioPlayer, clip.length);

        AudioSource audioSource = audioPlayer.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = gameSettings.GetSFXVolume();
        if (pitch > 0)
        {
            audioSource.pitch = pitch;
        }
        audioSource.Play();
        
    }

    public void PlayMusic(string clipName)
    {
        if(!musicDict.ContainsKey(clipName))
        {
            Debug.Log("No music named : " + clipName);
            return;
        }

        AudioClip clip = musicDict[clipName];


        if(musicPlayers.Count == 1)
        {
            if (musicPlayers.Peek().clip == clip)
            {
                //Already playing the song
                return;
            }
            //Do some fading between the music players
            StartCoroutine(FadeMusic(musicPlayers.Dequeue(), 0, 2f));

            AudioSource musicPlayer = PlayMusic(clip);
            musicPlayer.volume = 0;
            StartCoroutine(FadeMusic(musicPlayer, gameSettings.GetMusicVolume(), 2f));

        }
        else if(musicPlayers.Count == 0)
        {
            PlayMusic(clip);
        }
        else
        {
            //Multiple musics trying to queue up at same time, instantly terminate lower queue songs and play newest 1
            musicPlayers.Clear();
            PlayMusic(clip);
        }
    }

    private IEnumerator FadeMusic(AudioSource source, float targetVolume, float fadeTime)
    {
        float startingVol = source.volume;
        float curTime = 0f;
        while(source.volume != targetVolume)
        {
            source.volume = Mathf.Lerp(startingVol, targetVolume, curTime / fadeTime);
            curTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private AudioSource PlayMusic(AudioClip clip)
    {
        //spawn a new music audio source and add it to the list
        GameObject musicPlayer = new GameObject("MusicPlayer");
        DontDestroyOnLoad(musicPlayer);
        AudioSource musicSource = musicPlayer.AddComponent<AudioSource>();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
        musicSource.volume = gameSettings.GetMusicVolume();

        musicPlayers.Enqueue(musicSource);
        return musicSource;
    }

    public void UpdateMusicVolumes(float volume)
    {
        foreach(AudioSource source in musicPlayers)
        {
            source.volume = volume;
        }
    }
}
