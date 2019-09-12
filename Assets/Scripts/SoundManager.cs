using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<AudioSource> soundEffects = new List<AudioSource>();

    void Start()
    {
        soundEffects = new List<AudioSource>(GetComponentsInChildren<AudioSource>());
    }

    public void PlaySound(string soundName)
    {
        // Search Sound List for 'soundName'
        foreach (var soundEffect in soundEffects)
        {
            // If found Sound
            if (soundEffect.name == soundName)
            {
                //  Play that sound!
                soundEffect.Play();
            }
        }
    }
}
