﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public static class SoundManager {

    public enum Sound {
        BirdJump,
        Score,
        Lose,
        ButtonOver,
        ButtonClick,
    }

    /**
    * Method to play the sound.
    **/
    public static void PlaySound(Sound sound) {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    /**
    * Method to get the right audioclip.
    **/
    private static AudioClip GetAudioClip(Sound sound) {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray) {
            if (soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    /**
    * Method to add sounds to the buttons.
    **/
    public static void AddButtonSounds(this Button_UI buttonUI) {
        buttonUI.MouseOverOnceFunc += () => PlaySound(Sound.ButtonOver);
        buttonUI.ClickFunc += () => PlaySound(Sound.ButtonClick);
    }

}
