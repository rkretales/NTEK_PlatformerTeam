using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCubeSize : MonoBehaviour
{
    private float sHeight;
    private float sWidth;
    private void Start()
    {
        float targetScreenWidth = 1920f; // Set your target screen width
        float targetScreenHeight = 1080f; // Set your target screen height

        float currentScreenWidth = Screen.width;
        float currentScreenHeight = Screen.height;

        float scaleX = currentScreenWidth / targetScreenWidth;
        float scaleY = currentScreenHeight / targetScreenHeight;

        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
