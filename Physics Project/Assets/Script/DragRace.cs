using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragRace : MonoBehaviour
{
    private Rigidbody m_rb = null;
    private bool m_isStarted = false;
    private float m_timeElapsed = 0.0f;

    public float zeroToHundredTime;
    public float maxSpeed;

    
    [SerializeField]
    Text speedometer;
    float currentSpeed;
    


    //calculate me
    public float calcAccel(float initVel, float finalVel, float time)
    {
        return (time > Mathf.Epsilon) ? ((finalVel - initVel) / time) : 0.0f;

    }
    public float m_accelerationSpeed = 0.0f;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_accelerationSpeed = calcAccel(0, 100, zeroToHundredTime);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_isStarted = true;
        }
    }

    void FixedUpdate()
    {
        currentSpeed = m_rb.velocity.z;
        speedometer.text = "Km/h: " + (int)currentSpeed;
        //if started we want to accelerate the object
        if (m_isStarted)
        {
            if (m_rb.velocity.z < maxSpeed)
            {

                Vector3 acceleration = transform.forward * m_accelerationSpeed;
                m_rb.AddForce(acceleration, ForceMode.Acceleration);
                m_timeElapsed += Time.fixedDeltaTime;

            }
            else
            {
                m_rb.velocity = new Vector3(0, 0, maxSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        m_isStarted = false;
        m_rb.velocity = Vector3.zero;
        

        Debug.Log("Velocity: " + m_rb.velocity);
        Debug.Log("Time Elapsed: " + m_timeElapsed);
    }
}
