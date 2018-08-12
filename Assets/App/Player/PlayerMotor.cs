using Assets.App.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    CharacterController controller;

    public float WalkSpeed = 5;
    public float RunSpeed = 10;
    public float JumpForce = 5;
    public ThirdPersonCameraController CamTransform;

    Vector3 dir = Vector3.zero;

    public float ForwardSpeed
    {
        get
        {
            return Mathf.Abs(Input.GetAxis("Vertical"));
        }
    }
    public float HorizontalSpeed
    {
        get
        {
            return Mathf.Abs(Input.GetAxis("Horizontal"));
        }
    }

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerInput();
    }

    void GetPlayerInput()
    {
        dir = Vector3.zero;
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        var walking = Input.GetKey(KeyCode.LeftShift);

        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }
        dir = RotateWithView(dir);

        controller.Move(
            dir * Time.deltaTime *
            (walking ? WalkSpeed : RunSpeed)
            );
    }

    private Vector3 RotateWithView(Vector3 input)
    {
        Vector3 dir = CamTransform.transform.TransformDirection(input);
        dir.Set(dir.x, 0, dir.z);
        //float yAngle = Mathf.Abs(CamTransform.transform.rotation.y / 180) * 100;
        transform.localRotation = Quaternion.FromToRotation(transform.position, new Vector3(CamTransform.HAngle,0,0));


        return dir.normalized * input.magnitude;
    }
}
