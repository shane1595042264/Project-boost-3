﻿
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocket : MonoBehaviour
{
     [SerializeField]float rcsThrust = 100f;
    [SerializeField]float mainThrust = 100f;
    [SerializeField] float levelLoadDelay =  2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;
        Rigidbody rigidBody;
    AudioSource audioSource;
   
   enum State { Alive, Dying, Transcending }
   State state = State.Alive;
    // Start is called before the first frame update
    bool collisionsDisabled = false;
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
    RespondToDebugKeys();
    }

    private void RespondToDebugKeys(){
        if (Input.GetKeyDown(KeyCode.L)){
            LoadNextScene();
        }
        else if(Input.GetKeyDown(KeyCode.C)){
            collisionsDisabled = !collisionsDisabled;
        }

    }

    
    void OnCollisionEnter(Collision collision){
        if(state != State.Alive || collisionsDisabled){
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
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }
    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
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
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }

       mainEngineParticles.Play();
    }
}
