using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ballController : MonoBehaviour {

    public GameObject restartButton;
    public Text jumpCounterText, MiddleText;
    public float fireForce;
    public Rigidbody2D rb;

    public GameObject center;

    private bool escaping;
    private bool stuck;
    private int jumpCounter;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!escaping && coll.CompareTag("FirePad"))
        {
            Debug.Log("Entering FirePad");
            this.transform.parent = coll.gameObject.transform;
            this.GetComponent<Rigidbody2D>().isKinematic = true;
            Quaternion rotation = Quaternion.identity;
            rotation.x = 0.0f;
            rotation.y = 0.0f;
            rotation.z = 0.0f;
            transform.localRotation = rotation;
            stuck = true;
        }
        else if (coll.CompareTag("Exit"))
        {
            Debug.Log("Entering Exit");
            Destroy(this.gameObject);
            MiddleText.text = "You Made It!!";
            restartButton.SetActive(true);
        }
        else if (coll.CompareTag("OuterBounds"))
        {
            Debug.Log("Out of bounds");
            Destroy(this.gameObject);
            MiddleText.text = "Game Over";
            restartButton.SetActive(true);

        }
        else if (coll.CompareTag("Trigger"))
        {
            Debug.Log("Hitted Trigger");
            coll.GetComponent<Trigger>().toggleTrigger();
        }

        Debug.Log(coll.gameObject.tag.ToString());
    }

    void OnTriggerExit2D()
    {
        escaping = false;
        stuck = false;
    }

	// Use this for initialization
	void Start () {
        rb.GetComponent<Rigidbody2D>();
        escaping = false;
        stuck = false;
        jumpCounter = 20;
        jumpCounterText.text = "" + jumpCounter;
        restartButton.SetActive(false);

    }   
	
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space) && stuck == true && jumpCounter > 0)
        {
            Debug.Log("Fire was pressed");
            rb.isKinematic = false;
            escaping = true;
            jumpCounter -= 1;
            jumpCounterText.text = "" + jumpCounter;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
	    if(escaping)
        {
            
            rb.AddRelativeForce(new Vector2(0, fireForce), ForceMode2D.Impulse);
  
        }
	}
}
