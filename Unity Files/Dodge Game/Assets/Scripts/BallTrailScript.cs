using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallTrailScript : MonoBehaviour {

	public float maxLifespan;
	float lifespan;

	SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		lifespan = maxLifespan;
	}
	
	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, lifespan / maxLifespan / 2f);
		if (lifespan <= 0f)
			Destroy (gameObject);
	}
}
