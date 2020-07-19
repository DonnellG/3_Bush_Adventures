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

    enum State {Alive, Dying, Transcending, Godmode}

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
        if (state == State.Alive || state == State.Godmode)
        {
            //todo stop show on death
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondtoDebugKeys();
            ResetRocket();
        }


    }

    private void RespondtoDebugKeys()
    {
        LoadNextLevelKey();
        EnableGodeMode();
    }

    private void EnableGodeMode()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (state == State.Godmode)
            {
                state = State.Alive;
                print("you are MORTAL");
            }
            else
            {
                state = State.Godmode;
                print("you are a GOD");
            }
        }
    }

    private void LoadNextLevelKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive && state != State.Godmode) {return;}

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                if (state == State.Godmode) { return; }
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
            SceneManager.LoadScene(nextSceneIndex);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }

        else
        {
            StopApplyingThrust();
        }

    }

    private void StopApplyingThrust()
    {
        audioComponent.Stop();
        mainEngineParticles.Stop();
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
        ridgeBody.angularVelocity = Vector3.zero;

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
