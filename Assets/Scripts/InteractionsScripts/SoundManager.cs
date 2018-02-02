using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    [SerializeField]
    AudioSource selectionSound;
    [SerializeField]
    AudioSource releaseSound;
    [SerializeField]
    AudioSource hoverSound;


    // The sound that is playes when an object is selected
    public void PlaySelectionSound(Vector3 pos)
    {
        selectionSound.gameObject.transform.position = pos;
        selectionSound.enabled = true;
        selectionSound.Play();
    }


    // The sound that is played when an object is released
    public void PlayDropSound(Vector3 pos)
    {
        releaseSound.gameObject.transform.position = pos;
        releaseSound.enabled = true;
        releaseSound.Play();
    }


    // The sound that is played when we can select an object
    public void PlayHoverSound(Vector3 pos)
    {
        hoverSound.gameObject.transform.position = pos;
        hoverSound.enabled = true;
        hoverSound.Play();
    }


}
