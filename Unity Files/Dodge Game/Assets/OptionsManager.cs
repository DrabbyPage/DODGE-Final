using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class OptionsManager : MonoBehaviour {

    public int playerId;
    private Rewired.Player player;
    
    int menuState;
    public bool verticalAxisActive;
    float verticalAxisValue;
    public bool horizontalAxisActive;
    float horizontalAxisValue;

    public static bool audioOn, fxOn;
    public static int audioVolume, fxAmount;

    public GameObject musicIcon, fxIcon;
    public GameObject musicBar0, musicBar1, musicBar2, musicBar3, musicBar4, musicBar5, musicBar6, musicBar7, musicBar8;
    public GameObject fxBar0, fxBar1, fxBar2, fxBar3, fxBar4, fxBar5, fxBar6, fxBar7, fxBar8;
    public GameObject musicBarShadow, musicShadow;
    public GameObject fxBarShadow, fxShadow;

    public AudioClip buttonClick;
    AudioSource buttonClickSound;

    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);

        buttonClickSound = GetComponent<AudioSource>();

        menuState = 0;
        verticalAxisActive = false;
        verticalAxisValue = 0f;
        horizontalAxisActive = false;
        horizontalAxisValue = 0f;

        audioOn = GameDetails.audioOn;
        fxOn = GameDetails.fxOn;
        audioVolume = GameDetails.audioVolume;
        fxAmount = GameDetails.fxVolume;

        AdjustVolume();
        AdjustFX();

        if (audioOn)
        {
            musicIcon.SetActive(true);
        }
        if (!audioOn)
        {
            musicIcon.SetActive(false);
        }

        if (fxOn)
        {
            fxIcon.SetActive(true);
        }
        if (!fxOn)
        {
            fxIcon.SetActive(false);
        }

        musicBarShadow.SetActive(false);
        musicShadow.SetActive(false);
        fxBarShadow.SetActive(true);
        fxShadow.SetActive(true);
    }

    void Update()
    {
        Debug.Log("Audio On? " + audioOn);

        //Makes the axis like a button and only moved once
        if (player.GetAxis("LSV") != 0)
        {
            if (!verticalAxisActive)
            {
                SwitchOption();
                MenuState();
                verticalAxisActive = true;
            }
        }
        if (player.GetAxis("LSV") == 0)
        {
            verticalAxisActive = false;
            verticalAxisValue = 0f;
        }

        if (player.GetAxis("LSH") != 0)
        {
            if (!horizontalAxisActive)
            {
                if (menuState == 0 && audioOn)
                {
                    if (player.GetAxis("LSH") < 0)
                    {
                        audioVolume--;
                    }
                    if (player.GetAxis("LSH") > 0)
                    {
                        audioVolume++;
                    }
                    AdjustVolume();
                }

                if (menuState == 1)
                {
                    if (player.GetAxis("LSH") < 0)
                    {
                        fxAmount--;
                    }
                    if (player.GetAxis("LSH") > 0)
                    {
                        fxAmount++;
                    }
                    AdjustFX();
                }
                horizontalAxisActive = true;
            }
        }
        if (player.GetAxis("LSH") == 0)
        {
            horizontalAxisActive = false;
            horizontalAxisValue = 0f;
        }

        if (player.GetButtonDown("A"))
        {
            if (menuState == 0)
            {
                SetAudioOnOrOff();
            }

            if (menuState == 1)
            {
                SetFXOnOrOff();
            }

            if (audioOn)
            {
                buttonClickSound.PlayOneShot(buttonClick, 1f);
            }
        }

        if (player.GetButtonDown("B"))
        {
            if (audioOn)
            {
                buttonClickSound.PlayOneShot(buttonClick, 1f);
            }

            if (menuState == 0 || menuState == 1)
            {
                SceneManager.LoadScene("Start_Menu");
            }
        }
    }

    void MenuState()
    {
        switch (menuState)
        {
            case -1:
                SceneManager.LoadScene("Start_Menu");
                break;

            case 0:
                Debug.Log("Main Audio");
                //if (audioOn)
                //{
                    musicBarShadow.SetActive(false);
                //}
                musicShadow.SetActive(false);
                fxBarShadow.SetActive(true);
                fxShadow.SetActive(true);
                //Audio();
                break;

            case 1:
                Debug.Log("Main SFX");
                musicBarShadow.SetActive(true);
                musicShadow.SetActive(true);
                //if (fxOn)
                //{
                    fxBarShadow.SetActive(false);
                //}
                fxShadow.SetActive(false);
                //SFX();
                break;

            case 2:
                menuState = 1;
                break;
        }
    }

    void SwitchOption()
    {
        if (audioOn)
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);
        }

        if (player.GetAxis("LSV") > 0)
        {
            menuState = 0;
        }

        if (player.GetAxis("LSV") < 0)
        {
            menuState = 1;
        }
    }

    void SetAudioOnOrOff()
    {
        Debug.Log("Check Audio");
        if (audioOn)
        {
            Debug.Log("Audio Off");
            audioOn = false;
            musicIcon.SetActive(false);
            musicBarShadow.SetActive(true);
            AudioListener.volume = 0.0f;
        }
        else if (!audioOn)
        {
            Debug.Log("Audio On");
            audioOn = true;
            musicIcon.SetActive(true);
            musicBarShadow.SetActive(false);
            AdjustVolume();
        }

        GameDetails.audioOn = audioOn;
    }

    void SetFXOnOrOff()
    {
        Debug.Log("Check FX");
        if (fxOn)
        {
            Debug.Log("FX Off");
            fxOn = false;
            fxIcon.SetActive(false);
            fxBarShadow.SetActive(true);
        }
        else if (!fxOn)
        {
            Debug.Log("FX On");
            fxOn = true;
            fxIcon.SetActive(true);
            fxBarShadow.SetActive(false);
        }

        GameDetails.fxOn = fxOn;
    }

    void AdjustVolume()
    {
        //0.0f to 1.0f
        // 9 adjusters (0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f)
        switch (audioVolume)
        {
            case 0:
                AudioListener.volume = 0.2f;
                audioVolume = 1;
                break;

            case 1:
                AudioListener.volume = 0.2f;
                musicBar0.SetActive(true);
                musicBar1.SetActive(false);
                musicBar2.SetActive(false);
                musicBar3.SetActive(false);
                musicBar4.SetActive(false);
                musicBar5.SetActive(false);
                musicBar6.SetActive(false);
                musicBar7.SetActive(false);
                musicBar8.SetActive(false);
                break;

            case 2:
                AudioListener.volume = 0.3f;
                musicBar0.SetActive(true);
                musicBar1.SetActive(true);
                musicBar2.SetActive(false);
                musicBar3.SetActive(false);
                musicBar4.SetActive(false);
                musicBar5.SetActive(false);
                musicBar6.SetActive(false);
                musicBar7.SetActive(false);
                musicBar8.SetActive(false);
                break;

            case 3:
                AudioListener.volume = 0.4f;
                musicBar0.SetActive(true);
                musicBar1.SetActive(true);
                musicBar2.SetActive(true);
                musicBar3.SetActive(false);
                musicBar4.SetActive(false);
                musicBar5.SetActive(false);
                musicBar6.SetActive(false);
                musicBar7.SetActive(false);
                musicBar8.SetActive(false);
                break;

            case 4:
                AudioListener.volume = 0.5f;
                musicBar0.SetActive(true);
                musicBar1.SetActive(true);
                musicBar2.SetActive(true);
                musicBar3.SetActive(true);
                musicBar4.SetActive(false);
                musicBar5.SetActive(false);
                musicBar6.SetActive(false);
                musicBar7.SetActive(false);
                musicBar8.SetActive(false);
                break;

            case 5:
                AudioListener.volume = 0.6f;
                musicBar0.SetActive(true);
                musicBar1.SetActive(true);
                musicBar2.SetActive(true);
                musicBar3.SetActive(true);
                musicBar4.SetActive(true);
                musicBar5.SetActive(false);
                musicBar6.SetActive(false);
                musicBar7.SetActive(false);
                musicBar8.SetActive(false);
                break;

            case 6:
                AudioListener.volume = 0.7f;
                musicBar0.SetActive(true);
                musicBar1.SetActive(true);
                musicBar2.SetActive(true);
                musicBar3.SetActive(true);
                musicBar4.SetActive(true);
                musicBar5.SetActive(true);
                musicBar6.SetActive(false);
                musicBar7.SetActive(false);
                musicBar8.SetActive(false);
                break;

            case 7:
                AudioListener.volume = 0.8f;
                musicBar0.SetActive(true);
                musicBar1.SetActive(true);
                musicBar2.SetActive(true);
                musicBar3.SetActive(true);
                musicBar4.SetActive(true);
                musicBar5.SetActive(true);
                musicBar6.SetActive(true);
                musicBar7.SetActive(false);
                musicBar8.SetActive(false);
                break;

            case 8:
                AudioListener.volume = 0.9f;
                musicBar0.SetActive(true);
                musicBar1.SetActive(true);
                musicBar2.SetActive(true);
                musicBar3.SetActive(true);
                musicBar4.SetActive(true);
                musicBar5.SetActive(true);
                musicBar6.SetActive(true);
                musicBar7.SetActive(true);
                musicBar8.SetActive(false);
                break;

            case 9:
                AudioListener.volume = 1.0f;
                musicBar0.SetActive(true);
                musicBar1.SetActive(true);
                musicBar2.SetActive(true);
                musicBar3.SetActive(true);
                musicBar4.SetActive(true);
                musicBar5.SetActive(true);
                musicBar6.SetActive(true);
                musicBar7.SetActive(true);
                musicBar8.SetActive(true);
                break;

            case 10:
                AudioListener.volume = 1.0f;
                audioVolume = 9;
                break;
        }

        if (audioOn)
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);
        }

        GameDetails.audioVolume = audioVolume;
    }

    void AdjustFX()
    {
        // 9 adjusters
        switch (fxAmount)
        {
            case 0:
                fxAmount = 1;
                break;

            case 1:
                fxBar0.SetActive(true);
                fxBar1.SetActive(false);
                fxBar2.SetActive(false);
                fxBar3.SetActive(false);
                fxBar4.SetActive(false);
                fxBar5.SetActive(false);
                fxBar6.SetActive(false);
                fxBar7.SetActive(false);
                fxBar8.SetActive(false);
                break;

            case 2:
                fxBar0.SetActive(true);
                fxBar1.SetActive(true);
                fxBar2.SetActive(false);
                fxBar3.SetActive(false);
                fxBar4.SetActive(false);
                fxBar5.SetActive(false);
                fxBar6.SetActive(false);
                fxBar7.SetActive(false);
                fxBar8.SetActive(false);
                break;

            case 3:
                fxBar0.SetActive(true);
                fxBar1.SetActive(true);
                fxBar2.SetActive(true);
                fxBar3.SetActive(false);
                fxBar4.SetActive(false);
                fxBar5.SetActive(false);
                fxBar6.SetActive(false);
                fxBar7.SetActive(false);
                fxBar8.SetActive(false);
                break;

            case 4:
                fxBar0.SetActive(true);
                fxBar1.SetActive(true);
                fxBar2.SetActive(true);
                fxBar3.SetActive(true);
                fxBar4.SetActive(false);
                fxBar5.SetActive(false);
                fxBar6.SetActive(false);
                fxBar7.SetActive(false);
                fxBar8.SetActive(false);
                break;

            case 5:
                fxBar0.SetActive(true);
                fxBar1.SetActive(true);
                fxBar2.SetActive(true);
                fxBar3.SetActive(true);
                fxBar4.SetActive(true);
                fxBar5.SetActive(false);
                fxBar6.SetActive(false);
                fxBar7.SetActive(false);
                fxBar8.SetActive(false);
                break;

            case 6:
                fxBar0.SetActive(true);
                fxBar1.SetActive(true);
                fxBar2.SetActive(true);
                fxBar3.SetActive(true);
                fxBar4.SetActive(true);
                fxBar5.SetActive(true);
                fxBar6.SetActive(false);
                fxBar7.SetActive(false);
                fxBar8.SetActive(false);
                break;

            case 7:
                fxBar0.SetActive(true);
                fxBar1.SetActive(true);
                fxBar2.SetActive(true);
                fxBar3.SetActive(true);
                fxBar4.SetActive(true);
                fxBar5.SetActive(true);
                fxBar6.SetActive(true);
                fxBar7.SetActive(false);
                fxBar8.SetActive(false);
                break;

            case 8:
                fxBar0.SetActive(true);
                fxBar1.SetActive(true);
                fxBar2.SetActive(true);
                fxBar3.SetActive(true);
                fxBar4.SetActive(true);
                fxBar5.SetActive(true);
                fxBar6.SetActive(true);
                fxBar7.SetActive(true);
                fxBar8.SetActive(false);
                break;

            case 9:
                fxBar0.SetActive(true);
                fxBar1.SetActive(true);
                fxBar2.SetActive(true);
                fxBar3.SetActive(true);
                fxBar4.SetActive(true);
                fxBar5.SetActive(true);
                fxBar6.SetActive(true);
                fxBar7.SetActive(true);
                fxBar8.SetActive(true);
                break;

            case 10:
                fxAmount = 9;
                break;
        }

        if (audioOn)
        {
            buttonClickSound.PlayOneShot(buttonClick, 1f);
        }

        GameDetails.fxVolume = fxAmount;
    }
}
