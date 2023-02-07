using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int level; //level (scene number in builder) to upload - should be set in every scene accordingly

    /*Scene order:
     * 0 - menu
     * 1 - lvl1
     * 2 - lvl2
     * 3 - lvl3
     * 4 - lvl4
     * 5 - lvl5
     * 6 - lvl6
     * 7 - lvl7
     * 8 - results / records table
     * 9 - gameOver menu
     */ 

    /*
     * Collision with portal
     */
    private void OnCollisionEnter(Collision player) 
    {
        if(player.gameObject.CompareTag("Player"))
        {
            GetToNextLevel();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(level);
    }

    /// <summary>
    /// Instructions to get to the next scene
    /// </summary>
    public void GetToNextLevel()
    {
        SceneManager.LoadScene(level);
        if (level == 0)
        {
            if (GameManager.Instance != null)
            {
                Destroy(GameManager.Instance.gameObject);
                //GameManager.Instance.gameObject.SetActive(false);
            }
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.levelReached++;
            GameManager.Instance.TurnTimeBack();
            GameManager.Instance.reloadSceneButton.interactable = true;
        }

        if (level == 8 || level == 9)
        {
            GameManager.Instance.isTimeTicking = false;
            GameManager.Instance.reloadSceneButton.interactable = false;
        }
        if(level == 3) // levels get more complicated so we put time up
        {
            GameManager.Instance.maxTime = 15;
            GameManager.Instance.TurnTimeBack();
        }
        if(level == 4)
        {
            GameManager.Instance.maxTime = 20;
            GameManager.Instance.TurnTimeBack();
        }
        if (level == 5)
        {
            GameManager.Instance.maxTime = 25;
            GameManager.Instance.TurnTimeBack();
        } 
        if (level == 6)
        {
            GameManager.Instance.maxTime = 50;
            GameManager.Instance.TurnTimeBack();
        }
        if(level == 7)
        {
            GameManager.Instance.maxTime = 150;
            GameManager.Instance.TurnTimeBack();
        }
        
    }

    /// <summary>
    /// Reload scene
    /// </summary>
    public void ReloadLevel()
    {
        SceneManager.LoadScene(GameManager.Instance.levelReached);
    }

    /*
     * Controls if time is over
     */
    private void Update()
    {
        if(GameManager.Instance != null)
        {
            if (GameManager.Instance.isTimeOver == true)
            {
                GameManager.Instance.TurnTimeBack();
                GameManager.Instance.isTimeTicking = false;
                GameManager.Instance.isTimeOver = false;
                level = 9; //sends to a gameOver scene
                GetToNextLevel();
            }
        }
    }
}
