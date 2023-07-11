using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    private CubeState cubeState;
    private ReadCube readCube;
    private PivotRotation pivotRotation;
    private InGameUI ui;
    private bool faceSelecting = false;
    private int layerMask = 1 << 0;

    
    // Start is called before the first frame update
    private void Start()
    {
        pivotRotation = FindObjectOfType<PivotRotation>();
        ui = FindObjectOfType<InGameUI>();
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!faceSelecting && Input.GetMouseButtonDown(0) && !CubeState.autoRotating && !ui.isPaused && !pivotRotation.rotating)
        {
            // read the current state of the cube
            // Debug.Log("Select click!");            
            readCube.ReadState();

            // Set faceSelecting to true to prevent multiple selections
            faceSelecting = true;

            // raycast from the mouse towards the cube to see if a face is hit  
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                // Debug.Log("Ray Hit!");
                GameObject face = hit.collider.gameObject;
                // Make a list of all the sides (lists of face GameObjects)
                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState.front,
                    cubeState.back,
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right
                };
                // If the face hit exists within a side
                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    if (cubeSide.Contains(face))
                    {
                        Debug.Log(face);
                        // Pick it up
                        cubeState.PickUp(cubeSide);
                        // Start the side rotation logic
                        cubeSide[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);
                    }
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            faceSelecting = false;
        }
    }
}