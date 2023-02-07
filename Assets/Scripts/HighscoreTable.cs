using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Table of records class - saves current result, upload all the rest in the final table 
/// </summary>
public class HighscoreTable : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName; // name input
    private Transform entryContainer; //parent container
    private Transform entryTemplate; //child template
    private List<Transform> highscoreEntryTransformList; // We need that list in order to order our templates
                                                         // so that they won't be gathered together in one spot
    
    private void Awake()
    {
        entryContainer = transform.Find("highScoreEntryContainer"); // references to our real obj-s on the scene
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false); // We don't need the initial garbage appear so that
                                                   // we excldue it here 
        AddHighScoreEntry(GameManager.Instance.GetBonusPoints(), GameManager.Instance.totalTime, playerName.text); // save new data

        string jsonString = PlayerPrefs.GetString("highscoreTable1");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString); // container of all data to be sorted

        for (int i = 0; i < highscores.highscoreEntryList.Count; i++) // sort-score algorithm
        {
            for (int j = i+1; j < highscores.highscoreEntryList.Count; j++)
            {

                if (highscores.highscoreEntryList[j].bonuses == highscores.highscoreEntryList[i].bonuses ||
                    highscores.highscoreEntryList[j].time < highscores.highscoreEntryList[i].time)
                {
                    //Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
                if (highscores.highscoreEntryList[j].bonuses > highscores.highscoreEntryList[i].bonuses)
                {
                    //Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();

        for (int i = 0; i < 10; i++) // Show only first 10 entries on the board
        { 
            CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);  
        }

        //foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) //Show all the entries
        //{
        //    CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        //}
    }

    /// <summary>
    /// Shows how the results are placed in the table
    /// </summary>
    /// <param name="highscoreEntry">data from JSON container</param>
    /// <param name="container">physical container in UI/param>
    /// <param name="transformList">param to count entries</param>
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, 
                                    Transform container, List<Transform> transformList)
    {
        float templateHeight = 80f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count); // shift down with each iteration
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
            default:
                rankString = rank + "TH";
                break;
        }
        entryTransform.Find("posText").GetComponent<TMP_Text>().text = rankString;

        int bonuses = highscoreEntry.bonuses;
        entryTransform.Find("bonusesText").GetComponent<TMP_Text>().text = bonuses.ToString();

        float time = highscoreEntry.time;
        entryTransform.Find("timeText").GetComponent<TMP_Text>().text = time.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TMP_Text>().text = name;

        transformList.Add(entryTransform);
    }

    public void AddHighScoreEntry(int bonuses, float time, string name)
    {
        //Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { bonuses = bonuses, time = time, name = name};

        //Load saved Highscores data
        string jsonString = PlayerPrefs.GetString("highscoreTable1");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
        //Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        //Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable1", json);
        PlayerPrefs.Save();
    }

    //To Save highscores as an object, List doesn't work with JSON
    [System.Serializable]
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

 /*
 * This class represents a single high score entry
 */ 
     [System.Serializable]
    private class HighscoreEntry
    {
        public int bonuses;
        public float time;
        public string name;
    }
}

