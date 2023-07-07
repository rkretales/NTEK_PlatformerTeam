using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Lowscope.Saving;

public class PersonalBest : MonoBehaviour
{
    [SerializeField]private TMPro.TMP_Text PBTimerText;
    [SerializeField]private TMPro.TMP_Text CurrentTimerText;
    private float currentTime;
    private TimerScript timerScript;
    // Start is called before the first frame update
    private void Start()
    {
        timerScript = FindObjectOfType<TimerScript>();

        currentTime = SaveMaster.GetFloat("CurrentTime");
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        CurrentTimerText.text = time.ToString(@"mm\:ss\:ff");

        float bestTime = SaveMaster.GetFloat("BestTime");

        if (currentTime > bestTime)
        {
            bestTime = currentTime;
            SaveMaster.SetFloat("BestTime", currentTime);
        }

        TimeSpan bestTimer = TimeSpan.FromSeconds(bestTime);
        PBTimerText.text = bestTimer.ToString(@"mm\:ss\:ff");

    }
}
