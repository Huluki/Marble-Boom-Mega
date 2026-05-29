using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{ 
    
    
    public void TurnOnMenu(GameObject turnOn)
    {
        PlayerController.shouldMove = false;
        turnOn.SetActive(true);
    }
    
    public void TurnOffMenu( GameObject turnOff)
    {
        PlayerController.shouldMove = false;
        turnOff.SetActive(false);
    }

    public void StartLevel()
    {
        PlayerController.shouldMove = true;
    }


    public void move(bool move)
    {
        PlayerController.shouldMove = move;
    }

    public void StopMovingPlease()
    {
        PlayerController.shouldMove = false;
    }
    
    public Button[] buttons;

    private void Awake()
    {
        Time.timeScale = 1f;
        PlayerController.shouldMove = true;
        int unlockedLevel = EndPoint.maxLevel;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    private void Start()
    {
        PlayerController.shouldMove = true;
    }

    public void OpenLevel (int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
        CameraController.shouldLook = true;
    }
    
   
}
