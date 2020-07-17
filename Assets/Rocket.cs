using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 500f;
    [SerializeField] float thrusterBoost = 300f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip levelStart;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    AudioSource audioComponent;
    Rigidbody ridgeBody;

    enum State {Alive, Dying, Transcending}

    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        ridgeBody = GetComponent<Rigidbody>();
        audioComponent = GetComponent<AudioSource>();
        audioComponent.PlayOneShot(levelStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            //todo stop show on death
            RespondToThrustInput();
            RespondToRotateInput();
        }
        
        ResetRocket();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) {return;}

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                StartDeathSequence();
                break;
        }
        /*foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
            audioSource.Play();*/
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioComponent.Stop();
        deathParticles.Play();
        audioComponent.PlayOneShot(death);
        Invoke("LoadFirstScene", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioComponent.Stop();
        audioComponent.PlayOneShot(levelStart);
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay); //parameterise time
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ridgeBody.AddRelativeForce(-Vector3.up * thrusterBoost);
            audioComponent.Stop();
        }

        else
        {
            audioComponent.Stop();
            mainEngineParticles.Stop();
        }

    }

    private void ApplyThrust()
    {
        ridgeBody.AddRelativeForce(Vector3.up * thrusterBoost*Time.deltaTime);
        if (!audioComponent.isPlaying)
        {
            audioComponent.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        ridgeBody.freezeRotation = true; // take manual control of rotation

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {

            print("Rotate Left");
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            print("Rotate Right");
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        ridgeBody.freezeRotation = false; // remove manual control of rotation

    }

    private void ResetRocket()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Quaternion zeroRotation = new Quaternion(0, 0, 0, 0);
            Vector3 zeroOut = new Vector3(-35, 2.5f, 0);
            print("You reset the position");
            transform.SetPositionAndRotation(zeroOut, zeroRotation);
            ridgeBody.velocity = Vector3.zero;
            ridgeBody.angularVelocity = Vector3.zero;
        }
    }
}
