using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInfoScript : MonoBehaviour {

    // player info for team and character
    public int infoTeam;
    public int characterClass;
    public int playerNum;

    // info whether a player exists
    public bool ableToSpawn;
    public bool isOut = false;

    // the name for the player's controls
    public string playerHor = "LSH";
    public string playerVer = "LSV";
    public string playerJump = "A";
    public string playerPickup = "X";
    public string playerAimHor = "RSH";
    public string playerAimVer = "RSV";
    public string playerThrow = "RT";
    public string playerShield = "LT";
    

    Vector2 spawn;

    string ballSavedName;
    int numBalls = 0;
    int maxBalls = 1;

    Rewired.Player player;

    SpriteRenderer spriteRen;
    Animator anim;

    public GameObject ballPrefab;
    public GameObject ballUI;

    // Use this for initialization
    void Start () {
        InitPlayer();
        characterClass = GameObject.Find("GameManager").GetComponent<ManagerScript>().GetPlayerClass(tag);
        ableToSpawn = GameObject.Find("GameManager").GetComponent<ManagerScript>().GetPlayerAbilityToSpawn(tag);
        SetPlayerNum();
        spriteRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.SetInteger("CharacterClass", characterClass);
    }

    // Update is called once per frame
    void Update () {

        gameObject.SetActive(ableToSpawn);

        anim.SetBool("Throwing", false);
        anim.SetBool("Catching", false);
        anim.SetBool("Dashing", false);
        
        if (!isOut && ableToSpawn)
        {
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.2f, 1.85f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.04f, -0.075f);

        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.2f, 0.1f);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2( 0f, -1f);

        }

        GetComponent<Animator>().SetBool("Dead", isOut);

    }

    public void SetTeam(int teamNum)
    {
        infoTeam = teamNum;
    }

    public void SetBallName(string newName)
    {
        ballSavedName = newName;
    }

    public void IncreaseBalls()
    {
        numBalls++;
    }

    public void DecreaseBalls()
    {
        numBalls--;
    }

    void SetPlayerNum()
    {
        
        if (tag == "Player1")
        {
            playerNum = 0;
            infoTeam = 1;
        }
        else if (tag == "Player2")
        {
            playerNum = 1;
            infoTeam = 2;
        }
        else if (tag == "Player3")
        {
            playerNum = 2;
            infoTeam = 1;
        }
        else if (tag == "Player4")
        {
            playerNum = 3;
            infoTeam = 2;
        }

        player = ReInput.players.GetPlayer(playerNum);

    }

    public void KillPlayer()
    {
        if (numBalls > 0)
        {
            GameObject ball = Instantiate(ballPrefab, gameObject.transform);
            ball.name = ballSavedName;
            numBalls--;
            GetComponent<PlayerMovement>().DecreaseBall();
            GetComponent<Animator>().SetBool("HoldingBall", false);

        }
        StartCoroutine(Disappear());
        isOut = true;
    }

    public void ResetPos()
    {
        gameObject.transform.position = spawn;
    }

    public void ResetPlayer()
    {
        if (ableToSpawn)
        {
            gameObject.transform.position = spawn;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<PlayerSkillScript>().SetShieldHealth(1);
            spriteRen.flipX = gameObject.transform.position.x > 0;

            ballUI.SetActive(false);

            if (numBalls > 0)
            {
                //Debug.Log(numBalls + " is " + name);
                GameObject ball = Instantiate(ballPrefab);
                ball.name = ballSavedName;
                numBalls = 0;
                ball.GetComponent<BallScript>().ResetPos();
                GetComponent<PlayerMovement>().DecreaseBall();
                GetComponent<Animator>().SetBool("HoldingBall", false);

                //Debug.Log(numBalls + name);
            }

            anim.SetInteger("CharacterClass", characterClass);
            anim.SetBool("Dead", false);

            gameObject.SetActive(true);

            isOut = false;
        }

    }

    void InitPlayer() //TODO add multiplayer options
    {
        spawn = gameObject.transform.position;


        if (gameObject.tag == "Player1")
        {
            spawn = GameObject.Find("Player_Start_Point_Blue_1").transform.position;
        }
        else if (gameObject.tag == "Player2")
        {
            spawn = GameObject.Find("Player_Start_Point_Red_1").transform.position;
        }
        else if (gameObject.tag == "Player3")
        {
            spawn = GameObject.Find("Player_Start_Point_Blue_2").transform.position;
        }
        else if (gameObject.tag == "Player4")
        {
            spawn = GameObject.Find("Player_Start_Point_Red_2").transform.position;
        }
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }

}
