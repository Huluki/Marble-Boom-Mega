using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndPoint : MonoBehaviour
{
    [SerializeField] public static Button[] buttons;
    public static int maxLevel = 1;
    private static int currentLevel;
    [SerializeField] public  GameObject goalMenu;
    private void OnTriggerEnter(Collider eh)
    {
        if (eh.CompareTag("Player"))
        {
            PlayerController.goalChanger = true;
            UnlockNewLevel();
            StartCoroutine(Wait(2));
            Debug.Log("yuh cuh it worked like normal");
            PlayerController.GoalTextChange(true);
        }
    }

    private void Awake()
    {
        
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (buttons != null)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
            }

            for (int i = 0; i < 1; i++)
            {
                buttons[i].interactable = true;
            }
        }
    }
    
    public static void hitGoal()
    {
        if (currentLevel >= maxLevel)
        {
            maxLevel = currentLevel + 1;
        }
        for (int i = 0; i < maxLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public static void doAnythingPlease()
    {
        
            PlayerController.goalChanger = true;
           

            hitGoal();
            PlayerController.GoalTextChange(true);
        SceneController.instance.NextLevel();
        PlayerController.hitEnd = false;
    }
    
    public static void UnlockNewLevel()
    {
        
    }

     IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        
    }
}
