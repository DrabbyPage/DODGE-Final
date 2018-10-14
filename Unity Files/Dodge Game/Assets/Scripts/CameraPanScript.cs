using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanScript : MonoBehaviour {

	float minX; float maxX; float minY; float maxY;
	List<GameObject> players = new List<GameObject> ();
	public GameObject manager;
	public Vector2 cameraBuffer;

	float camWidth ;
	float camHeight;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager");
		//players = new List<GameObject> ();

		
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePlayers ();
		CalculateFrame ();
		CalculatePosition ();
		
	}

	void UpdatePlayers(){
		if (manager.GetComponent<ManagerScript> ().team2Players != null) 
		{
			for (int i = 1; i <= 4; i++) {
				if (GameObject.Find ("Player" + i)) {
					players.Add (GameObject.Find ("Player" + i));
				}
			}

			//players = manager.GetComponent<ManagerScript> ().team1Players;
			//players.AddRange (manager.GetComponent<ManagerScript> ().team2Players);


			Debug.Log (players.Count);

		}
			
	}

	void CalculateFrame () {
		minX = Mathf.Infinity;
		maxX = -Mathf.Infinity;
		minY = Mathf.Infinity;
		maxY = -Mathf.Infinity;


		foreach (GameObject player in players){
			Vector3 tempPlayer = player.transform.position;
			if (player.GetComponent<PlayerInfoScript> ().isOut == false) {
				if (tempPlayer.x < minX) {
					minX = tempPlayer.x;
				}
				if (tempPlayer.x > maxX) {
					maxX = tempPlayer.x;
				}
				if (tempPlayer.y < minY) {
					minY = tempPlayer.y;
				}
				if (tempPlayer.y > maxY) {
					maxY = tempPlayer.y;
				}
			}
		}
		
	}


	void CalculatePosition () {
	
		Vector3 center = Vector3.zero;
		Vector3 finalPos;

		foreach (GameObject player in players) {
			if (player.GetComponent<PlayerInfoScript>().isOut == false){
				center += player.transform.position;
			}

		}
		finalPos = center / players.Count;

		float sizeX = maxX - minX + cameraBuffer.x;
		float sizeY = maxY - minY + cameraBuffer.y;
		float windowSize = (sizeX > sizeY ? sizeX : sizeY);


		//Checking Var Limits (width)
		if (windowSize < 8) {
			windowSize = 8;
		} else if (windowSize > 18) {
			windowSize = 18;
		}

		Camera cam = GetComponent<Camera>();

		cam.orthographicSize = (windowSize + cam.orthographicSize)/2f;
		camHeight = cam.orthographicSize * 2f;
		camWidth = camHeight * cam.aspect;

		Debug.Log (finalPos.y - camHeight/4);

		//Checking Var Limits (pos)
		if (finalPos.x + camWidth/10 > 21) {
			finalPos.x = 21 - camWidth/10;
		} else if (finalPos.x - camWidth/10 < -21) {
			finalPos.x = -21 + camWidth/10;
		}

		if (finalPos.y - camHeight/4 < 0) {
			finalPos.y = 0 + camHeight/4;
		}




		//Debug.Log (finalPos);

		gameObject.transform.position = Vector3.Lerp (transform.position, new Vector3(finalPos.x, finalPos.y, transform.position.z), 0.5f);

	

	}


}
