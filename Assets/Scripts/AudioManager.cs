using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public static AudioManager Instance;
    [SerializeField] Sound[] musicSounds, sfxSounds;
    [SerializeField] AudioSource musicSrc, sfxSrc;

    AudioClip currentSound;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("A" + this.GetType() + "MonoBehaviour already exists");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(AudioClip snd)
    {
        musicSrc.Stop();
        musicSrc.clip = snd;
        musicSrc.Play();
    }
    public void PlayMusic(string nam)
    {
        Sound selected = Array.Find(musicSounds, x => x.name == nam);
        PlayMusic(selected.audio);
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audio;
}