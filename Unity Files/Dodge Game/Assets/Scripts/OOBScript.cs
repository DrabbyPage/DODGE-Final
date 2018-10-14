using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOBScript : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ball")
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            col.gameObject.GetComponent<BallScript>().ResetPos();
        }
        if(col.gameObject.layer == 8)
        {
            col.gameObject.GetComponent<PlayerInfoScript>().ResetPos();
        }
    }
}
