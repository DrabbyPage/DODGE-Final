using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour {
    
    // the manager script will be used to check if all players are out
    // and reset if on team is out
    public int gameRound = 1;
    public int maxRounds = 7;
    public static int playerAmount;// = 0;

    public int team1Score = 0;
    public int team2Score = 0;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    bool p1CanSpawn = true;
    bool p2CanSpawn = true;
    bool p3CanSpawn = true;
    bool p4CanSpawn = true;

    int player1Kills;
    int player2Kills;
    int player3Kills;
    int player4Kills;

    int player1Deaths;
    int player2Deaths;
    int player3Deaths;
    int player4Deaths;

    public int player1Class;
    public int player2Class;
    public int player3Class;
    public int player4Class;

    public List<GameObject> team1Players = new List<GameObject>();
    public int sizeOfTeam1 = 0;

    public List<GameObject> team2Players = new List<GameObject>();
    public int sizeOfTeam2 = 0;

    List<GameObject> ballList = new List<GameObject>();
    int sizeOfBalls = 0;

    bool canCheck = true;

    bool onLevel = false;
    public bool endofRound = false;

    public static GameObject instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = gameObject;
        }


        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }


        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {

        CheckPlayer();

    }

    // Update is called once per frame
    void Update () {

        //CheckForStartOfGame();

        CheckPlayer();

        if (SceneManager.GetActiveScene().name == "End_Scene")
        {
            canCheck = false;
            //Debug.Log("setting player stats");
            //Debug.Log("Player1 kills: " + player1Kills);
            GameObject.Find("End_Manager").GetComponent<EndSceneUIData>().SetKills(player1Kills, player2Kills, player3Kills, player4Kills);
            GameObject.Find("End_Manager").GetComponent<EndSceneUIData>().SetDeaths(player1Deaths, player2Deaths, player3Deaths, player4Deaths);

            if (team1Score > team2Score)
            {
                GameObject.Find("End_Manager").GetComponent<EndSceneUIData>().SetWinningText("Blue Team Wins " + team1Score + " to " + team2Score + "!");
            }
            else
            {
                GameObject.Find("End_Manager").GetComponent<EndSceneUIData>().SetWinningText("Red Team Wins " + team2Score + " to " + team1Score + "!");
            }
        }

        if(SceneManager.GetActiveScene().name == "Start_Menu")
        {
            MenuSettings();
        }

        if (canCheck)
        {
            CheckTeamOne();
            CheckTeamTwo();
        }

        if(GetComponent<UIManager>())
        {
        GetComponent<UIManager>().SetBlueTeamNum(team1Score);
        GetComponent<UIManager>().SetRedTeamNum(team2Score);
        GetComponent<UIManager>().SetRoundNum(gameRound);
        }

    }

    void CheckForStartOfGame()
    {
        if(endofRound == true)
        {
            StartCoroutine(StartCountDown());
        }
    }

    void MenuSettings()
    {
        maxRounds = 0;
        team1Score = 0;
        team2Score = 0;
        gameRound = 1;

        canCheck = true;

        onLevel = false;
        endofRound = false;

        player1 = null;
        player2 = null;
        player3 = null;
        player4 = null;

        player1Kills = 0;
        player2Kills = 0;
        player3Kills = 0;
        player4Kills = 0;

        player1Deaths = 0;
        player2Deaths = 0;
        player3Deaths = 0;
        player4Deaths = 0;

        player1Class = 0;
        player2Class = 0;
        player3Class = 0;
        player4Class = 0;

        team1Players.Clear();
        sizeOfTeam1 = 0;

        team2Players.Clear();
        sizeOfTeam2 = 0;

        ballList.Clear();
        sizeOfBalls = 0;
    }

    public void SetPlayerAmount(int numPlayers)
    {
        playerAmount = numPlayers;
    }

    public void SetPlayerAbleToSpawn(string playerTag, bool ableTo)
    {
        if(playerTag == "Player1")
        {
            p1CanSpawn = ableTo;
        }
        else if (playerTag == "Player2")
        {
            p2CanSpawn = ableTo;

        }
        else if (playerTag == "Player3")
        {
            p3CanSpawn = ableTo;

        }
        else if (playerTag == "Player4")
        {
            p4CanSpawn = ableTo;

        }
    }

    public bool GetPlayerAbilityToSpawn(string playerTag)
    {
        if (playerTag == "Player1")
        {
            return p1CanSpawn;
        }
        else if (playerTag == "Player2")
        {
            return p2CanSpawn;
        }
        else if (playerTag == "Player3")
        {
            return p3CanSpawn;
        }
        else if (playerTag == "Player4")
        {
            return p4CanSpawn;
        }
        else
            return true;
    }

    public int GetNumPlayers()
    {

        return playerAmount;
    }

    void CheckPlayer()
    {
        if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Test_Level_2" || 
            SceneManager.GetActiveScene().name == "Test_Level_3" || SceneManager.GetActiveScene().name == "Test_Level_4")
        {
            player1 = GameObject.FindGameObjectWithTag("Player1");
            player2 = GameObject.FindGameObjectWithTag("Player2");
            player3 = GameObject.FindGameObjectWithTag("Player3");
            player4 = GameObject.FindGameObjectWithTag("Player4");

            if (GetComponent<UIManager>() && !endofRound )
            {
                GetComponent<UIManager>().EnableRoundEndText("");
            }

            if (!onLevel)
            {
                SetTeams();
                onLevel = true;
                StartCoroutine(StartCountDown());

            }
        }
    }

    public void SetPlayerClass(string player, int playerClass)
    {
        if (player == "Player1")
        {
            player1Class = playerClass;
            //Debug.Log("Set Player1: " + player1Class);
        }

        if (player == "Player2")
        {
            player2Class = playerClass;
            //Debug.Log("Set Player2: " + player2Class);
        }

        if (player == "Player3")
        {
            player3Class = playerClass;
            //Debug.Log("Set Player3: " + player3Class);
        }

        if (player == "Player4")
        {
            player4Class = playerClass;
            //Debug.Log("Set Player4: " + player4Class);
        }
    }

    public int GetPlayerClass(string tag)
    {
        if (tag == "Player1")
        {
            return player1Class;
        }

        else if (tag == "Player2")
        {
            return player2Class;
        }

        else if(tag == "Player3")
        {
            return player3Class;
        }

        else if(tag == "Player4")
        {
            return player4Class;
        }

        else
        {
            return 0;
        }
    }

    public void SetMaxRounds(int newMax)
    {
        maxRounds = newMax;
    }

    void SetTeams()
    {
        if (p1CanSpawn)
        {
            team1Players.Add(player1);
            sizeOfTeam1++;
        }


        if (p2CanSpawn)
        {
            team2Players.Add(player2);
            sizeOfTeam2++;
        }


        if (p3CanSpawn)
        {
            team1Players.Add(player3);
            sizeOfTeam1++;
        }


        if (p4CanSpawn)
        {
            team2Players.Add(player4);
            sizeOfTeam2++;
        }

    }

    void CheckTeamOne()
    {
        int teamMemebersOut = 0;
        int totalTeamMembers = sizeOfTeam1;

        for (int i = 0; i < totalTeamMembers; i++)
        {
            if (team1Players[i].GetComponent<PlayerInfoScript>().isOut)
                teamMemebersOut++;

            if(teamMemebersOut >= totalTeamMembers)
            {
                // appear congratulations to the winning team, increase score, reset
                canCheck = false;
                endofRound = true;
                //Debug.Log("Team2 Wins Round " + gameRound + "!");

                if (GetComponent<UIManager>())
                {
                    GetComponent<UIManager>().EnableRoundEndText("Team2 Wins Round " + gameRound + "!");
                }

                team2Score++;
                //Debug.Log("Team 1 score: " + team1Score + " vs " + "Team 2 score: " + team2Score);
                StartCoroutine(WaitInBetweenMatch());
            }
        }
    }

    void CheckTeamTwo()
    {
        int teamMemebersOut = 0;
        int totalTeamMembers = sizeOfTeam2;

        for (int i = 0; i < totalTeamMembers; i++)
        {
            if (team2Players[i].GetComponent<PlayerInfoScript>().isOut || !team1Players[i].GetComponent<PlayerInfoScript>().ableToSpawn)
                teamMemebersOut++;

            if (teamMemebersOut >= totalTeamMembers)
            {
                // appear congratulations to the winning team, increase score, reset
                canCheck = false;
                endofRound = true;

                //Debug.Log("Team1 Wins Round " + gameRound + "!");

                if (GetComponent<UIManager>())
                {
                    GetComponent<UIManager>().EnableRoundEndText("Blue Team Wins Round " + gameRound + "!");
                }

                team1Score++;
                //Debug.Log("Team 1 score: " + team1Score + " vs " + "Team 2 score: " + team2Score);
                StartCoroutine(WaitInBetweenMatch());
            }
        }
    }

    IEnumerator WaitInBetweenMatch()
    {
        yield return new WaitForSeconds(3.5f);

        if (GetComponent<UIManager>())
        {
            GetComponent<UIManager>().DisableRoundEndText();
        }

        ResetMatch();
    }

    void ResetMatch()
    {
		GameObject.Find ("Buzzer").GetComponent<AudioSource> ().Play ();

		if (team1Score > (maxRounds / 2)) 
		{
			ResetGame ();
		} 
		else if (team2Score > (maxRounds / 2)) 
		{
			ResetGame ();
		}
        else
        {
            for (int i = 0; i < sizeOfTeam1; i++)
            {
                // set the positions
                team1Players[i].GetComponent<PlayerInfoScript>().ResetPlayer();
            }

            for (int i = 0; i < sizeOfTeam2; i++)
            {
                team2Players[i].GetComponent<PlayerInfoScript>().ResetPlayer();
            }

            //endofRound = false;
            canCheck = true;
            gameRound++;

            int amountOfBalls = 4;

            for (int i = 1; i <= amountOfBalls; i++)
            {
                GameObject.Find("Ball" + i).GetComponent<BallScript>().ResetPos();
            }

            StartCoroutine(StartCountDown());
        }


    }

    void ResetGame()
    {
        if(team1Score > team2Score)
        {
            //Debug.Log("Team1 Wins " + team1Score + " to " + team2Score + "!");

            if (GetComponent<UIManager>())
            {
                GetComponent<UIManager>().EnableRoundEndText("Team1 Wins " + team1Score + " to " + team2Score + "!");
            }

        }
        else
        {
            //Debug.Log("Team2 Wins " + team2Score + " to " + team1Score + "!");

            if (GetComponent<UIManager>())
            {
                GetComponent<UIManager>().EnableRoundEndText("Team2 Wins " + team2Score + " to " + team1Score + "!");
            }
        }

        StartCoroutine(GoBackToMenu());
    }

    IEnumerator GoBackToMenu()
    {
        yield return new WaitForSeconds(4);
        // go back to the menu

        SceneManager.LoadScene("End_Scene");

    }

    public void IncrementPlayerKills(string player)
    {
        if(player == "Player1")
        {
            player1Kills++;
        }
        else if (player == "Player2")
        {
            player2Kills++;
        }
        else if (player == "Player3")
        {
            player3Kills++;
        }
        else if (player == "Player4")
        {
            player4Kills++;
        }
    }

    public void IncrementPlayerDeaths(string player)
    {
        if (player == "Player1")
        {
            player1Deaths++;
        }
        else if (player == "Player2")
        {
            player2Deaths++;

        }
        else if (player == "Player3")
        {
            player3Deaths++;
        }
        else if (player == "Player4")
        {
            player4Deaths++;
        }
    }

    IEnumerator StartCountDown()
    {
        yield return new WaitForSeconds(1.0f);
        GetComponent<UIManager>().EnableRoundEndText("3");

        yield return new WaitForSeconds(1.0f);
        GetComponent<UIManager>().EnableRoundEndText("2");

        yield return new WaitForSeconds(1.0f);
        GetComponent<UIManager>().EnableRoundEndText("1");

        yield return new WaitForSeconds(1.0f);
        GetComponent<UIManager>().EnableRoundEndText("GO");

        endofRound = false;
    }
}

