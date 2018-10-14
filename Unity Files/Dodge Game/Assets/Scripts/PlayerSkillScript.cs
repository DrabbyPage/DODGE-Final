using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerSkillScript : MonoBehaviour {

    bool canBeHit = true;
    public bool skillActive = false;

    bool rightFacing = true;

    public bool ableToThrow = false;
    public bool ableToPickUp = true;
    public bool ableToShield = true;
    bool canCheckDash = true;

    int shieldHealth = 1;
    int dashAmount = 1;

    int skillTeam;

    public GameObject shieldPrefab;
    public GameObject rechargeUI;

    public AudioClip strikerHit;
    AudioSource strikerHitSound;

    public AudioClip blockerHit;
    AudioSource blockerHitSound;

    public AudioClip strikerDash;
    AudioSource strikerDashSound;

    public AudioClip shieldHit;
    AudioSource shieldHitSound;

    string playerSkill = "LT";
    string playerAimHor = "RSH";
    string playerAimVer = "RSV";
    string playerHor = "LSH";
    string playerVer = "LSV";

    Animator anim;

    Rewired.Player player;

    // Use this for initialization
    void Start () {
        player = ReInput.players.GetPlayer(GetComponent<PlayerInfoScript>().playerNum);

        skillTeam = GetComponent<PlayerInfoScript>().infoTeam;

        shieldPrefab = Instantiate(shieldPrefab);
        shieldPrefab.transform.parent = gameObject.transform;
        shieldPrefab.transform.position = gameObject.transform.position;
        shieldPrefab.name = "Shield";
        shieldPrefab.SetActive(false);

        anim = GetComponent<Animator>();
        //anim.SetBool("Dashing", false);

        strikerHitSound = GetComponent<AudioSource>();
        blockerHitSound = GetComponent<AudioSource>();
        strikerDashSound = GetComponent<AudioSource>();
        shieldHitSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        player = ReInput.players.GetPlayer(GetComponent<PlayerInfoScript>().playerNum);

        rightFacing = GetComponent<PlayerMovement>().rightFacing;

        //anim.SetBool("Dashing", false);

        rechargeUI.SetActive(canCheckDash && ableToShield);

        if(GetComponent<PlayerInfoScript>().characterClass == 0)
        {
            Dash();
        }
        else if (GetComponent<PlayerInfoScript>().characterClass == 1)
        {
            CheckShield();
        }
    }


    void CheckShield()
    {
        if (shieldHealth > 0 && //Input.GetAxis(playerSkill) > 0
            player.GetAxis(playerSkill) > 0
            )
        {

            //Debug.Log("Shielding");
            //float xMag = Input.GetAxis(playerAimHor);
            //float yMag = Input.GetAxis(playerAimVer);
            //float xMag2 = Input.GetAxis(playerHor);
            //float yMag2 = Input.GetAxis(playerVer);


            float xMag = player.GetAxis(playerAimHor);
            float yMag = player.GetAxis(playerAimVer);
            float xMag2 = player.GetAxis(playerHor);
            float yMag2 = player.GetAxis(playerVer);

            // activate the shield
            shieldPrefab.SetActive(true);

            // putting the shield in place
            float spawnX = gameObject.transform.localPosition.x;
            float spawnY = gameObject.transform.localPosition.y;
            float spawnDist = 0.9f;

            shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 0);

            // positioning the shield and angling the shield
            if (Mathf.Abs(xMag) > 0 || Mathf.Abs(yMag) > 0)
            {
                // setting pos
                if (xMag > 0)
                {
                    spawnX = gameObject.transform.localPosition.x + spawnDist;
                }
                else if (xMag < 0)
                {
                    spawnX = gameObject.transform.localPosition.x - spawnDist;
                }

                if (yMag > 0)
                {
                    spawnY = gameObject.transform.localPosition.y + spawnDist;
                }
                else if (yMag < 0)
                {
                    spawnY = gameObject.transform.localPosition.y - spawnDist;
                }

                if (xMag == 0 && yMag == 0)
                {
                    if (rightFacing)
                        spawnX = gameObject.transform.localPosition.x + spawnDist;
                    else
                        spawnX = gameObject.transform.localPosition.x - spawnDist;
                }

                // setting the angle
                if (xMag > 0)
                {
                    shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 0);
                    if (yMag > 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 45);
                    }
                    else if (yMag < 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 315);
                    }

                }
                else if (xMag < 0)
                {
                    shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 180);
                    if (yMag > 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 135);
                    }
                    else if (yMag < 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 225);
                    }
                }
                else
                {
                    if (yMag > 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 90);
                    }
                    else if (yMag < 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 270);
                    }
                    else
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                }

            }
            else if (Mathf.Abs(xMag2) > 0 || Mathf.Abs(yMag2) > 0)
            {
                // seting the pos
                if (xMag2 > 0)
                {
                    spawnX = gameObject.transform.localPosition.x + spawnDist;
                }
                else if (xMag2 < 0)
                {
                    spawnX = gameObject.transform.localPosition.x - spawnDist;
                }

                if (yMag2 > 0)
                {
                    spawnY = gameObject.transform.localPosition.y + spawnDist;
                }
                else if (yMag2 < 0)
                {
                    spawnY = gameObject.transform.localPosition.y - spawnDist;
                }

                if (xMag2 == 0 && yMag2 == 0)
                {
                    spawnX = gameObject.transform.localPosition.x + spawnDist;
                }

                // seting the angle
                if (xMag2 > 0)
                {
                    shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 0);
                    if (yMag2 > 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 45);
                    }
                    else if (yMag2 < 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 315);
                    }

                }
                else if (xMag2 < 0)
                {
                    shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 180);
                    if (yMag2 > 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 135);
                    }
                    else if (yMag2 < 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 225);
                    }
                }
                else
                {
                    if (yMag2 > 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 90);
                    }
                    else if (yMag2 < 0)
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 270);
                    }
                    else
                    {
                        shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                }
            }
            else
            {
                if (rightFacing)
                {
                    spawnX = gameObject.transform.localPosition.x + spawnDist;
                    spawnY = gameObject.transform.localPosition.y;
                    shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    spawnX = gameObject.transform.localPosition.x - spawnDist;
                    spawnY = gameObject.transform.localPosition.y;
                    shieldPrefab.transform.eulerAngles = new Vector3(0, 0, 180);
                }
            }
            shieldPrefab.transform.position = new Vector2(spawnX, spawnY);

        }
        else
        {
            shieldPrefab.SetActive(false);
        }

        if (shieldHealth < 1)
        {
            StartCoroutine(AbleToShieldAgain());
        }
    }


    void Dash()
    {
        if (dashAmount > 0 && //Input.GetAxis(playerSkill) > 0
            player.GetAxis(playerSkill) > 0 
            )
        {
            strikerDashSound.PlayOneShot(strikerDash, 1f);

            GetComponent<PlayerMovement>().SetPlayerSpeed(4.0f);
            dashAmount--;
            anim.SetBool("Dashing", true);

            for (int i = 1; i < GameObject.FindGameObjectsWithTag("Ball").Length + 1; i++)
            {
                if (GameObject.Find("Ball" + i) != null)
                    Physics2D.IgnoreCollision(GameObject.Find("Ball" + i).GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), true);
            }

            canBeHit = false;
            StartCoroutine(NormalSpeed());
        }
        else
        {
            //anim.SetBool("Dashing", false);
        }

    }


    IEnumerator NormalSpeed()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<PlayerMovement>().SetPlayerSpeed(1.0f);
        canBeHit = true;

        for (int i = 1; i < GameObject.FindGameObjectsWithTag("Ball").Length + 1; i++)
        {
            if (GameObject.Find("Ball" + i) != null)
                Physics2D.IgnoreCollision(GameObject.Find("Ball" + i).GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), false);
        }
        //anim.SetBool("Dashing", false);
        StartCoroutine(AbleToDashAgain());

    }


    IEnumerator AbleToShieldAgain()
    {
        yield return new WaitForSeconds(2.0f);
        ableToShield = true;
        shieldHealth = 1;
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


    IEnumerator AbleToDashAgain()
    {
        yield return new WaitForSeconds(1.0f);
        dashAmount = 1;
        anim.SetBool("Dashing", false);

    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.otherCollider.transform.tag == "Shield")
        {
            if (col.transform.tag == "Ball" && col.transform.GetComponent<BallScript>().possession != 0 && 
                col.transform.GetComponent<BallScript>().possession != GetComponent<PlayerInfoScript>().infoTeam)
            {
                shieldHitSound.PlayOneShot(shieldHit, 1f);

                col.gameObject.GetComponent<BallScript>().ChangeTeam();
                col.gameObject.GetComponent<Rigidbody2D>().velocity *= -1;
                shieldHealth--;

            }
        }
        else
        {
            if (canBeHit && !GetComponent<PlayerInfoScript>().isOut)
            {
                if (col.transform.tag == "Ball" && col.transform.GetComponent<BallScript>().possession != 0 &&
                    col.transform.GetComponent<BallScript>().possession != GetComponent<PlayerInfoScript>().infoTeam)
                {
                    if (GetComponent<PlayerInfoScript>().characterClass == 0)
                    {
                        strikerHitSound.PlayOneShot(strikerHit, 1f);
                    }
                    else if (GetComponent<PlayerInfoScript>().characterClass == 1)
                    {
                        blockerHitSound.PlayOneShot(blockerHit, 1f);
                    }

                    if (col.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0)
                        GetComponent<SpriteRenderer>().flipX = true;
                    else
                        GetComponent<SpriteRenderer>().flipX = false;

                    GetComponent<PlayerInfoScript>().KillPlayer();

                    col.gameObject.GetComponent<BallScript>().SendKillInfo(gameObject);
                    Vector3 deathPoint = transform.position;

                }
            }
            else
            {
                if (col.gameObject.tag == "Ball")
                {
                    Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
                }

            }
        }
    }

    public void SetShieldHealth(int newHealth)
    {
        shieldHealth = newHealth;
    }

    public void SetDashAmount(int newAmount)
    {
        dashAmount = newAmount;
    }
}
