using UnityEngine;
using System.Collections;

public class Gravity_script : MonoBehaviour {

    public GameObject entity;
    public float StrengthOfAttraction;

    void OnTriggerEnter2D(Collider2D coll)
    {
        coll.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        coll.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    // Use this for initialization
    void Start () {
        entity = null;
        entity = GameObject.FindGameObjectWithTag("Ball");
        if (entity != null)
            Debug.Log("Ball found");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        entity = null;
        entity = GameObject.FindGameObjectWithTag("Ball");

        if (entity != null)
        {
            //magsqr will be the offset squared between the object and the planet
            float magsqr;

            //offset is the distance to the planet
            Vector3 offset;

            //get offset between each planet and the player
            offset = entity.transform.position - transform.position;

            //My game is 2D, so  I set the offset on the Z axis to 0
            offset.z = 0;

            //Offset Squared:
            magsqr = offset.sqrMagnitude;

            //Check distance is more than 0 to prevent division by 0
            if (magsqr > 0.0001f)
            {
                //Create the gravity- make it realistic through division by the "magsqr" variable

                entity.GetComponent<Rigidbody2D>().AddForce(-(StrengthOfAttraction * offset.normalized / magsqr) * entity.GetComponent<Rigidbody2D>().mass);
            }
        }
    }
}
