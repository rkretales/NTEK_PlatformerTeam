using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCubeSize : MonoBehaviour
{
    public float targetWidth = 1080f; // Set the target width for scaling
    public float targetHeight = 1920f; // Set the target height for scaling
    public float maxScaleFactor = 1.5f; // Adjust this value to control the maximum scale

    void Start()
    {
        ScaleObject();
    }

    void ScaleObject()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float screenAspect = screenWidth / screenHeight;

        float targetAspect = targetWidth / targetHeight;

        float scale;

        if (screenAspect > targetAspect)
        {
            // Landscape orientation
            scale = targetWidth / screenWidth;
        }
        else
        {
            // Portrait orientation
            scale = targetHeight / screenHeight;
        }

        scale *= maxScaleFactor;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
