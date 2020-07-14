using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Rocket : MonoBehaviour
{
    [SerializeField]float rcsThrust = 500f;
    [SerializeField] float thrusterBoost = 100f;

    AudioSource thrustAudio;
    Rigidbody ridgeBody;
    // Start is called before the first frame update
    void Start()
    {
        ridgeBody = GetComponent<Rigidbody>();
        thrustAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotation();
        ResetRocket();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print( "OK");
                break;

            case "Fuel":
                print("OK");
                break;
            default:
                print("DEAD!");
                break;
        }
        /*foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
            audioSource.Play();*/
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ridgeBody.AddRelativeForce(Vector3.up * thrusterBoost);
            if (!thrustAudio.isPlaying)
            {
                thrustAudio.Play();
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ridgeBody.AddRelativeForce(-Vector3.up * thrusterBoost);
            thrustAudio.Stop();
        }

        else
        {
            thrustAudio.Stop();
        }

    }
    private void Rotation()
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
