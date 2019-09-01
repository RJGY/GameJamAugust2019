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

    public List<AudioSource> sounds = new List<AudioSource>();

    void Start()
    {
        sounds = new List<AudioSource>(GetComponentsInChildren<AudioSource>());
    }

    public void PlaySound(string soundName)
    {
        // Search Sound List for 'soundName'
        foreach (var sound in sounds)
        {
            // If found Sound
            if (sound.name == soundName)
            {
                //  Play that sound!
                sound.Play();
            }
        }
    }
}
