using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
