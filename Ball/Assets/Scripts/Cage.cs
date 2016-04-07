using UnityEngine;
using System.Collections;

public class Cage : MonoBehaviour {

    public float rotation_speed;
    public Rigidbody2D rb;


    private int lastRotationDirection, rotationDirection;
    

	// Use this for initialization
	void Start () {
        lastRotationDirection = 0;
        rb.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        float rawInput = Input.GetAxis("Horizontal");

        rb.MoveRotation(rb.rotation + rawInput * Time.fixedDeltaTime * rotation_speed);

    }
}
