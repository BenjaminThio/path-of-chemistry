
using UnityEngine;

public class Experiment : MonoBehaviour
{
    public void React()
    {
        GameObject.FindGameObjectWithTag("Reaction").GetComponent<Reaction>().PlayPourAnimation();
    }
}
