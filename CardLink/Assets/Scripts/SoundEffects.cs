using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum SoundType
{
    SHUFFLE,
    DRAW,
    HOVER,
    CLICKED,
    PLAYCARD,
    DEATHCARD,
    PLAYERHURT,
    DEFEAT,
    VICTORY
}

[RequireComponent(typeof(AudioSource))]
public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundEffects instance;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound]);
    }

}