using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{

    public void ReturnToMainMenu()
    {
        Debug.Log("The In-Game Scene has been unloaded and we are now in the MainMenu Scene");
        SceneManager.LoadScene(0);
    }
}
