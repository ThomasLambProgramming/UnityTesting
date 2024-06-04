using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Separate Camera controller to manage when the player can move the camera themselves or if it is during a cutscene.
    /// </summary>
    public class PlayerCameraController : MonoBehaviour
    {
        public Camera mainCamera;
        
        //We want to rotate the camera around the player from far away so making a pivot point is easier.
        [SerializeField] private Transform cameraRotateObject;
        [SerializeField] private float rotateSpeedX;
        [SerializeField] private float rotateSpeedY;
        [SerializeField] private float lowerXRotationLimit = -30f;
        [SerializeField] private float higherXRotationLimit = 30f;
        [SerializeField] private float positionFollowSpeed = 0.125f;

        //It over rotates when first loading in, possibly due to delta time being large or input values being wrong on setup.
        [SerializeField] private float rotateDelayAtStart = 1.0f;
        private float rotateDelayTimer = 0;

        //Where do we want the camera to follow to.
        [SerializeField] private Transform playerCameraFollowSpot;


        [SerializeField] private bool stopTrackingPlayer = false;
        private void Start()
        {
            mainCamera = Camera.main;
        }

        public void UpdateCamera(Vector2 playerInput, bool fromController)
        {
            if (rotateDelayTimer < rotateDelayAtStart)
            {
                rotateDelayTimer += Time.deltaTime;
                return;
            }

            if (stopTrackingPlayer)
                return;

            Vector3 goalPosition = Vector3.Lerp(cameraRotateObject.position, playerCameraFollowSpot.position, positionFollowSpeed * Time.deltaTime);
            cameraRotateObject.position = goalPosition;

            Quaternion cameraRotationObjectRot = cameraRotateObject.transform.rotation;
            Vector3 newRotation = cameraRotationObjectRot.eulerAngles + new Vector3(
                                playerInput.y * (fromController ? rotateSpeedX * rotateSpeedX : rotateSpeedX) * Time.deltaTime,
                                playerInput.x * (fromController ? rotateSpeedY * rotateSpeedY : rotateSpeedY) * Time.deltaTime,
                                0);

            if (newRotation.x > higherXRotationLimit && newRotation.x <= 180)
                newRotation.x = higherXRotationLimit;
            else if (newRotation.x < lowerXRotationLimit && newRotation.x > 180)
                newRotation.x = lowerXRotationLimit;
            
            cameraRotateObject.transform.rotation = Quaternion.Euler(newRotation.x, newRotation.y, 0); 
            mainCamera.transform.LookAt(cameraRotateObject.position);
        }
    }
}