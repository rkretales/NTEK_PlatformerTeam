using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Lowscope.Saving;

public class TimerScript : MonoBehaviour
{
    public TMPro.TMP_Text TimerText;
    public float currentTime;
    public GameObject startButton;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!startButton.activeSelf)
        {
            currentTime = currentTime + Time.deltaTime; 

            TimeSpan time = TimeSpan.FromSeconds(currentTime);

            TimerText.text = time.ToString(@"mm\:ss\:ff");
            SaveMaster.SetFloat("CurrentTime", currentTime);
        }
    }
}
