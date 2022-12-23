using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Experience : MonoBehaviour
{
    private Database db;
    private int exp;
    private int expLevel;
    private bool isAddingExp = false;
    public Gradient gradient;
    public List<int> expQueues = new List<int>();

    private void Start()
    {
        db = Database.db;
        exp = db.exp; 
        expLevel = db.expLevel;
        CheckExp();
    }

    private void Update()
    {
        if (expQueues.Count > 0 && !isAddingExp)
        {
            StartAddExpCoroutine(expQueues[0]);
            expQueues.RemoveAt(0);
        }
    }

    private void StartAddExpCoroutine(int quantity)
    {
        StartCoroutine(GameObject.FindGameObjectWithTag("Experience").GetComponent<Experience>().AddExp(quantity));
    }

    private IEnumerator AddExp(int quantity)
    {
        isAddingExp = true;
        Slider expBar = GameObject.Find("Experience/Bar").GetComponent<Slider>();
        for (int i = 0; i < quantity; i++)
        {
            yield return new WaitForSeconds(.3f);
            exp += 1;
            if (exp == expBar.maxValue)
            {
                yield return new WaitForSeconds(.3f);
                exp = 0;
                expLevel += 1;
            }

            CheckExp();
        }
        isAddingExp = false;
    }

    private void CheckExp()
    {
        Slider expBar = GameObject.Find("Experience/Bar").GetComponent<Slider>();
        if (expLevel > 0)
        {
            GameObject.Find("Experience/EXP Level").GetComponent<TextMeshProUGUI>().text = Convert.ToString(expLevel);
        }
        else
        {
            GameObject.Find("Experience/EXP Level").GetComponent<TextMeshProUGUI>().text = "";
        }
        expBar.maxValue = (expLevel + 1) * 3;
        expBar.value = exp;
        GameObject.Find("Experience/Bar/Fill Area/Fill").GetComponent<Image>().color = gradient.Evaluate(expBar.normalizedValue);
    }

    public void AddExpToQueue(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            db.exp += 1;
            if (db.exp == (db.expLevel + 1) * 3)
            {
                db.exp = 0;
                db.expLevel += 1;
            }
        }
        expQueues.Add(quantity);
    }
}
