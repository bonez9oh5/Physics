using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GolfBall : MonoBehaviour {


    
    [SerializeField]
    Terrain ground;
    [SerializeField]
    GameObject[] particleEffects;
    [SerializeField]
    GameObject shootButton;
    [SerializeField]
    GameObject target;
    [SerializeField]
    GameObject[] targets;
    [SerializeField]
    Slider powerSlider;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text strokesText;

    [SerializeField]
    float timeToTarget;
    

    public GameManager gm;
    Rigidbody rb;

    int targetsHit = 0;
    int strokesRemaining = 10;
    int score = 0;
    float angle = 45.0f;
    bool inBunker;
    public bool isGrounded = false;
    public bool putting;
    public bool fireBall;
    Transform lastPosition;
    Transform currentPosition;


    TextMeshPro ballText;
    
    

    // Use this for initialization
    void Start () {
        ballText = GetComponentInChildren<TextMeshPro>();
        currentPosition = transform;
        lastPosition = currentPosition;
        gm = FindObjectOfType<GameManager>();
        targets = GameObject.FindGameObjectsWithTag("Target");
        target = targets[0];

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

        scoreText.text = "Score: " + score;
        strokesText.text = "Strokes: " + strokesRemaining;

        if (powerSlider.value >= 0.9f || powerSlider.value <=1.1f)
        {
            timeToTarget = 1;
        }
        else
        {
            timeToTarget = powerSlider.value;
        }


        if (targetsHit == 3 && strokesRemaining > 0)
        {
            target = null;
            //win display score
            ballText.text = "Win! Points: " + score;
            //gm.height = 0;
            //gm.strokePower = 0;
            Invoke("ClearBallText", 2);
            LoadScene("Title");
              
        }
        else if (targetsHit < 3 && strokesRemaining == 0)
        {
            target = null;
            //game over display score
            ballText.text = "Loose! Points: " + score;
            Invoke("ClearBallText", 2);
            LoadScene("Title");
        }

        if (fireBall == true)
        {
            particleEffects[2].SetActive(true);
        }
        else
        {
            particleEffects[2].SetActive(false);
        }

        if (rb.velocity.x==0 && target)
        {
            //transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
            transform.LookAt(target.transform.position);
            shootButton.SetActive(true);
        }
        else if (rb.velocity.x>0||rb.velocity.x<0 || !target)
        {
            shootButton.SetActive(false);
        }
       
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.name == "HoleDetection" && targetsHit == 2)
        //{
        //    ballText.text="ball in, it took " + gm.strokesToHole + "strokes!";
        //    gm.height = 0;
        //    gm.strokePower = 0;
        //    Invoke("ClearBallText", 2);
        //    LoadScene("Title");
        //    //transition to next scene/hole 
        //    //if holesPlayed == 18 finish game and display stats
        //}


        
        if (other.gameObject.name == "WaterHazard")
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
            //putting = true;
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
        else if (collision.gameObject.tag == "Target")
        {
            fireBall = true;
            ballText.text = "FIRE!";
            Invoke("ClearBallText", 2);
            targetsHit += 1;
            target = targets[targetsHit];
            //switch (targetsHit)
            //{
            //    case 0 : target = targets[0];
            //        break;
            //    case 1: target = targets[1];
            //        break;
            //    case 2: target = targets[2];
            //        break;
            //}
            score += 10;
        }
    }

    public void ApplyForce()
    {
        lastPosition = transform;

        
        
        if (fireBall == true)
        {
            particleEffects[2].SetActive(true);
        }
        Vector3 direction = target.transform.position - transform.position;
        float heightDiff = direction.y;
        direction.y = 0;
        float distance = Mathf.Abs(direction.magnitude);
        float a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += heightDiff / Mathf.Tan(a);
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        rb.velocity = (velocity * direction.normalized) * timeToTarget;

        
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

    void LoadScene(string level)
    {
        SceneManager.LoadScene(level);
    }
}
