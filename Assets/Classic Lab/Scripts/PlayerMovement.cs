using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 velocity;
    private bool isGrounded;
    public CharacterController controller;
    public float speed = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 5f;
    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;
    public int tapTimes;
    public bool speedUp;
    public float walkSpeed = 15f;
    public float sprintSpeed = 30f;
    public float resetTimer = .3f;

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
            }
        }
        if (tapTimes >= 2)
        {
            if (!Input.GetKey(KeyCode.W))
            {
                speed = walkSpeed;
                tapTimes = 0;
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
