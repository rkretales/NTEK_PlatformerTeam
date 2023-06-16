using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMap : MonoBehaviour
{
    CubeState cubeState;

    public Transform mFront;
    public Transform mBack;
    public Transform mUp;
    public Transform mDown;
    public Transform mLeft;
    public Transform mRight;

    public void Set()
    {
        cubeState = FindObjectOfType<CubeState>();

        UpdateMap(cubeState.front, mFront);
        UpdateMap(cubeState.back, mBack);
        UpdateMap(cubeState.up, mUp);
        UpdateMap(cubeState.down, mDown);
        UpdateMap(cubeState.left, mLeft);
        UpdateMap(cubeState.right, mRight);
    }

    void UpdateMap(List<GameObject> face, Transform side)
    {
        int i = 0;
        foreach(Transform map in side)
        {
            // Debug.Log("Setting color for " + face[i].name[0] + " face");
            if (face[i].name[0] == 'F')
            {
                map.GetComponent<Image>().color = Color.white;
            }
            if (face[i].name[0] == 'B')
            {
                map.GetComponent<Image>().color = Color.yellow;
            }
            if (face[i].name[0] == 'U')
            {
                map.GetComponent<Image>().color = new Color(1, 0.5f, 0, 1);
            }
            if (face[i].name[0] == 'D')
            {
                map.GetComponent<Image>().color = Color.red;
            }
            if (face[i].name[0] == 'L')
            {
                map.GetComponent<Image>().color = Color.green;
            }
            if (face[i].name[0] == 'R')
            {
                map.GetComponent<Image>().color = Color.blue;
            }
            i++;
        }
    }
}
