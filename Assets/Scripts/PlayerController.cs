using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public GameObject goalMenu;
    public Button pauseButton;
    public static Rigidbody Rb;
    private InputManager _input;
    public static bool hitEnd = false;
    
    [SerializeField] private Transform camParent;
    [SerializeField] private Transform cam;
    
    [SerializeField] private float rotSpeed = 10f;
    [SerializeField] private float jumpPower = 10f;
    
    [SerializeField] private float moveForce = 30f;
    public static float maxSpeed = 60f;
    [SerializeField] private float airControlMultiplier = 0.2f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float extraGravity = 20f;
    [SerializeField] float maxSlopeAngle = 45f;
    
    [SerializeField]  private TextMeshProUGUI powerUpText;
    [SerializeField] public TextMeshProUGUI goalText;
    [SerializeField] private float level = 0;
    [SerializeField] private Vector3 respawnPoint = new Vector3(0, 1, 0);
    
    [SerializeField] Transform marbleVisual;
	public Transform spawnPoint;
    [SerializeField] float radius = 0.75f;
    private string lookType = "d";
    
    private Vector3 groundNormal;
    private float slopeAngle;
    private bool speedChanger =  false;
    public static bool goalChanger = false;
    public static bool shouldReset = false;
    public static bool shouldMove = false;

    private float currentJump;
    private float currentSpeed;
    
    private float currentGravity;

    public static float powerUpNum = 0;

    private float groundCheckRadius = .75f * .3f;
    private float groundCheckDistance = 1f;
    
    void Start()
    {
        shouldReset = false;
        Rb = GetComponent<Rigidbody>();
        _input = InputManager.instance;
        powerUpNum = 0;
        goalText.enabled = false;
        hitEnd = false;
        shouldReset = true;
        shouldMove = false;
        Timer.shouldTime = false;
        maxSpeed = 60f;
        transform.position = spawnPoint.position;
        Timer.tellMill = false;
        StartCoroutine(Wait(3, 7));
    }
    
    private void FixedUpdate()
    {
        if (shouldMove)
        {
            HandleMovement(Time.fixedDeltaTime);
            SpeedChange();
            Reset();
        }
        Debug.Log(CameraController.shouldLook);
    }
    
    private bool IsGrounded()
    {
        // Cast a sphere downwards from the center of the marble
         // Adjust based on your marble size
        float groundDistance = 0.3f; // Margin for error
        
        RaycastHit groundHit;
        bool trueGrounded = Physics.SphereCast(transform.position, radius * 0.4f, Vector3.down, out groundHit, groundCheckDistance);
        
        if(trueGrounded){
         groundNormal = groundHit.normal;
         slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
        }
        
        return trueGrounded;
    }

    
    
  

    private void HandleMovement(float deltaTime)
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * _input.Move.y + camRight * _input.Move.x;
        
        Vector3 horizontalVel = Rb.linearVelocity;
        horizontalVel.y = 0;

        if (horizontalVel.magnitude > maxSpeed)
        {
            Rb.linearVelocity = horizontalVel.normalized * maxSpeed + Vector3.up * Rb.linearVelocity.y;
        }

        if (IsGrounded() && _input.Move.magnitude < 0.1f)
        {
            Rb.linearVelocity = Vector3.Lerp(
                Rb.linearVelocity,
                new Vector3(0, Rb.linearVelocity.y, 0),
                groundDrag * Time.fixedDeltaTime
            );
        }

        if (!IsGrounded())
        {
            Rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }
        
        float controlMultiplier = IsGrounded() ? 1f : airControlMultiplier;
        
        Vector3 slopeMoveDir = Vector3.ProjectOnPlane(moveDir, groundNormal).normalized;
        
        IsGrounded();
        
        bool tooSteep = slopeAngle > maxSlopeAngle;

        
        if (IsGrounded() && !tooSteep)
        {
            Rb.AddForce(slopeMoveDir * moveForce, ForceMode.Acceleration);
        }
        else if (!IsGrounded())
        {
            Rb.AddForce(moveDir * (moveForce * controlMultiplier), ForceMode.Acceleration);
        }
    }
    
    

    private void SpeedChange()
    {
        if (speedChanger)
        {
            float duration = 3f;
            
            maxSpeed = Mathf.MoveTowards(maxSpeed, currentSpeed, duration * Time.deltaTime);
            Wait(duration, 1);
        }

        if (goalChanger)
        {
            
           maxSpeed = Mathf.MoveTowards(maxSpeed, 0, 20 * Time.deltaTime);
            
        }
    }
    
    

    public void OnJump()
    {
        Debug.Log ("JumpPreformed");
        // Only trigger on the initial press (performed) and if grounded
        if (IsGrounded() && shouldMove)
        {
            Rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Vector3 origin = transform.position + Vector3.down * 0.05f;

        // Top sphere (start)
        Gizmos.DrawWireSphere(origin, groundCheckRadius);

        // Bottom sphere (end)
        Vector3 end = origin + Vector3.down * groundCheckDistance;
        Gizmos.DrawWireSphere(end, groundCheckRadius);

        // Connect them
        Gizmos.DrawLine(origin + Vector3.forward * groundCheckRadius, end + Vector3.forward * groundCheckRadius);
        Gizmos.DrawLine(origin - Vector3.forward * groundCheckRadius, end - Vector3.forward * groundCheckRadius);
        Gizmos.DrawLine(origin + Vector3.right * groundCheckRadius, end + Vector3.right * groundCheckRadius);
        Gizmos.DrawLine(origin - Vector3.right * groundCheckRadius, end - Vector3.right * groundCheckRadius);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("powerUpSpeed") && powerUpNum ==0)
        {
            powerUpNum = 1;
            powerUpText.text = "powerup: \n" +
                               "  speed";
            
        } else if (other.CompareTag("powerUpJump")&& powerUpNum ==0)
        {
            powerUpNum = 2;
            powerUpText.text = "powerup: \n" +
                               "  jump";
            
        }else if (other.CompareTag("powerUpBounce")&& powerUpNum ==0)
        {
            powerUpNum = 4;
            
            powerUpText.text = "powerup: \n" +
                               "  bounce";
            
        }else if (other.CompareTag("powerUpLowGrav")&& powerUpNum ==0)
        {
            powerUpNum = 5;
            
            powerUpText.text = "powerup: \n" +
                               "  low grav";
            
        }else if (other.CompareTag("reset"))
        {
            Debug.Log("reset");
            shouldReset =  true;
        }else if (other.CompareTag("goal"))
        {
            /*
            if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
            {
                PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
                PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel",1)+1);
                PlayerPrefs.Save();
                Debug.Log("yuh cuh it worked");
            }
            goalText.text = "YOU WON YAY HAPPY CELEBRATE!!";
            goalText.enabled = true;
            goalChanger = true;
            StartCoroutine(Wait(2,3));
            */
            Debug.Log("THIS SHOULD WORK");
            hitEnd = true;
            Timer.shouldTime = false;
            goalChanger = true;
            pauseButton.gameObject.SetActive(false);
            StartCoroutine(Wait(1,3));
            
            Timer.tellMill =  true;
            
            EndPoint.doAnythingPlease();
            //Time.timeScale = 0;
        }
            
    }

    public static void GoalTextChange(bool goal)
    {
        if (goal)
        {
            
        } else if (!goal)
        {
            shouldReset = true;
            goalChanger = false;
            //maxSpeed = 60;
        }
    }
    

    public void PowerUpCheck()
    {
        if (powerUpNum > 0)
        {
            if (powerUpNum == 1)
            {
                //1 speed
                
                Vector3 boostDir = cam.forward;
                boostDir.y = 0.0f;                 // keep boost horizontal
                boostDir.Normalize();
                
                
                currentSpeed = maxSpeed;
                maxSpeed = 80;
                Rb.AddForce(boostDir * 80, ForceMode.VelocityChange);
                
                speedChanger = true;
                powerUpText.text = "powerup: " +
                                   "  ";
                powerUpNum = 0;
            } else if (powerUpNum == 2)
            {
                //2 jump
                
                currentJump = jumpPower;
                jumpPower = currentJump * 1.5f;
            
                StartCoroutine(Wait(5,2));
            }else if (powerUpNum == 4)
            {
                //4 bounce
                
                Rb.AddForce(Vector3.up * (jumpPower*4), ForceMode.Impulse);
                
                //jumpPower =  currentJump;
                powerUpText.text = "powerup: " +
                                   "  ";
                powerUpNum = 0;
            }else if (powerUpNum == 5)
            {
                //5 low grav
                
               Rb.useGravity = false;
               currentJump = jumpPower;
               jumpPower = currentJump * 1.3f;
                
               currentGravity = extraGravity;
               extraGravity = 15;
               StartCoroutine(Wait(10,5));
                
                
            }
        }
    }

    IEnumerator Wait(float duration, float instance)
    {
        powerUpNum = 0;
        yield return new WaitForSecondsRealtime(duration);
        if (instance == 1)
        {
            // speed
            speedChanger = false;
        } else if (instance == 2)
        {
            // jump
            
            jumpPower = currentJump;

            powerUpNum = 0;
            powerUpText.text = "powerup: " +
                               "  ";
        } else if (instance == 3)
        {
            // goal
            
            goalMenu.SetActive(true);
            CameraController.shouldLook =  false;
            Time.timeScale = 0;
            goalChanger = false;
            maxSpeed = 60;
        }else if (instance == 5)
        {
            extraGravity = currentGravity;
            jumpPower = currentJump;
            Rb.useGravity = true;
            
            powerUpText.text = "powerup: " +
                               "  ";
            powerUpNum = 0;
        } else if (instance == 6)
        {
           /*
            Time.timeScale = 0;
            CameraController.shouldLook =  false;
            goalMenu.SetActive(true);
            */
        }
        else if (instance == 7)
        {
            
            
        }
    }
    public void Reset()
    {
        if (shouldReset)
        {
            Rb.linearVelocity = Vector3.zero;
            Rb.angularVelocity = Vector3.zero;

            powerUpText.text = "powerup: " +
                               "  ";
            transform.position = spawnPoint.position;
            shouldReset = false;
        }
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        var device = context.control.device;
        /*
        if (device is Mouse)
        {
            
            lookType = "Mouse";
            CameraController.orbitSpeed = .006f;
            CameraController.followSpeed = 5f;
            CameraController.tiltSpeed = .003f;
        }
        else if (device is Gamepad)
        {
           
            CameraController.orbitSpeed = .008f;
            CameraController.followSpeed = 5f;
            CameraController.tiltSpeed = .0065f;
            lookType = "Gamepad";
            
        }
        */
    }

    public void changeReset()
    {
        shouldReset = true;
    }
    
}