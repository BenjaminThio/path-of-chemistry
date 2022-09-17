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

public class PlayerMovement : MonoBehaviour
{
    //private int tapTimes;
    private bool isGrounded;
    //private bool crouch = false;
    private bool isCrouching = false;
    private bool holding = false;
    private bool jump = false;
    private float speed = 15f;
    private float gravity = -58.86f;
    private float walkSpeed = 15f;
    private float sprintSpeed = 30f;
    private float crouchSpeed = 5f;
    private float jumpHeight = 5f;
    private float groundDistance = 0.5f;
    //private float resetTimer = .3f;
    private float normalCameraHeight = 5f;
    private Vector3 velocity;
    public LayerMask groundMask;
    public Transform groundCheck;
    public CharacterController controller;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Action").GetComponent<Button>().onClick.AddListener(SwitchAction);
        GameObject.FindGameObjectWithTag("Jump").GetComponent<EventTrigger>().AddListener(EventTriggerType.PointerDown, Hold);
        GameObject.FindGameObjectWithTag("Jump").GetComponent<EventTrigger>().AddListener(EventTriggerType.PointerUp, Unhold);
    }

    private void Update()
    {
        if (!Player.pause)
        {
            /*
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    crouch = true;
                    Crouch();
                }
                if (crouch)
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        crouch = false;
                        Walk();
                    }
                }
            if (!isCrouching)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    tapTimes++;
                    if (tapTimes == 1)
                    {
                        StartCoroutine(ResetTapTimes());
                    }
                    if (tapTimes == 2)
                    {
                        speed = sprintSpeed;
                        if (GameObject.FindGameObjectWithTag("Sprint") == null)
                        {
                            Instantiate(Resources.Load<GameObject>("Sprint/Sprint"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
                        }
                    }
                }
                if (tapTimes >= 2)
                {
                    if (!Input.GetKey(KeyCode.W))
                    {
                        speed = walkSpeed;
                        DisableSprint();
                    }
                }
            }
            */
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            float x = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>().Horizontal;
            float z = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>().Vertical;
            if (!isCrouching)
            {
                if (z >= .95f)
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
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
            if (holding && isGrounded)
            {
                jump = true;
            }
            if (jump && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jump = false;
            }
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    /*private void DisableSprint()
    {
        tapTimes = 0;
        if (GameObject.FindGameObjectWithTag("Sprint") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Sprint"));
        }
    }*/

    public void SwitchAction()
    {
        isCrouching = !isCrouching;
        if (isCrouching)
        {
            Crouch();
        }
        else
        {
            Walk();
        }
    }

    private void Crouch()
    {
        GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crouch");
        //DisableSprint();
        speed = crouchSpeed;
        gravity = -98.2f;
        GameObject playerView = GameObject.FindGameObjectWithTag("MainCamera");
        playerView.transform.localPosition = new Vector3(playerView.transform.localPosition.x, normalCameraHeight / 2, playerView.transform.localPosition.z);
    }

    private void Walk()
    {
        GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Stand");
        speed = walkSpeed;
        gravity = -58.86f;
        GameObject playerView = GameObject.FindGameObjectWithTag("MainCamera");
        playerView.transform.localPosition = new Vector3(playerView.transform.localPosition.x, normalCameraHeight, playerView.transform.localPosition.z);
    }

    private void Sprint()
    {
        speed = sprintSpeed;
        if (GameObject.FindGameObjectWithTag("Sprint") == null)
        {
            Instantiate(Resources.Load<GameObject>("Sprint/Sprint"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
        }
    }

    private void Hold(PointerEventData eventData)
    {
        holding = true;
    }

    private void Unhold(PointerEventData eventData)
    {
        holding = false;
    }


    /*IEnumerator ResetTapTimes()
    {
        yield return new WaitForSeconds(resetTimer);
        if (tapTimes < 2)
        {
            tapTimes = 0;
        }
    }*/
}
