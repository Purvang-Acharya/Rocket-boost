using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody m_RocketShip;
    AudioSource m_ThrustSound;
    [SerializeField]
    float rcsThrust = 100f;

    [SerializeField]
    float mainThrust = 100f;

    enum State { Alive, Dying , Transcending };
    State state = State.Alive;
    
    void Start()
    {
        m_RocketShip = GetComponent<Rigidbody>();
        m_ThrustSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;


            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene", 1f);                
                break;


            default:
                state = State.Dying;
                Invoke("LoadFirstScene", 1f);                
                break;



        }
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
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
