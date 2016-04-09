using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class mapController : MonoBehaviour {

    private GameObject[] mapFoundations;

    void objectRotation()
    {

    }

	// Use this for initialization
	void Start () {

        //Find all "cages" and "planets" and put them into the foundation list
        mapFoundations = GameObject.FindGameObjectsWithTag("mapBodies");

	}
	
	// Update is called once per frame
	void Update () {

        float rawInput = Input.GetAxis("Horizontal");

        foreach (GameObject obj in mapFoundations)
        {
            Body objBody = obj.GetComponent<Body>();
            if (!objBody.getSelfRotating())
            { 
                obj.GetComponent<Rigidbody2D>().MoveRotation(obj.GetComponent<Rigidbody2D>().rotation + rawInput * Time.fixedDeltaTime * objBody.getRotationSpeed() * objBody.getRotationDirection());
            }
            else if (objBody.isRotating())
            {
                obj.GetComponent<Rigidbody2D>().MoveRotation(obj.GetComponent<Rigidbody2D>().rotation + Time.fixedDeltaTime * objBody.getRotationSpeed() * objBody.getRotationDirection());
            }
        }
	}
}
