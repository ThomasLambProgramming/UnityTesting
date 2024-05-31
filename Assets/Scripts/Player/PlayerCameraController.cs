﻿using System;
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

        //It over rotates when first loading in, possibly due to delta time being large or input values being wrong on setup.
        [SerializeField] private float rotateDelayAtStart = 1.0f;
        private float rotateDelayTimer = 0;

        //Where do we want the camera to follow to.
        [SerializeField] private Transform playerCameraFollowSpot;
        private void Start()
        {
            mainCamera = Camera.main;
        }

        public void UpdateCamera(Vector2 playerInput)
        {
            if (rotateDelayTimer < rotateDelayAtStart)
            {
                rotateDelayTimer += Time.deltaTime;
                return;
            }

            cameraRotateObject.position = playerCameraFollowSpot.position;
            cameraRotateObject.transform.Rotate(
                playerInput.y * rotateSpeedX * Time.deltaTime,
                playerInput.x * rotateSpeedY * Time.deltaTime,
                0);
            mainCamera.transform.LookAt(cameraRotateObject.position);

            Vector3 eular = cameraRotateObject.transform.rotation.eulerAngles;

            if (eular.x > higherXRotationLimit && eular.x <= 180)
                eular.x = higherXRotationLimit;
            else if (eular.x < lowerXRotationLimit && eular.x > 180)
                eular.x = lowerXRotationLimit;
            
            cameraRotateObject.transform.rotation = Quaternion.Euler(eular.x, eular.y, 0); 
        }

        public void ToggleControllerSpeed()
        {
            if (rotateSpeedX > 50)
            {
                rotateSpeedX = Mathf.Sqrt(rotateSpeedX);
                rotateSpeedY = Mathf.Sqrt(rotateSpeedY);
            }
            else
            {
                rotateSpeedX *= rotateSpeedX;
                rotateSpeedY *= rotateSpeedY;
            }
        }
    }
}