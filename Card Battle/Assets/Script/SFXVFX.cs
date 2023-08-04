using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXVFX : MonoBehaviour
{
    public Action play;
    public AudioSource drawSFX;

    public void PlaySFXVFX() 
    {
        play();
    }

    public void DeleteSFXVFX()
    {
        play = null;
    }

    public void PlayDrawSFX()
    {
        drawSFX = GetComponent<AudioSource>();
        drawSFX.Play();
    }

}
