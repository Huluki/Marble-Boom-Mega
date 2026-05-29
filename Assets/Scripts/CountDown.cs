using UnityEngine;
using TMPro; // Use 'using UnityEngine.UI;' if using standard UI Text
using System.Collections;

public class CountDown : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Drag your TextMeshPro object here in Inspector
    public int countdownSeconds = 3;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (countdownSeconds > 0)
        {
            countdownText.text = countdownSeconds.ToString();
            yield return new WaitForSeconds(1f); // Wait for 1 second
            
            countdownSeconds--;
        }

        PlayerController.shouldMove = true;
        Timer.shouldTime = true;
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false); // Hide the text when finished
    }
}