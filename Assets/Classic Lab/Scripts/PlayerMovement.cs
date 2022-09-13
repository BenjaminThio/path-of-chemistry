using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int tapTimes;
    private bool isGrounded;
    private bool crouch = false;
    private float speed = 15f;
    private float gravity = -58.86f;
    private float walkSpeed = 15f;
    private float sprintSpeed = 30f;
    private float crouchSpeed = 5f;
    private float jumpHeight = 5f;
    private float groundDistance = 0.5f;
    private float resetTimer = .3f;
    private float normalCameraHeight = 5f;
    private Vector3 velocity;
    public LayerMask groundMask;
    public Transform groundCheck;
    public CharacterController controller;

    IEnumerator ResetTapTimes()
    {
        yield return new WaitForSeconds(resetTimer);
        if (tapTimes < 2)
        {
            tapTimes = 0;
        }
    }

    void Update()
    {
        if (!Player.pause)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                crouch = true;
                DisableSprint();
                speed = crouchSpeed;
                gravity = -98.2f;
                GameObject playerView = GameObject.FindGameObjectWithTag("MainCamera");
                playerView.transform.localPosition = new Vector3(playerView.transform.localPosition.x, normalCameraHeight / 2, playerView.transform.localPosition.z);
            }
            if (crouch)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    crouch = false;
                    speed = walkSpeed;
                    gravity = -58.86f;
                    GameObject playerView = GameObject.FindGameObjectWithTag("MainCamera");
                    playerView.transform.localPosition = new Vector3(playerView.transform.localPosition.x, normalCameraHeight, playerView.transform.localPosition.z);
                }
            }
            if (!crouch)
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
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void DisableSprint()
    {
        tapTimes = 0;
        if (GameObject.FindGameObjectWithTag("Sprint") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Sprint"));
        }
    }
}
