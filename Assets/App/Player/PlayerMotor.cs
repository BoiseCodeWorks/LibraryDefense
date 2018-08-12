using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    CharacterController controller;

    public float WalkSpeed = 5;
    public float RunSpeed = 10;
    public float JumpForce = 5;
    public Camera CamTransform;

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
        var dir = Vector3.zero;
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
        float yAngle = (CamTransform.transform.rotation.y / 180) * 100;
        //Debug.Log(yAngle);
        transform.localRotation = new Quaternion(0, yAngle, 0, 0);
        return dir.normalized * input.magnitude;
    }
}
