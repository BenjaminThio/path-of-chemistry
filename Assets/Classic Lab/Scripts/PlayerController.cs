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
    public float scale = 1f;
    public Vector3 velocity = Vector3.zero;

    private Database db;
    public bool crouch = false;
    private int rightFingerId = -1;
    private float speed = 10f;
    private float crouchSpeed = 5f;
    private float walkSpeed = 15f;
    private float sprintSpeed = 30f;
    private float cameraPitch;
    private float halfScreenWidth = Screen.width / 2;
    private Vector2 lookInput;

    public int shortClicks = 0;
    public bool doubleClick = false;
    private float resetTime = .3f;

    private bool isGrounded;
    private float gravity = -9.81f;
    private float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float cameraHeight;
    public AudioSource footstepSound;

    public bool crouchBeforeSit = false;

    private void Start()
    {
        db = Database.db;

        speed *= scale;
        crouchSpeed *= scale;
        walkSpeed *= scale;
        sprintSpeed *= scale;

        GameObject.FindGameObjectWithTag("Action").GetComponent<Button>().onClick.AddListener(SwitchAction);
    }

    private void Update()
    {
        Player player = GetComponent<Player>();

        if (player.moveable)
        {
            FreeFall();
            if (!Player.pause)
            {
                Rotate();
                Move();
            }
            else
            {
                //Cursor.visible = true;
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
                shortClicks = 0;
                doubleClick = false;
            }
        }
        else if (!Player.pause)
        {
            Rotate();

            if (Player.platform == "Desktop")
            {
                if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                {
                    if (!crouchBeforeSit)
                    {
                        Stand();
                    }
                    else
                    {
                        crouchBeforeSit = false;
                    }
                }
            }
        }
    }

    private void Stand()
    {
        Player player = GetComponent<Player>();
        GameObject playerView = GameObject.FindGameObjectWithTag("MainCamera");

        player.raycastRange = 10f * scale;
        player.moveable = true;
        player.transform.position = player.positionBeforeSit;
        player.targetedChair.GetComponent<MeshCollider>().enabled = true;
        player.targetedChair = null;
        playerView.transform.localPosition = new Vector3(playerView.transform.localPosition.x, cameraHeight, playerView.transform.localPosition.z);
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

    public void CrouchCheck()
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
            velocity = new Vector3(0, velocity.y, 0) + transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
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
            velocity = new Vector3(joystick.Horizontal, velocity.y, joystick.Vertical);
            velocity = transform.TransformDirection(velocity);

            if (crouch)
            {
                if (velocity.x < 0 || velocity.x > 0 || velocity.z < 0 || velocity.z > 0)
                {
                    GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crouch Walk V2");
                }
                else //if (velocity.x == 0 && velocity.z == 0)
                {
                    GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crouch");
                }
            }
        }

        characterController.Move(velocity * speed * Time.deltaTime);
        GenerateFootstepSoundBasedOnSpeed();
    }

    private void GenerateFootstepSoundBasedOnSpeed()
    {
        if (velocity.x < 0 || velocity.x > 0 || velocity.z < 0 || velocity.z > 0)
        {
            if (speed == walkSpeed || speed == sprintSpeed || speed == crouchSpeed)
            {
                if (speed == walkSpeed)
                {
                    footstepSound.pitch = 1f;
                }
                else if (speed == sprintSpeed)
                {
                    footstepSound.pitch = 1.4f;
                }
                else if (speed == crouchSpeed)
                {
                    footstepSound.pitch = .7f;
                }

                if (!footstepSound.isPlaying)
                {
                    footstepSound.Play();
                }
            }
        }
        else
        {
            if (footstepSound.isPlaying)
            {
                footstepSound.Pause();
            }
        }
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
        Player player = GetComponent<Player>();

        if (!Player.pause && player.moveable/* && Player.platform == "Mobile"*/)
        {
            crouch = !crouch;
            CrouchCheck();
        }
        else if (!player.moveable)
        {
            Stand();
        }
    }

    private void Crouch()
    {
        if (velocity.x < 0 || velocity.x > 0 || velocity.z < 0 || velocity.z > 0)
        {
            GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crouch Walk V2");
        }
        else
        {
            GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crouch");
        }
        speed = crouchSpeed;
        GameObject playerView = GameObject.FindGameObjectWithTag("MainCamera");
        playerView.transform.localPosition = new Vector3(playerView.transform.localPosition.x, cameraHeight / 2, playerView.transform.localPosition.z);
    }

    private void Walk()
    {
        if (velocity.x < 0 || velocity.x > 0 || velocity.z < 0 || velocity.z > 0)
        {
            GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Walk");
        }
        else
        {
            GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Stand");
        }
        speed = walkSpeed;
        GameObject playerView = GameObject.FindGameObjectWithTag("MainCamera");
        playerView.transform.localPosition = new Vector3(playerView.transform.localPosition.x, cameraHeight, playerView.transform.localPosition.z);
    }

    private void Sprint()
    {
        GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprint");
        speed = sprintSpeed;
        if (GameObject.FindGameObjectWithTag("Sprint") == null)
        {
            Instantiate(Resources.Load<GameObject>("Sprint/Sprint"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
        }
    }
}
