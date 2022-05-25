using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementConstructor : MonoBehaviour
{
    private Database db;
    private static Dictionary<string, int> particles = new Dictionary<string, int>()
    {
        {"Proton", 0},
        {"Electron", 0},
        {"Neutron", 0}
    };
    private readonly Dictionary<string, int> particlesMaxValue = new Dictionary<string, int>()
    {
        {"Proton", 120},
        {"Electron", 120},
        {"Neutron", 180}
    };

    private void Start()
    {
        db = Database.db;
        string particleName = gameObject.transform.parent.name;
        GameObject.Find($"Element Constructor/{particleName}/Slider").GetComponent<Slider>().maxValue = particlesMaxValue[particleName];
        GameObject.Find($"Element Constructor/{particleName}/Input").GetComponent<TMP_InputField>().text = Convert.ToString(particles[particleName]);
    }

    public void OnInputChanged(string value)
    {
        string particleName = gameObject.transform.parent.name;
        if (!Global.IsDigit(value))
        {
            UpdateParticle(Global.Digitize(value));
            GameObject.Find($"Element Constructor/{particleName}/Input").GetComponent<TMP_InputField>().MoveTextEnd(true);
            return;
        }
        else if (Convert.ToInt64(value) > particlesMaxValue[particleName])
        {
            UpdateParticle(particlesMaxValue[particleName]);
            return;
        }
        UpdateParticle(Convert.ToInt32(value));
    }

    public void OnSliderValueChanged(float value)
    {
        string particleName = gameObject.transform.parent.name;
        particles[particleName] = Convert.ToInt32(Mathf.Floor(value));
        GameObject.Find($"Element Constructor/{particleName}/Input").GetComponent<TMP_InputField>().text = Convert.ToString(particles[particleName]);
    }

    public void AddOrRemove()
    {
        string particleName = gameObject.transform.parent.name;
        if (gameObject.name == "Add")
        {
            if (particles[particleName] + 1 <= particlesMaxValue[particleName])
            {
                particles[particleName] += 1;
            }
        }
        else if (gameObject.name == "Remove")
        {
            if (particles[particleName] - 1 >= 0)
            {
                particles[particleName] -= 1;
            }
        }
        GameObject.Find($"Element Constructor/{particleName}/Slider").GetComponent<Slider>().value = particles[particleName];
    }

    private void UpdateParticle(int value)
    {
        string particleName = gameObject.transform.parent.name;
        particles[particleName] = value;
        GameObject.Find($"Element Constructor/{particleName}/Slider").GetComponent<Slider>().value = particles[particleName];
        GameObject.Find($"Element Constructor/{particleName}/Input").GetComponent<TMP_InputField>().text = Convert.ToString(particles[particleName]);
    }
}
