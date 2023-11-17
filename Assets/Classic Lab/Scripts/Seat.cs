using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool isPlayerInTriggerArea = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerInTriggerArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerInTriggerArea = false;
        }
    }
}
