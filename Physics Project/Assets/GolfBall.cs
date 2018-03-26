using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GolfBall : MonoBehaviour {


    public GameManager gm;
    Rigidbody rb;
    [SerializeField]
    public Terrain ground;
    public bool isGrounded = false;
    public bool putting;
    public bool fireBall;
    Transform lastPosition ;
    Transform currentPosition;
    [SerializeField]
    GameObject[] particleEffects;
    [SerializeField]
    GameObject shootButton;
    public bool inBunker;
    [SerializeField]
    Transform target;
    [SerializeField]
    GameObject[] targets;
    int targetsHit;
    [SerializeField]
    float timeToTarget;
    
    TextMeshPro ballText;
    
	// Use this for initialization
	void Start () {
        ballText = GetComponentInChildren<TextMeshPro>();
        currentPosition = transform;
        lastPosition = currentPosition;
        gm = FindObjectOfType<GameManager>();

#region EXCEPTION_CHECKS
        if (!gm)
        {
            Debug.LogError("No game manager linked to ball.");
        }
        rb = GetComponent<Rigidbody>();
        if(!rb)
        {
            Debug.LogError("No Rigidbody on ball.");
        }
        if(!ballText)
        {
            Debug.LogError("No text linked to ball.");
        }
        if (!ground)
        {
            Debug.LogError("Ball not tracking terrain");
        }
#endregion
    }
	
	// Update is called once per frame
	void Update () {

        if (fireBall == true)
        {
            particleEffects[2].SetActive(true);
        }
        else
        {
            particleEffects[2].SetActive(false);
        }

        if (rb.velocity.x==0)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
            shootButton.SetActive(true);
        }
        else if (rb.velocity.x>0||rb.velocity.x<0)
        {
            shootButton.SetActive(false);
        }
       
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HoleDetection")
        {
            ballText.text="ball in, it took " + gm.strokesToHole + "strokes!";
            gm.height = 0;
            gm.strokePower = 0;
            //transition to next scene/hole 
            //if holesPlayed == 18 finish game and display stats
        }
        
        else if (other.gameObject.name == "WaterHazard")
        {
            //splash
            particleEffects[0].SetActive(true);
            Invoke("StopEffects", 5f);
            //move ball to last location and add a stroke
            HazardDrop();
            ++gm.strokesToHole;
            ballText.text = "Drop! +1 Stroke";
            Invoke("ClearBallText", 2);
            gm.height = 0;
            gm.strokePower = 0;
        }
        else if (other.gameObject.name == "GreenDetection")
        {
            gm.height = 0;
            gm.strokePower = 0;
            //enable putting only
            putting = true;
            ballText.text = "On Green! Good Shot!";
            Invoke("ClearBallText", 2);
        }
        else if (other.gameObject.name == "BunkerDetection")
        {
            //kick up dirt
            particleEffects[1].SetActive(true);
            Invoke("StopEffects", 5f);
            gm.height = 0;
            gm.strokePower = 0;

            //apply bunker physics
            inBunker = true;
            ballText.text = "Playing in the sand?";
            Invoke("ClearBallText", 2);


        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name== "BunkerDetection")
        {
            inBunker = false;
        }
        else if(other.gameObject.name == "GreenDetection")
        {
            putting = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Terrain" )
        {
            gm.height = 0;
            gm.strokePower = 0;
            isGrounded = true;
            
            
            
        }
        else if (collision.gameObject.name == "FireballPowerUp")
        {
            fireBall = true;
            ballText.text = "FIRE!";
            Invoke("ClearBallText", 2);
        }
    }

    public void ApplyForce()
    {
        lastPosition = transform;
        if (fireBall == true)
        {
            particleEffects[2].SetActive(true);
        }
        Vector3 direction = target.position - transform.position;
        
        rb.AddForce(transform.forward * gm.strokePower);
        rb.AddForce(transform.up * gm.height);
    }

    void StopEffects()
    {
        for (int i = 0; i < particleEffects.Length; i++)
        {
            if (particleEffects[i].activeSelf==true)
            {
                particleEffects[i].SetActive(false);
            }
        }
    }

    void HazardDrop()
    {
        transform.position = lastPosition.position;
    }
    
    void ClearBallText()
    {
        ballText.text = "";
    }
}
