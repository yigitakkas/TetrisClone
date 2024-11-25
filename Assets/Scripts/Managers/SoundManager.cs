using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public bool MusicEnabled = true;
    public bool FxEnabled = true;

    [Range(0,1)]
    public float MusicVolume = 1.0f;
    [Range(0, 1)]
    public float FxVolume = 1.0f;

    public AudioClip ClearRowSound;

    public AudioClip MoveSound;

    public AudioClip DropSound;

    public AudioClip GameOverSound;

    public AudioClip ErrorSound;

    private AudioClip _backgroundMusic;

    public AudioSource MusicSource;

    public AudioClip[] MusicClips;

    public AudioClip[] VocalClips;

    public AudioClip GameOverVocalClip;

    public IconToggle MusicIconToggle;

    public IconToggle FxIconToggle;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _backgroundMusic = GetRandomClip(MusicClips);
        PlayBackgroundMusic(_backgroundMusic);
    }

    public AudioClip GetRandomClip(AudioClip[] clips)
    {
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        return randomClip;
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!MusicEnabled || !musicClip || !MusicSource)
            return;

        MusicSource.Stop();
        MusicSource.clip = musicClip;
        MusicSource.volume = MusicVolume;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    void UpdateMusic()
    {
        if(MusicSource.isPlaying != MusicEnabled)
        {
            if(MusicEnabled)
            {
                PlayBackgroundMusic(_backgroundMusic);
            } else
            {
                MusicSource.Stop();
            }
        }
    }

    public void ToggleMusic()
    {
        MusicEnabled = !MusicEnabled;
        UpdateMusic();

        if(MusicIconToggle)
        {
            MusicIconToggle.ToggleIcon(MusicEnabled);
        }
    }

    public void ToggleFX()
    {
        FxEnabled = !FxEnabled;
        if(FxIconToggle)
        {
            FxIconToggle.ToggleIcon(FxEnabled);
        }
    }
}