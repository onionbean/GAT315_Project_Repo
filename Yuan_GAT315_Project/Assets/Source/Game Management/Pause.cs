using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
    // Key for pausing
    float oldFixedDelta = 0;

    public static bool Paused { get; set; }

    private void OnEnable()
    {
        EventManager.Register("Pause", PauseGame);
        EventManager.Register("Unpause", UnPause);
    }

    private void OnDisable()
    {
        
    }

    // Use this for initialization
    void Start () {
        Paused = false;
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void PauseGame(GameObject sender, GameObject reciever = null)
    {
        Paused = true;

        Time.timeScale = 0;

        //oldFixedDelta = Time.fixedDeltaTime;
        //Time.fixedDeltaTime = 0;
    }

    void UnPause(GameObject sender, GameObject reciever = null)
    {
        Paused = false;

        Time.timeScale = 1;
        //Time.fixedDeltaTime = oldFixedDelta;
    }
}
