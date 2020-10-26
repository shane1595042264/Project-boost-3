﻿
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocket : MonoBehaviour
{
     [SerializeField]float rcsThrust = 100f;
    [SerializeField]float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
        Rigidbody rigidBody;
    AudioSource audioSource;
   
   enum State { Alive, Dying, Transcending }
   State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
    if(state == State.Alive)
    
    {
        RespondToThrustInput();
        
        RespondToRotateInput();
    }
    }

    
    void OnCollisionEnter(Collision collision){
        if(state != State.Alive){
            return;
        }
        switch (collision.gameObject.tag){
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
private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        Invoke("LoadNextScene", 1f);
    }
    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        Invoke("LoadFirstLevel", 1f);
    }

    

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToRotateInput()
    {   
        rigidBody.freezeRotation = true;
       
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            transform.Rotate(-Vector3.forward* rotationThisFrame);
        }
        rigidBody.freezeRotation = false;
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
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }

       
    }
}
