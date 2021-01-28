using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody m_RocketShip;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip levelFinishedSuccess;
    [SerializeField] AudioClip levelFinishedDeath;

    //[SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem levelFinishedSuccessParticles;
    [SerializeField] ParticleSystem levelFinishedDeathParticles;

    enum State { Alive, Dying , Transcending };
    State state = State.Alive;
    
    void Start()
    {
        m_RocketShip = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
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
                StartSuccessSequence();
                break;


            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(levelFinishedDeath);
        levelFinishedDeathParticles.Play();
        Invoke("LoadFirstScene", 1f);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelFinishedSuccess);
        levelFinishedSuccessParticles.Play();
        Invoke("LoadNextScene", 1f);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
           // mainEngineParticles.Stop();
            
        }
    }

    private void ApplyThrust()
    {
        
        m_RocketShip.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
       // mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
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
