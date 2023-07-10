using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lowscope.Saving;

public class LeaderboardManager : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private void Start()
    {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        HighScores highscores = new HighScores();

        // Load saved Highscores
        string jsonString = SaveMaster.GetString("highscoreTable");

        if (string.IsNullOrEmpty(jsonString))
        {
            highscores = new HighScores();
            highscores.highScoreEntryList = new List<HighScoreEntry>();
        }
        else
        {
            highscores = JsonUtility.FromJson<HighScores>(jsonString);
        }

        HighScoreEntry highScoreEntry = new HighScoreEntry
        {
            name = SaveMaster.GetString("PlayerName"),
            time = SaveMaster.GetFloat("CurrentTime")
        };
        AddHighScoreEntry(highScoreEntry.time, highScoreEntry.name, highscores);

        // Sorting the list from lowest to highest time
        for (int i = 0; i < highscores.highScoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highScoreEntryList.Count; j++)
            {
                if (highscores.highScoreEntryList[j].time < highscores.highScoreEntryList[i].time)
                {
                    // Swap
                    HighScoreEntry tmp = highscores.highScoreEntryList[i];
                    highscores.highScoreEntryList[i] = highscores.highScoreEntryList[j];
                    highscores.highScoreEntryList[j] = tmp;
                }
            }
        }

        if (highscores.highScoreEntryList.Count > 7)
        {
            for (int h = highscores.highScoreEntryList.Count; h>7; h--)
            {
                highscores.highScoreEntryList.RemoveAt(7);
            }
        }
    
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighScoreEntry highscoreEntry in highscores.highScoreEntryList)
        {
            CreateHighScoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    public void GetName(string name)
    {
        SaveMaster.SetString("PlayerName", name);
    }

    private void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 78f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
        }
        entryTransform.Find("Rank").GetComponent<TMPro.TMP_Text>().text = rankString;

        string name = highScoreEntry.name;
        entryTransform.Find("Player").GetComponent<TMPro.TMP_Text>().text = name;

        float time = highScoreEntry.time;
        entryTransform.Find("Time").GetComponent<TMPro.TMP_Text>().text = time.ToString();

        transformList.Add(entryTransform);
    }

    [System.Serializable]
    public class HighScores
    {
        public List<HighScoreEntry> highScoreEntryList;

        public HighScores()
        {
            highScoreEntryList = new List<HighScoreEntry>();
        }
    }

    private void AddHighScoreEntry(float time, string name, HighScores highscores)
    {
        // Create HighScoreEntry
        HighScoreEntry highScoreEntry = new HighScoreEntry { time = time, name = name };

        // Add new entry to Highscores
        highscores.highScoreEntryList.Add(highScoreEntry);

        if (highscores.highScoreEntryList.Count > 7)
        {
            for (int h = highscores.highScoreEntryList.Count; h>7; h--)
            {
                highscores.highScoreEntryList.RemoveAt(7);
            }
        }

        // Save Updated HighScores
        string json = JsonUtility.ToJson(highscores);
        SaveMaster.SetString("highscoreTable", json);
    }

    // Represents a single High Score Entity
    [System.Serializable]
    public class HighScoreEntry
    {
        public float time;
        public string name;
    }
}