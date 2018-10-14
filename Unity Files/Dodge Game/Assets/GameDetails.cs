using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDetails : MonoBehaviour {

    public static int audioVolume, fxVolume;
    public static bool audioOn, fxOn;

	void Start ()
    {
        audioVolume = 9;
        fxVolume = 9;

        audioOn = true;
        fxOn = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*
        Debug.Log("Audio Volume: " + audioVolume);
        Debug.Log("Audio On: " + audioOn);
        Debug.Log("FX Volume: " + fxVolume);
        Debug.Log("FX On: " + fxOn);
        */
	}
}
