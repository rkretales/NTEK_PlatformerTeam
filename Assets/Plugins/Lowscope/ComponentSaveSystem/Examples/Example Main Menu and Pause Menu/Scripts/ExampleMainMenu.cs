using System;
using Lowscope.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lowscope.Saving.Examples
{
    public class ExampleMainMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button buttonContinue;
        [SerializeField] private Button buttonNew;
        [SerializeField] private Button buttonLoad;
        [SerializeField] private Button buttonQuit;

        [SerializeField] private ExampleErrorScreen errorMessage;
        [SerializeField] private ExampleSlotMenu slotMenu;

        [Header("Configuration")]
        [SerializeField] private string sceneToLoadOnNewGame;

        private int lastUsedValidSlot;

        private void Start()
        {
            lastUsedValidSlot = SaveMaster.GetLastUsedValidSlot();
            buttonContinue.interactable = lastUsedValidSlot != -1;

            buttonContinue.onClick.AddListener(Continue);
            buttonNew.onClick.AddListener(NewGame);
            buttonLoad.onClick.AddListener(LoadGame);
            buttonQuit.onClick.AddListener(QuitGame);

            SaveMaster.OnDeletedSave += OnDeletedSave;
            SaveMaster.OnLoadingFromDiskCorrupt += OnLoadCorrupt;
        }

        private void OnDestroy()
        {
            SaveMaster.OnDeletedSave -= OnDeletedSave;
            SaveMaster.OnLoadingFromDiskCorrupt -= OnLoadCorrupt;
        }
        
        private void OnLoadCorrupt(int slot)
        {
            slotMenu.gameObject.SetActive(false);
            
            errorMessage.Configure("Unable to load save file",
                "The save file might potentially be corrupt. \n" +
                "Please contact the developer for the game for support.");
            
            // In case you want the game to just load.
            // The save system uses two files to prevent corruption cases.
            // So if save A fails, it tries to load save B and vice versa (attempts to load newest file first).
            // You can toggle "Archive Save File On Full Corruption" in Window/Saving/Open Save Settings
            // And then call the set slot again here. Using SaveMaster.SetSlot(slot, true);
        }

        private void OnDeletedSave(int obj)
        {
            if (lastUsedValidSlot == obj)
                buttonContinue.interactable = false;
        }

        private void Continue()
        {
            SaveMaster.SetSlotToLastUsedSlot(true);
        }

        private void LoadGame()
        {
            slotMenu.gameObject.SetActive(true);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void NewGame()
        {
            int slot;
            if (SaveMaster.SetSlotToNewSlot(false, out slot))
            {
                SceneManager.LoadScene(sceneToLoadOnNewGame);
            }
            else
            {
                errorMessage.Configure("All slots full",
                    "No more available save slots. \n" +
                    "Please overwrite or remove a slot");
            }
        }
    }
}