using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementConstructor : MonoBehaviour
{
    private Database db;
    private int proton = 0;
    private int electron = 0;
    private int neutron = 0;
    
    private void Start()
    {
        db = Database.db;
    }

    public void Add()
    {
        string particleName = gameObject.transform.parent.name;
        GameObject.Find($"Element Constructor/{particleName}/Slider").GetComponent<Slider>().value += 1;
    }

    public void Remove()
    {
        string particleName = gameObject.transform.parent.name;
        GameObject.Find($"Element Constructor/{particleName}/Slider").GetComponent<Slider>().value -= 1;
    }
}
