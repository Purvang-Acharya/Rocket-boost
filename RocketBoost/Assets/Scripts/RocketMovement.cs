using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody m_RocketShip;
    AudioSource m_ThrustSound;
    [SerializeField]
    float rcsThrust = 100f;
    
    [SerializeField]
    float mainThrust = 100f;
    
    void Start()
    {
        m_RocketShip = GetComponent<Rigidbody>();
        m_ThrustSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            

        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            m_RocketShip.AddRelativeForce(Vector3.up * mainThrust);
            if (!m_ThrustSound.isPlaying)
            {
                m_ThrustSound.Play();
            }
        }
        else
        {
            m_ThrustSound.Stop();
        }
    }

    private void Rotate()
    {
        m_RocketShip.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        m_RocketShip.freezeRotation = false;
    }
}
