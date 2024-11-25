using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
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

    public AudioClip BackgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}