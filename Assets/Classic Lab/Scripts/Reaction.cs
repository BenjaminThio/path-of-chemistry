using System.Collections;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    private float animationLength = 7f;

    public void PlayPourAnimation()
    {
        Instantiate(Resources.Load<GameObject>("Animations/Flask_Pour"), GameObject.FindGameObjectWithTag("Reaction").transform, false);
        StartCoroutine(WaitAnimationFinish(animationLength));
    }

    private IEnumerator WaitAnimationFinish(float waitTime)
    {
        GameObject flaskPour = GameObject.FindGameObjectWithTag("Flask_Pour");
        GameObject flaskFill = GameObject.FindGameObjectWithTag("Flask");
        flaskPour.layer = LayerMask.NameToLayer("Uninteractable");
        flaskFill.layer = LayerMask.NameToLayer("Uninteractable");
        flaskPour.GetComponent<Animator>().SetTrigger("Pour");
        flaskFill.GetComponent<Animator>().SetTrigger("Fill");
        yield return new WaitForSeconds(waitTime);
        Destroy(flaskPour);
    }
}
