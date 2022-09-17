using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void LateUpdate()
    {
        Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        transform.LookAt(transform.position + cam.forward);
    }
}
