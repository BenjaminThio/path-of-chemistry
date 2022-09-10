using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int tapTimes;

    IEnumerator ResetTapTimes()
    {
        yield return new WaitForSeconds(.5f);
        if (tapTimes < 2)
        {
            tapTimes = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            tapTimes++;
            if (tapTimes == 1)
            {
                StartCoroutine(ResetTapTimes());
            }
        }
        if (tapTimes >= 2)
        {
            if (Input.GetKey(KeyCode.W))
            {
                print("Running");
            }
            else
            {
                tapTimes = 0;
            }
        }
    }
}
