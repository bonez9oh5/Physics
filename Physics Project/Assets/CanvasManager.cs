using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public Slider heightSlider;
    public Slider powerSlider;
    public Button shootButton;
    public GameManager gm;
    public Text heightText;
    public Text powerText;
    public GolfBall ball;

	// Use this for initialization
	void Start () {
        
        if (!gm)
        {
            Debug.LogError("Game Manager not linked to canvas manager");
        }
	}
	
	// Update is called once per frame
	void Update () {


        if (ball.putting == true)
        {
            gm.height = 0;
            gm.strokePower = powerSlider.value;
        }
        else if (ball.inBunker == true)
        {
            gm.height = heightSlider.value / 2;
            gm.strokePower = powerSlider.value / 2;
        }
        else
        {

            gm.height = heightSlider.value;

            gm.strokePower = powerSlider.value;

        }
        

        
    }
   
}
