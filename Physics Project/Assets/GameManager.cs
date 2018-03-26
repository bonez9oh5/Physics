using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public CanvasManager cm;
    public GolfBall bs;

    public GameObject golfball;
    GameObject currentBall;
    public GameObject hitPoint;
    public GameObject club;
    public float strokePower;
    public float height;
    public bool onGreen;
    public int strokesToHole;
    int holesPlayed;


	// Use this for initialization
	void Start () {
        holesPlayed++;
        strokesToHole = 0;
        strokePower = 0.0f;
        height = 0.0f;
        onGreen = false;

      
        if(!bs)
        {
            Debug.LogError("BallScript not linked to Game Manager");
        }

        if (!golfball)
        {
            Debug.LogError("Ball not linked in inspector; gm");
        }

        
        if (!cm)
        {
            Debug.LogError("Canvas Manager not linked to Game Manager");
        }

}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void HitBall()
    {
        if (bs.isGrounded == true)
        {
            if (!golfball)
            {
                Debug.LogError("golfball not linked");
            }
            else
            {

                //play swing animation
                //club should have a trigger to call apply force.
                bs.ApplyForce();
                ++strokesToHole;
            }
        }
    }
    
}
