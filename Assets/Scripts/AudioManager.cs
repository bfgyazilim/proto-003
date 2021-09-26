using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource levelMusic, gameOverMusic, winMusic;

    public AudioSource[] music;
    public AudioSource[] sfx;

    public int levelMusicToPlay;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        levelMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(int musicToPlay)
    {
        for(int i = 0; i < music.Length; i++)
        {
            music[i].Stop();
        }

        music[musicToPlay].Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Play();
    }

    public void PlayGameOver()
    {
        levelMusic.Stop();

        gameOverMusic.Play();
    }

    public void PlayLevelWin()
    {
        levelMusic.Stop();

        winMusic.Play();
    }
}
