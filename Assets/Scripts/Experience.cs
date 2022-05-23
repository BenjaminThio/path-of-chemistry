using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Experience : MonoBehaviour
{
    private Database db;
    public int addExp = 1;
    public Gradient gradient;

    private void Start()
    {
        db = Database.db;
        CheckExp();
    }

    public void AddExpButton()
    {
        StartCoroutine(AddExp(addExp));
    }

    private IEnumerator AddExp(int quantity)
    {
        Slider expBar = GameObject.Find("Experience/Bar").GetComponent<Slider>();
        for (int i = 0; i < quantity; i++)
        {
            yield return new WaitForSeconds(.3f);
            db.exp += 1;
            if (db.exp == expBar.maxValue)
            {
                yield return new WaitForSeconds(.3f);
                db.exp = 0;
                db.expLevel += 1;
            }

            CheckExp();
        } 
    }

    private void CheckExp()
    {
        Slider expBar = GameObject.Find("Experience/Bar").GetComponent<Slider>();
        if (db.expLevel > 0)
        {
            GameObject.Find("Experience/EXP Level").GetComponent<TextMeshProUGUI>().text = Convert.ToString(db.expLevel);
        }
        else
        {
            GameObject.Find("Experience/EXP Level").GetComponent<TextMeshProUGUI>().text = "";
        }
        expBar.maxValue = (db.expLevel + 1) * 3;
        expBar.value = db.exp;
        GameObject.Find("Experience/Bar/Fill Area/Fill").GetComponent<Image>().color = gradient.Evaluate(expBar.normalizedValue);
    }
}
