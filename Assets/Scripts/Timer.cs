using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI text;
  float elapsedTime;
  public static bool shouldTime;
  public static bool tellMill;
  private int minutes;
  private int seconds;
  private int milliseconds;
    void Update()
    {
        if (shouldTime)
        {
            elapsedTime += Time.deltaTime;
             minutes = Mathf.FloorToInt(elapsedTime / 60);
             seconds = Mathf.FloorToInt(elapsedTime % 60);
             milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);
            if (tellMill)
            {
                text.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
            }
            else
            {
                text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            
        } 
        if (tellMill)
        {
            text.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
            tellMill = false;
        }
        
    }
}
