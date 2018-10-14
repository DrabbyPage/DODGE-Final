using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    Vector2 spawn;

	public int possession = 0;

	Rigidbody2D rb;

	float speedThreshold = 20f;
    float maxBounces = 4f; // always do a plus one because it collides with player 

    GameObject thrower;
    string possessorName;

	public GameObject ballTrailPrefab;
	float trailLifespan = 0.1f;
	float trailSpawnCountdownMax = 0.01f;
	float trailSpawnCountdown = 0.01f;

	AudioSource collisionSound;
	GameObject lastCollisionObject = null;
	float collisionCooldown = 0.1f;


	// Use this for initialization
	void Start () {
		collisionSound = GetComponent<AudioSource> ();

        thrower = null;
        SetSpawn();
        rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		//CheckForDeadBall ();
		SpawnTrail ();

		collisionCooldown -= Time.deltaTime;
		if (collisionCooldown <= 0f) {
			lastCollisionObject = null;
			collisionCooldown = 0f;
		}
	}

	void SpawnTrail()
	{
		trailSpawnCountdown -= Time.deltaTime;
		if (trailSpawnCountdown <= 0f)
		{
			trailSpawnCountdown = trailSpawnCountdownMax;
			GameObject trail = Instantiate (ballTrailPrefab);
			trail.transform.position = transform.position;
			trail.GetComponent<BallTrailScript> ().maxLifespan = trailLifespan;
			trail.GetComponent<SpriteRenderer> ().color = GetComponent<SpriteRenderer> ().color;
		}
	}

    public void ChangeTeam()
    {
        if(possession == 1){
            possession = 2;
        }
        if(possession == 2){
            possession = 1;
        }
        else{
            possession = 0;
        }
        UpdateColor();
    }

    void SetSpawn()
    {
        if (name == "Ball1")
            spawn = GameObject.Find("Ball_Start_Point_0").transform.position;
        else if (name == "Ball2")
            spawn = GameObject.Find("Ball_Start_Point_1").transform.position;
        else if (name == "Ball3")
            spawn = GameObject.Find("Ball_Start_Point_2").transform.position;
        else if (name == "Ball4")
            spawn = GameObject.Find("Ball_Start_Point_3").transform.position;
    }

    public void ResetPos()
    {
        SetSpawn();
        thrower = null;
        gameObject.transform.position = spawn;
        possession = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    void CheckForDeadBall()
	{

        if(rb.velocity.x > 25)
        {
            rb.velocity.Set(25, 0);
        }
        if(rb.velocity.x < -25)
        {
            rb.velocity.Set(-25, 0);
        }
		float speed = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y);

        
		if (speed < speedThreshold)
		{
			possession = 0;
			UpdateColor ();
		}
	}

	public void UpdateColor()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();

		if (possession == 0)
		{
			sr.color = Color.white;
		}
		else if (possession == 1)
		{
			sr.color = Color.blue;
		}
		else if (possession == 2)
		{
			sr.color = Color.red;
		}
	}

    public void SetPossessorName(string newName)
    {
        possessorName = newName;
    }

    public void SetThrower(GameObject player)
    {
        thrower = player;
        possessorName = player.name;
        possession = thrower.GetComponent<PlayerInfoScript>().infoTeam;
        maxBounces = 3;
    }

    public void SendKillInfo(GameObject deadObj)
    {
        GameObject GameMan = GameObject.Find("GameManager");

        if(GameMan.GetComponent<ManagerScript>())
        {
            //Debug.Log(deadObj.name + " is dead");
            //Debug.Log(possessorName + " got a kill");
            GameMan.GetComponent<ManagerScript>().IncrementPlayerDeaths(deadObj.name);
            GameMan.GetComponent<ManagerScript>().IncrementPlayerKills(possessorName);
        }
    }


	void OnCollisionEnter2D(Collision2D col)
	{

		if (col.gameObject.layer == LayerMask.NameToLayer("Ground") && lastCollisionObject != col.gameObject) {
			
			collisionSound.Play();

			lastCollisionObject = col.gameObject;
			collisionCooldown = 0.1f;
		}

        if (col.gameObject.tag == "Floor")
        {
            maxBounces = maxBounces - 1;

            if (maxBounces <= 0)
            {
                possession = 0;
            }

            UpdateColor();
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            //maxBounces = maxBounces - 1;

            if(maxBounces <= 0)
            {
               possession = 0;
            }
        }

        UpdateColor();
    }
}
