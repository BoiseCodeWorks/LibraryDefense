using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.App.Camera
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [Header("Camera Configuration")]
        public Transform player;
        public Texture crosshair;

        protected Transform aimTarget;

        public float smoothingTime = 10.0f;
        [Tooltip("offset of point from player transform")]
        public Vector3 pivotOffset = new Vector3(0.2f, 0.7f, 0.0f);
        [Tooltip("offset of camera from pivotOffset")]
        public Vector3 camOffset = new Vector3(0.0f, 4.5f, -6f);
        [Tooltip("close offset of camera from pivotOffset")]
        public Vector3 closeOffset = new Vector3(0.35f, 1.7f, 0.0f);

        public float horizontalAimingSpeed = 200f;
        public float verticalAimingSpeed = 200f;
        public float maxVerticalAngle = 20f;
        public float minVerticalAngle = -30f;

        [Header("Mouse Options")]
        public float mouseSensitivity = 0.1f;
        [Tooltip("Press Escape to toggle in game")]
        public bool lockCursor = true;

        private float angleH = 0;
        private float angleV = 0;
        private Transform cam;
        private float maxCamDist = 3;
        private LayerMask mask;
        private Vector3 smoothPlayerPos;

        void Start()
        {
            // [edit] no aimtarget gameobject needs to be placed anymore - ben0bi
            GameObject g = new GameObject();
            aimTarget = g.transform;
            // Add player's own layer to mask
            mask = 1 << player.gameObject.layer;
            // Add Igbore Raycast layer to mask
            mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
            // Invert mask
            mask = ~mask;

            cam = transform;
            smoothPlayerPos = player.position;

            if (lockCursor)
            {
                LockCursor();
            }
        }

        void LateUpdate()
        {
            if (Time.deltaTime == 0 || Time.timeScale == 0 || player == null)
                return;
            // if you want to set up an xbox controller or something, you need to uncomment the 
            // Horizontal2 and Vertical2 Axes as well as configure them in Unity Input Manager
            // (unity->edit->Project Settings->input)
            // you can set up a new axis in the inspector by adding 2 to the number in the size property at the top.
            // this will add two new fields to the list of inputs both with the same name which you will want to rename to Horizontal2 and Vertical2
            // Configure Horizontal2 and set it up to use joystick 3rd axis
            angleH += Mathf.Clamp(Input.GetAxis("Mouse X") /* + Input.GetAxis("Horizontal2") */ , -1, 1) * horizontalAimingSpeed * Time.deltaTime;
            // Configure Vertical2 and set it up to use joystick 4th axis
            angleV += Mathf.Clamp(Input.GetAxis("Mouse Y") /* + Input.GetAxis("Vertical2") */ , -1, 1) * verticalAimingSpeed * Time.deltaTime;
            // limit vertical angle
            angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);

            AdjustCameraSmooth();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                lockCursor = !lockCursor;
                if (lockCursor)
                {
                    LockCursor();
                }
                else
                {
                    UnlockCursor();
                }
            }
        }

        private void AdjustCameraSmooth()
        {
            // Before changing camera, store the prev aiming distance.
            // If we're aiming at nothing (the sky), we'll keep this distance.
            float prevDist = (aimTarget.position - cam.position).magnitude;

            // Set aim rotation
            Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
            Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
            cam.rotation = aimRotation;

            // Find far and close position for the camera
            smoothPlayerPos = Vector3.Lerp(smoothPlayerPos, player.position, smoothingTime * Time.deltaTime);
            smoothPlayerPos.x = player.position.x;
            smoothPlayerPos.z = player.position.z;
            Vector3 farCamPoint = smoothPlayerPos + camYRotation * pivotOffset + aimRotation * camOffset;
            Vector3 closeCamPoint = player.position + camYRotation * closeOffset;
            float farDist = Vector3.Distance(farCamPoint, closeCamPoint);

            // Smoothly increase maxCamDist up to the distance of farDist
            maxCamDist = Mathf.Lerp(maxCamDist, farDist, 5 * Time.deltaTime);

            // Make sure camera doesn't intersect geometry
            // Move camera towards closeOffset if ray back towards camera position intersects something 
            RaycastHit hit;
            Vector3 closeToFarDir = (farCamPoint - closeCamPoint) / farDist;
            float padding = 0.3f;
            if (Physics.Raycast(closeCamPoint, closeToFarDir, out hit, maxCamDist + padding, mask))
            {
                maxCamDist = hit.distance - padding;
            }
            cam.position = closeCamPoint + closeToFarDir * maxCamDist;

            // Do a raycast from the camera to find the distance to the point we're aiming at.
            float aimTargetDist;
            if (Physics.Raycast(cam.position, cam.forward, out hit, 100, mask))
            {
                aimTargetDist = hit.distance + 0.05f;
            }
            else
            {
                // If we're aiming at nothing, keep prev dist but make it at least 5.
                aimTargetDist = Mathf.Max(5, prevDist);
            }

            // Set the aimTarget position according to the distance we found.
            // Make the movement slightly smooth.
            aimTarget.position = cam.position + cam.forward * aimTargetDist;
        }

        // so you can change the camera from a static observer (level loading) or something else
        // to your player or something else. I needed that for network init... ben0bi
        public void SetTarget(Transform t)
        {
            player = t;
        }

        // uncomment this if you want to have a crosshair - ben0bi

        void OnGUI()
        {
            if (crosshair == null) { return; }
            if (Time.time != 0 && Time.timeScale != 0)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - (crosshair.width * 0.5f), Screen.height / 2 - (crosshair.height * 0.5f), crosshair.width, crosshair.height), crosshair);
            }
        }

        void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}