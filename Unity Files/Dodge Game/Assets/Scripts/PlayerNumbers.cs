using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNumbers : MonoBehaviour {

    static int numberOfPlayers = 0;

    public GameObject playerNumCanvas;
    public GameObject playerLoginCanvas;

    void Start()
    {
        numberOfPlayers = 0;
        playerNumCanvas.SetActive(true);
        playerLoginCanvas.SetActive(false);
    }

	public void SinglePlayer()
    {
        numberOfPlayers = 1;
        playerNumCanvas.SetActive(false);
        playerLoginCanvas.SetActive(true);
    }

    public void TwoPlayer()
    {
        numberOfPlayers = 2;
        playerNumCanvas.SetActive(false);
        playerLoginCanvas.SetActive(true);
    }

    public void ThreePlayer()
    {
        numberOfPlayers = 3;
        playerNumCanvas.SetActive(false);
        playerLoginCanvas.SetActive(true);
    }

    public void FourPlayer()
    {
        numberOfPlayers = 4;
        playerNumCanvas.SetActive(false);
        playerLoginCanvas.SetActive(true);
    }
}
