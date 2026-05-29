using TMPro;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static float sensitivity = 1f;
    private float orbit;
    private float tilt;
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityText;
    
    void Start()
    {
        sensitivitySlider.value = sensitivity;
        CameraController.orbitSpeed = 0.006f;
        CameraController.followSpeed = 5f;
        CameraController.tiltSpeed = .003f;
        
        orbit = CameraController.orbitSpeed;
        tilt = CameraController.tiltSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
            sensitivity = sensitivitySlider.value;
        

        
        sensitivity = Mathf.Round(sensitivity*100f)/100f;
        CameraController.orbitSpeed = orbit*sensitivity;
        CameraController.tiltSpeed = tilt* sensitivity;
        sensitivityText.text = "" + sensitivity*100f+"%";
    }
    
    
}
