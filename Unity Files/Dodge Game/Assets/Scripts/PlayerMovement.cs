using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour {

    float playerSpeed = 10f;
    float jumpForce = 800f;
	float throwSpeed = 100f;
    float speedMultiplier = 1.0f;

    public bool ableToThrow = false;
    public bool ableToPickUp = true;
    public bool ableToShield = true;
    public bool rightFacing = true;
    bool ableToJump = true;
    bool ableToWorm = false;

    public bool onGround;
	public LayerMask ground;
	float groundTimer = 0f;

    Rigidbody2D rig;

	public CircleCollider2D pickupRad;

    string ballSavedName;
	int numBalls = 0;
	int maxBalls = 1;

	public GameObject ballPrefab;
    public GameObject shieldPrefab;
    public GameObject cursorPrefab;
	public GameObject explosionPrefab;

	Color color;

	Animator anim;
	SpriteRenderer sr;


	enum State{
		IDLE = 0, JUMPING, RUNNING	
	};

	public int characterClass;

	public GameObject rechargeUI;

    public AudioClip throwBall;
    AudioSource throwBallSound;

    bool aDown = false;

    Rewired.Player pmPlayer;

    // the name for the player's controls
    public string playerHor = "LSH";
    public string playerVer = "LSV";
    public string playerJump = "A";
    public string playerPickup = "X";
    public string playerAimHor = "RSH";
    public string playerAimVer = "RSV";
    public string playerThrow = "RT";
    public string playerShield = "LT";

    // Use this for initialization
    void Start () {
        characterClass = GameObject.Find("GameManager").GetComponent<ManagerScript>().GetPlayerClass(gameObject.name);

        if (GetComponent<PlayerInfoScript>().ableToSpawn) {

            pmPlayer = ReInput.players.GetPlayer(GetComponent<PlayerInfoScript>().playerNum);

            cursorPrefab = Instantiate (cursorPrefab);
			shieldPrefab = Instantiate (shieldPrefab);
			cursorPrefab.transform.parent = gameObject.transform;
			shieldPrefab.transform.parent = gameObject.transform;
			cursorPrefab.transform.position = gameObject.transform.position;
			shieldPrefab.transform.position = gameObject.transform.position;
			cursorPrefab.name = "Cursor";
			shieldPrefab.name = "Shield";
			shieldPrefab.SetActive (false);

            GetComponent<PlayerInfoScript>().ballUI.SetActive(false);

			rig = GetComponent<Rigidbody2D> ();
			onGround = false;
			color = GetComponent<SpriteRenderer> ().color;

            anim = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
            anim.SetInteger("CharacterClass", characterClass);

            throwBallSound = GetComponent<AudioSource>();

            if (characterClass == 1) {
				gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (1.22f, 1.44f);
				gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (-0.089f, -0.28f);
			}
		}
    }


    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PlayerInfoScript>().isOut)// && !GetComponent<ManagerScript>().endofRound)
        {
            gameObject.SetActive(true);

            CheckGrounded();
            CheckPickup();
            CheckMove();

            CheckForWorm();

            if(pmPlayer.GetAxis(playerShield) < 1)
            {
                CheckThrow();
            }
        }
    }

    public void DecreaseBall()
    {
        numBalls--;
    }

    void CheckMove()
    {
        float xMove = pmPlayer.GetAxis(playerHor);
        float yMove = pmPlayer.GetAxis(playerJump);

        rig.velocity = new Vector2(xMove * playerSpeed * speedMultiplier, rig.velocity.y);

        if (xMove > 0)
        {
			sr.flipX = false;
            rightFacing = true;
        }
		if (xMove < 0)
        {
			sr.flipX = true;
            rightFacing = false;
        }


		if (yMove != 0 && onGround && !aDown)
		{
			onGround = false;
			groundTimer = 0.1f;

			anim.SetInteger ("State", (int)State.JUMPING);

			rig.AddForce (Vector2.up * 1.25f * jumpForce);
		}
		else if (xMove != 0 && onGround)
		{
			anim.SetInteger ("State", (int)State.RUNNING);
		}
		else if (onGround)
		{
			anim.SetInteger ("State", (int)State.IDLE);
		}

		aDown = yMove != 0;

    }

	void CheckGrounded()
	{
		if (groundTimer == 0f)
		{
			RaycastHit2D rcLeft, rcRight, rcCenter;

            rcLeft = Physics2D.Raycast (new Vector2 (transform.position.x - 0.5f, transform.position.y - 1.05f), Vector2.down, 0.05f, ground);
			rcRight = Physics2D.Raycast (new Vector2 (transform.position.x + 0.5f, transform.position.y - 1.05f), Vector2.down, 0.05f, ground);
			rcCenter = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y - 1.05f), Vector2.down, 0.05f, ground);


            if (rcLeft.transform != null || rcRight.transform != null || rcCenter.transform != null)
            {
                onGround = true;
                anim.SetInteger("State", (int)State.IDLE);
            }
            else
			{
				onGround = false;
				if (anim.GetInteger("State") == (int)State.RUNNING)
				{
					anim.SetInteger ("State", (int)State.IDLE);
				}
			}
		}
		else
		{
            anim.SetInteger("State", (int)State.JUMPING);
            groundTimer -= Time.deltaTime;
			if (groundTimer <= 0f)
			{
				groundTimer = 0f;
			}
        }
    }

	void CheckPickup()
	{
		if (pmPlayer.GetButton(GetComponent<PlayerInfoScript>().playerPickup) && ableToPickUp)
        {
			ableToPickUp = false;
			ableToThrow = false;

			Collider2D[] hits = Physics2D.OverlapCircleAll (pickupRad.bounds.center, pickupRad.radius, LayerMask.GetMask ("Ball"));
			for (int i = 0; i < hits.GetLength (0) && numBalls < maxBalls; i++) {

				ballSavedName = hits [i].gameObject.name;
                GetComponent<PlayerInfoScript>().SetBallName(hits[i].gameObject.name);

				numBalls++;
                GetComponent<PlayerInfoScript>().IncreaseBalls();

				Destroy (hits [i].gameObject);
				anim.SetBool ("Catching", true);
                anim.SetBool("HoldingBall", true);

                GetComponent<PlayerInfoScript>().ballUI.SetActive(true);

			}
		}
        else if (!pmPlayer.GetButton(playerPickup) && !ableToPickUp)
        {
			StartCoroutine(AbleToPickUpAgain());
		}

	}

    void SetCursor()
    {
        float spawnX;
        float spawnY;

        if(Mathf.Abs(pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerAimHor)) > 0 ||
            Mathf.Abs(pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerAimVer)) > 0 )
        {
            spawnX = pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerAimHor);
            spawnY = pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerAimVer);
        }
        else
        {
            spawnX = pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerHor);
            spawnY = pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerVer);
        }

        cursorPrefab.transform.position = new Vector2(gameObject.transform.position.x + spawnX, gameObject.transform.position.y + spawnY);
    }

	void CheckThrow()
	{

        if (pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerThrow) > 0 && ableToThrow && numBalls > 0)
		{

            throwBallSound.PlayOneShot(throwBall, 1f);
			anim.SetBool ("Throwing", true);
            GetComponent<PlayerInfoScript>().ballUI.SetActive(false);

            float xMag = pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerAimHor);
            float yMag = pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerAimVer);
            float xMag2 = pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerHor);
            float yMag2 = pmPlayer.GetAxis(GetComponent<PlayerInfoScript>().playerVer);

            GameObject ball = Instantiate (ballPrefab) as GameObject;

            float spawnX = gameObject.transform.position.x;
            float spawnY = gameObject.transform.position.y;

            if (xMag2 > 0)
            {
                spawnX = gameObject.transform.position.x + 1.0f;
            }
            else if (xMag2 < 0)
            {
                spawnX = gameObject.transform.position.x - 1.0f;
            }

            if (yMag2 > 0)
            {
                spawnY = gameObject.transform.position.y + 1.0f;
            }
            else if (yMag2 < 0)
            {
                spawnY = gameObject.transform.position.y - 1.0f;
            }

            if (xMag > 0)
            {
                sr.flipX = false;
                rightFacing = true;
                
                spawnX = gameObject.transform.position.x + 1.0f;
            }
            else if(xMag < 0)
            {
                sr.flipX = true; 
                rightFacing = false;

                spawnX = gameObject.transform.position.x - 1.0f;

            }

            if (yMag > 0)
            {
                spawnY = gameObject.transform.position.y + 1.0f;
            }
            else if (yMag < 0)
            {
                spawnY = gameObject.transform.position.y - 1.0f;

            }

            ball.transform.position = new Vector2(spawnX, spawnY);
            ball.name = ballSavedName;
            ball.GetComponent<BallScript>().possession = GetComponent<PlayerInfoScript>().infoTeam;
			ball.GetComponent<BallScript>().UpdateColor();
            ball.GetComponent<BallScript>().SetThrower(gameObject);

            anim.SetBool("HoldingBall", false);

            if (xMag != 0 || yMag != 0)
                ball.GetComponent<Rigidbody2D>().velocity = new Vector2(throwSpeed * xMag * 0.5f, throwSpeed * yMag * 0.5f);
            else if(xMag2 != 0 || yMag2 != 0)
                ball.GetComponent<Rigidbody2D>().velocity = new Vector2(throwSpeed * xMag2 * 0.5f, throwSpeed * yMag2 * 0.5f);
            else
            {
                if(rightFacing)
                    ball.GetComponent<Rigidbody2D>().velocity = new Vector2(throwSpeed * 1.0f * 0.5f, throwSpeed * yMag2 * 0.5f);
                else
                    ball.GetComponent<Rigidbody2D>().velocity = new Vector2(throwSpeed * -1.0f * 0.5f, throwSpeed * yMag2 * 0.5f);
            }


            numBalls--;
            GetComponent<PlayerInfoScript>().DecreaseBalls();
            ableToThrow = false;
		}

        if(numBalls > 0)
            StartCoroutine(AbleToShootAgain());

    }

    IEnumerator AbleToShootAgain()
    {
        yield return new WaitForSeconds(0.3f);
        ableToThrow = true;
    }

    IEnumerator AbleToPickUpAgain()
    {
        yield return new WaitForSeconds(0.3f);
        ableToPickUp = true;
    }

    public void SetPlayerSpeed(float newSpeed)
    {
        speedMultiplier = newSpeed;
    }

    void CheckForWorm()
    {
        if(GameObject.Find("GameManager").GetComponent<ManagerScript>().endofRound)
        {
            string wormButton = "Y";

            if(pmPlayer.GetButton(wormButton))
            {
                anim.SetBool("Worm", true);
            }
        }
        else
        {
            anim.SetBool("Worm", false);
        }
    }

}
