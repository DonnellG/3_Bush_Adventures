using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Rocket : MonoBehaviour
{
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

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ridgeBody.AddRelativeForce(Vector3.up);
            if (!thrustAudio.isPlaying)
            {
                thrustAudio.Play();
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            print("you are hiting D");
            ridgeBody.AddRelativeForce(-Vector3.up);
            thrustAudio.Stop();
        }

        else
        {
            thrustAudio.Stop();
        }

    }
    private void Rotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            print("Rotate Left");
            transform.Rotate(Vector3.forward);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            print("Rotate Right");
            transform.Rotate(-Vector3.forward);
        }
    }

    private void ResetRocket()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Quaternion zeroRotation = new Quaternion(0, 0, 0, 0);
            Vector3 zeroOut = new Vector3(0, 2.5f, 0);
            print("You reset the position");
            transform.SetPositionAndRotation(zeroOut, zeroRotation);
            ridgeBody.velocity = Vector3.zero;
            ridgeBody.angularVelocity = Vector3.zero;
        }
    }
}
