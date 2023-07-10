﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gives access to MoreMountain's Scripts
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
public class PivotRotation : MonoBehaviour
{
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;
    private bool dragging = false;
    public bool rotating = false;
    private float sensitivity = 0.4f;
    private float speed = 800f;
    private Vector3 rotation;
    private Quaternion targetQuaternion;
    private ReadCube readCube;
    private CubeState cubeState;
    private Automate automate;
    private InGameUI ui;
    
    
    [Header("Feedbacks Player")]
    [Tooltip("AUDIO>Cube Sfx should be added here to make the Sfx work")]
    [SerializeField] private MMF_Player Sfx;
       
    // Start is called before the first frame update
    private void Start()
    {
        ui = FindObjectOfType<InGameUI>();
        automate = FindObjectOfType<Automate>();
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
        
        // readies the sfx for use
        Sfx = GameObject.Find("CubeMovement").GetComponent<MMF_Player>();
        Sfx.Initialization();
    }

    // Late Update is called once per frame at the end
    private void LateUpdate()
    {
        if (dragging && !rotating && !automate.shuffling && !ui.isPaused && !CubeState.autoRotating)
        {
            SpinSide(activeSide);
            PlaySfx();
            if (Input.GetMouseButtonUp(0))
            {
                // Debug.Log("Second Click");
                dragging = false;
                RotateToRightAngle();
                readCube.ReadState();
            }
        }
        if (rotating)
        {
            AutoRotate();
            PlaySfx();
        }
    }

    private void Update()
    {
        if(automate.shuffling)
        {
            speed = 600f;
        }
        else
        {
            speed = 800f;
        }
    }

    private void SpinSide(List<GameObject> side)
    {
        // reset the rotation
        rotation = Vector3.zero;
        // current mouse position minus the last mouse position
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);        
        if (side == cubeState.up)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.down)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.left)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.right)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.front)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.back)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        // rotate
        transform.Rotate(rotation, Space.Self);

        // store mouse
        mouseRef = Input.mousePosition;
    }

         
    public void Rotate(List<GameObject> side)
    {
        activeSide = side;
        mouseRef = Input.mousePosition;
        dragging = true;
        // Create a vector to rotate around
        localForward = Vector3.zero - side[4].transform.localPosition;
    }

    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        cubeState.PickUp(side);
        Vector3 localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
        activeSide = side;
        rotating = true;
    }


    public void RotateToRightAngle()
    {
        Vector3 vec = transform.localEulerAngles;
        // round vec to nearest 90 degrees
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        targetQuaternion.eulerAngles = vec;
        rotating = true;
    }

    private void AutoRotate()
    {
        dragging = false;
        var step = speed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

        // if within one degree, set angle to target angle and end the rotation
        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
        {
            transform.localRotation = targetQuaternion;

            // Debug statements to check activeSide count and pivot
            // Debug.Log("activeSide count: " + activeSide.Count);
            // Debug.Log("Pivot: " + transform.parent);

            // unparent the little cubes
            cubeState.PutDown(activeSide, transform.parent);
            readCube.ReadState();
            CubeState.autoRotating = false;
            rotating = false;
            dragging = false;                                                               
        }
    }     
    
    
    #region Audio Methods
    public void PlaySfx()
    {
        Sfx.PlayFeedbacks();
    }
    #endregion
}
