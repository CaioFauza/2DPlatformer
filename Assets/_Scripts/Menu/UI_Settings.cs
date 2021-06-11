using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_Settings : MonoBehaviour
{
    GameManager gm;
    public Slider backgroundVolume, sfxVolume;

    public AudioSource ambienceSource, sfxSource;
    private void OnEnable()
    {
        gm = GameManager.GetInstance();
    }

    public void MainMenu()
    {
        gm.ChangeState(GameManager.GameState.MENU);
    }

    public void ChangeAmbienceVolume()
    {
        ambienceSource.volume = backgroundVolume.value;
        if (Math.Abs(backgroundVolume.value - sfxSource.volume) > 0.2)
        {
            sfxVolume.value = ambienceSource.volume * 0.8f;
            sfxSource.volume = sfxVolume.value;
        }
    }

    public void ChangeSFXVolume()
    {
        sfxSource.volume = sfxVolume.value;
        if (Math.Abs(sfxVolume.value - ambienceSource.volume) > 0.2)
        {
            backgroundVolume.value = sfxSource.volume * 0.8f;
            ambienceSource.volume = backgroundVolume.value;
        }
    }
}
