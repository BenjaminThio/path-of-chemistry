using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        print("IN");
    }

    /*
    public int maxFillTimes = 8;
    public float transitionDurationPerFill = 2.0f;
    public float minFillValue = -1f;
    public float maxFillValue = 1.3f;

    private void Start()
    {
        GetComponent<Renderer>().sharedMaterial.SetFloat("Fill", minFillValue);
        StartCoroutine(playFillAnimation());
    }

    IEnumerator playFillAnimation()
    {
        yield return new WaitForSeconds(2f);

        Fill(8);
    }

    public void Fill(int fillTimes)
    {
        LeanTween.value(gameObject, minFillValue * GetScaleMultiplication(), maxFillValue * GetScaleMultiplication() * fillTimes / maxFillTimes, fillTimes * transitionDurationPerFill).setOnUpdate(
            (float value) => {
                GetComponent<Renderer>().material.SetFloat("Fill", value);
            }
        );
    }

    private float GetScaleMultiplication()
    {
        Transform flask = transform.parent;
        Transform reaction = flask.parent;

        return flask.localScale.y * reaction.localScale.y;
    }
    */
}
