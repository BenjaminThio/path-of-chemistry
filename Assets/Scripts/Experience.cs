using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Experience : MonoBehaviour
{
    public int exp = 1;
    private int expLevel = 0;
    private void Awake()
    {
        CheckExp();
    }

    public void AddExpButton()
    {
        StartCoroutine(AddExp(exp));
    }

    private IEnumerator AddExp(int quantity)
    {
        Slider expBar = GameObject.Find("Experience/Bar").GetComponent<Slider>();
        for (int i = 0; i < quantity; i++)
        {
            yield return new WaitForSeconds(.3f);
            expBar.value += 1;
            if (expBar.value == expBar.maxValue)
            {
                yield return new WaitForSeconds(.3f);
                expBar.value = 0;
                expLevel += 1;
            }
            CheckExp();
        } 
    }

    private void CheckExp()
    {
        GameObject.Find("Experience/Level").GetComponent<TextMeshProUGUI>().text = Convert.ToString(expLevel);
        GameObject.Find("Experience/Bar").GetComponent<Slider>().maxValue = (expLevel + 1) * 3;
    }
}
