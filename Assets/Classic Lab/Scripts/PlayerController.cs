using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class ExtensionMethods
{
    public static void AddListener(this EventTrigger trigger, EventTriggerType eventType, System.Action<PointerEventData> listener)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
        trigger.triggers.Add(entry);
    }
}

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cameraTransform;

    private Database db;
    private bool crouch = false;
    private int rightFingerId = -1;
    private float speed = 10f;
    private float crouchSpeed = 5f;
    private float walkSpeed = 15f;
    private float sprintSpeed = 30f;
    private float cameraPitch;
    private float halfScreenWidth = Screen.width / 2;
    private float cameraHeight = 5f;
    private Vector2 lookInput;
    private Vector3 velocity = Vector3.zero;

    private int shortClicks = 0;
    private float resetTime = .3f;
    private bool doubleClick = false;

    private bool isGrounded;
    private float gravity = -9.81f;
    private float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    
    private void Start()
    {
        db = Database.db;
        GameObject.FindGameObjectWithTag("Action").GetComponent<Button>().onClick.AddListener(SwitchAction);
    }

    private void Update()
    {
        if (!Player.pause)
        {
            Rotate();
            Move();
            FreeFall();
        }
        else
        {
            Cursor.visible = true;
            if (rightFingerId > -1)
            {
                rightFingerId = -1;
            }
            if (GameObject.FindGameObjectWithTag("Sprint") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("Sprint"));
            }
            if (Player.platform == "Desktop")
            {
                if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                {
                    crouch = false;
                }
            }
        }
    }

    private void FreeFall()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void CrouchCheck()
    {
        if (crouch)
        {
            shortClicks = 0;
            doubleClick = false;
            if (GameObject.FindGameObjectWithTag("Sprint") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("Sprint"));
            }
            Crouch();
        }
        else if (doubleClick)
        {
            Sprint();
        }
        else
        {
            Walk();
        }
    }

    private void Move()
    {
        if (Player.platform == "Desktop")
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                crouch = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                crouch = false;
            }
            else if (!crouch)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    shortClicks++;
                    if (shortClicks == 1)
                    {
                        StartCoroutine(ShortClicksReset());
                    }
                    else if (shortClicks == 2)
                    {
                        doubleClick = true;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.W) && doubleClick || Input.GetKeyUp(KeyCode.UpArrow) && doubleClick)
                {
                    shortClicks = 0;
                    doubleClick = false;
                }
                if (doubleClick)
                {
                    Sprint();
                }
                else
                {
                    if (GameObject.FindGameObjectWithTag("Sprint") != null)
                    {
                        Destroy(GameObject.FindGameObjectWithTag("Sprint"));
                    }
                    Walk();
                }
            }
            CrouchCheck();
            velocity = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        }
        else if (Player.platform == "Mobile")
        {
            FixedJoystick joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FixedJoystick>();
            if (!crouch)
            {
                if (joystick.Vertical >= .97f)
                {
                    Sprint();
                }
                else
                {
                    if (GameObject.FindGameObjectWithTag("Sprint") != null)
                    {
                        Destroy(GameObject.FindGameObjectWithTag("Sprint"));
                    }
                    Walk();
                }
            }
            velocity = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            velocity = transform.TransformDirection(velocity);
        }
        characterController.Move(velocity * speed * Time.deltaTime);
    }

    private IEnumerator ShortClicksReset()
    {
        yield return new WaitForSeconds(resetTime);
        if (shortClicks < 2)
        {
            shortClicks = 0;
        }
    }

    private void Rotate()
    {
        if (Player.platform == "Desktop")
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            TurnAround(Input.GetAxis("Mouse X") * db.desktopSensitivity * Time.deltaTime, Input.GetAxis("Mouse Y") * db.desktopSensitivity * Time.deltaTime);
        }
        else if (Player.platform == "Mobile")
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                switch (t.phase)
                {
                    case TouchPhase.Began:
                        if (t.position.x > halfScreenWidth && rightFingerId == -1)
                        {
                            rightFingerId = t.fingerId;
                        }
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (t.fingerId == rightFingerId)
                        {
                            rightFingerId = -1;
                        }
                        break;
                    case TouchPhase.Moved:
                        if (t.position.x > halfScreenWidth && t.fingerId == rightFingerId)
                        {
                            lookInput = t.deltaPosition * db.mobileSensitivity * Time.deltaTime;
                            TurnAround(lookInput.x, lookInput.y);
                        }
                        break;
                    case TouchPhase.Stationary:
                        if (t.fingerId == rightFingerId)
                        {
                            lookInput = Vector2.zero;
                        }
                        break;
                }
            }
        }
    }

    private void TurnAround(float inputX, float inputY)
    {
        cameraPitch = Mathf.Clamp(cameraPitch - inputY, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        transform.Rotate(transform.up, inputX);
    }

    public void SwitchAction()
    {
        if (!Player.pause/* && Player.platform == "Mobile"*/)
        {
            crouch = !crouch;
            CrouchCheck();
        }
    }

    private void Crouch()
    {
        GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crouch");
        speed = crouchSpeed;
        GameObject playerView = GameObject.FindGameObjectWithTag("MainCamera");
        playerView.transform.localPosition = new Vector3(playerView.transform.localPosition.x, cameraHeight / 2, playerView.transform.localPosition.z);
    }

    private void Walk()
    {
        GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Stand");
        speed = walkSpeed;
        GameObject playerView = GameObject.FindGameObjectWithTag("MainCamera");
        playerView.transform.localPosition = new Vector3(playerView.transform.localPosition.x, cameraHeight, playerView.transform.localPosition.z);
    }

    private void Sprint()
    {
        speed = sprintSpeed;
        if (GameObject.FindGameObjectWithTag("Sprint") == null)
        {
            Instantiate(Resources.Load<GameObject>("Sprint/Sprint"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
        }
    }
}
