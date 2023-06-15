using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// These Assemblies gives access to MoreMountain's Scripts
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

// THIS SCRIPT MOVES THE CUBE AROUND TO SEE OTHER FACES

public class RotateBigCube : MonoBehaviour
{
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    private Vector3 previousMousePosition;
    private Vector3 mouseDelta;
    public GameObject target;
    [SerializeField]private float speed;
    
    [Header("Feedbacks Player")]
    [Tooltip("AUDIO>Cube Sfx should be added here to make the Sfx work")]
    // use MoreMountain's scripting to add the MMFPlayer to script
    [SerializeField] private MMF_Player WhooshSfx;

    // Update is called once per frame
    void Update()
    {
        Swipe();
        Drag();
        OnGUI();
    }

    void Drag()
    {
        if (Input.GetMouseButton(1))
        {
            // While the mouse is held down the cube can be move around its central axis to provide visual feedback
            mouseDelta = Input.mousePosition - previousMousePosition;
            mouseDelta *= 0.1f; // reduction of rotation speed
            transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
        }
        else
        {
            // automatically move to the target position
            if (transform.rotation != target.transform.rotation)
            {
                var step = speed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
            }
        }
        
        previousMousePosition = Input.mousePosition;
    }

    void Swipe()
    {
        if(Input.GetMouseButtonDown(1))
        {
            // get the 2D position of the first mouse click
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if(Input.GetMouseButtonUp(1))
        {
            // get the 2D position of the second mouse click
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create a vector from the first and second click positions
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2D vector
            currentSwipe.Normalize();

            switch (currentSwipe)
            {
                case Vector2 swipe when LeftSwipe(swipe):
                    target.transform.Rotate(0, swipe.x < 0 ? 90 : -90, 0, Space.World);
                    // This calls the PlaySfx when the player rotates a face of the cube.
                    PlaySfx();
                    break;
                case Vector2 swipe when RightSwipe(swipe):
                    // This calls the PlaySfx when the player rotates a face of the cube.
                    PlaySfx();
                    target.transform.Rotate(0, swipe.x < 0 ? 90 : -90, 0, Space.World);
                    break;
                case Vector2 swipe when UpLeftSwipe(swipe):
                    // This calls the PlaySfx when the player rotates a face of the cube.
                    PlaySfx();
                    target.transform.Rotate(90, 0, 0, Space.World);
                    break;
                case Vector2 swipe when UpRightSwipe(swipe):
                    // This calls the PlaySfx when the player rotates a face of the cube.
                    PlaySfx();
                    target.transform.Rotate(0, 0, -90, Space.World);
                    break;
                case Vector2 swipe when DownLeftSwipe(swipe):
                    // This calls the PlaySfx when the player rotates a face of the cube.
                    PlaySfx();
                    target.transform.Rotate(0, 0, 90, Space.World);
                    break;
                case Vector2 swipe when DownRightSwipe(swipe):
                    // This calls the PlaySfx when the player rotates a face of the cube.
                    PlaySfx();
                    target.transform.Rotate(-90, 0, 0, Space.World);
                    break;
            } // Developer RA Note: We might need to refactor this switch statement since nag copy paste ako ng maraming beses for the playSfx(); nakakatamad hahahha
        }
    }
    
    public void PlaySfx()
    {
        WhooshSfx = GameObject.Find("ViewMovement Sfx").GetComponent<MMF_Player>();
        WhooshSfx.Initialization();
        WhooshSfx.PlayFeedbacks(); 
    }


    void OnGUI()
    {
        Event e = Event.current;
         
        if (e != null && e.isKey && e.type == EventType.KeyDown)
        {
            switch (e.keyCode)
            {
                case KeyCode.LeftArrow:
                    target.transform.Rotate(0, 90, 0, Space.World);
                    break;
                case KeyCode.RightArrow:
                    target.transform.Rotate(0, -90, 0, Space.World);
                    break;
                case KeyCode.UpArrow:
                    target.transform.Rotate(0, 0, -90, Space.World);
                    break;
                case KeyCode.DownArrow:
                    target.transform.Rotate(0, 0, 90, Space.World);
                    break;
                default:
                    break;
            }
            
        }
    }

    public void UpKey()
    {
        target.transform.Rotate(0, 0, -90, Space.World);
    }

    public void DownKey()
    {
        target.transform.Rotate(0, 0, 90, Space.World);
    }

    public void LeftKey()
    {
        target.transform.Rotate(0, 90, 0, Space.World);
    }

    public void RightKey()
    {
        target.transform.Rotate(0, -90, 0, Space.World);
    }

    bool LeftSwipe(Vector2 swipe)
    {
        return currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    bool RightSwipe(Vector2 swipe)
    {
        return currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    bool UpLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x < 0f;
    }

    bool UpRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x > 0f;
    }

    bool DownLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x < 0f;
    }

    bool DownRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x > 0f;
    }
}

