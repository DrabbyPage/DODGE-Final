using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Floor" || col.gameObject.tag == "Platform")
        {
            //gameObject.GetComponentInParent<PlayerMovement>().Grounded();

        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Floor" || col.gameObject.tag == "Platform")
        {
            //gameObject.GetComponentInParent<PlayerMovement>().NotGrounded();

        }

    }
}
