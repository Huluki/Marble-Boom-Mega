using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   bool isPaused = false;
   
   [SerializeField] GameObject pauseMenu;
   [SerializeField] Transform cameraPi;
   [SerializeField] EventSystem eventSystem;
   [SerializeField] bool lastLevel = false;
   [SerializeField] GameObject firstButton;
   public void Pause()
   {
      if (!isPaused && !PlayerController.hitEnd)
      {
         CameraController.shouldLook = false;
         pauseMenu.SetActive(true);
         
         Time.timeScale = 0;
         isPaused = true;
         eventSystem.SetSelectedGameObject(firstButton.gameObject);
      }
      else
      {
         Resume();
         isPaused = false;
      }
      eventSystem.SetSelectedGameObject(firstButton.gameObject);
   }

   public void Resume()
   {
      if (isPaused && !PlayerController.hitEnd)
      {

         pauseMenu.SetActive(false);
         CameraController.shouldLook = true;
         Time.timeScale = 1;
      }
   }

   public void Home()
   {
      CameraController.shouldLook = false;
      Time.timeScale = 1;
      SceneManager.LoadScene("Scenes/Main Menu");

   }
   
   public void NextLevel()
   {
      if (!lastLevel)
      {
         CameraController.shouldLook = true;
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
         Time.timeScale = 1;
      }
   }

   public void Restart()
   {
      CameraController.shouldLook = true;
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      Time.timeScale = 1;
   }
}
