using System.Collections;
using UnityEngine;
using TMPro;

public class Controls : MonoBehaviour
{
    public Gradient gradient;
    private bool show = false;
    private bool switchingKey = false;
    private float switchTime = 1f;

    private void Start()
    {
        Visibility();
    }

    private void Update()
    {
        float gradientValue = GameObject.FindGameObjectWithTag("Guide").GetComponent<Guide>().slider;
        gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = gradient.Evaluate(gradientValue);
        gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = gradient.Evaluate(gradientValue);
        if (!Player.pause)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                show = !show;
                Visibility();
            }
        }
        if (show)
        {
            gameObject.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color = gradient.Evaluate(gradientValue);
        }
        if (show && !switchingKey)
        {
            StartCoroutine(SwitchKey());
        }
    }

    private IEnumerator SwitchKey()
    {
        switchingKey = true;
        string[] before = new string[] { "W", "W", "A", "S", "D" };
        int[] after = new int[] { 0, 0, -90, 180, 90 };
        for (int i = 0; i < before.Length; i++)
        {
            Transform key = gameObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(0);
            key.GetComponent<TextMeshProUGUI>().text = before[i];
            key.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
        }
        yield return new WaitForSeconds(switchTime);
        for (int i = 0; i < after.Length; i++)
        {
            Transform key = gameObject.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetChild(0);
            key.GetComponent<TextMeshProUGUI>().text = "▲";
            key.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, after[i]);
        }
        yield return new WaitForSeconds(switchTime);
        switchingKey = false;
    }

    private void Visibility()
    {
        if (show)
        {
            gameObject.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
            gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Hide Controls";
        }
        else
        {
            gameObject.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0;
            gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Show Controls";
        }
    }
}
