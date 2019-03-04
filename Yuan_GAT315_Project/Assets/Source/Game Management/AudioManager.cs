using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // Audio source to be played by other objects 
    AudioSource _soundPlayer;

    // List of audiosources from 
    //List<AudioSource> 

	// Use this for initialization
	void Start ()
    {
        _soundPlayer = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    // Use soundplayer to play a clip
    public void PlayOneShot(AudioClip clip, float volumeScale)
    {
        _soundPlayer.PlayOneShot(clip, volumeScale);
    }
}
