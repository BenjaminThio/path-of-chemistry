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

    private void Start()
    {
        db = Database.db;
        UpdateSpecificParticle(gameObject.transform.parent.name);
    }

    public void UpdateParticles(float value)
    {
        string particleName = gameObject.transform.parent.name;
        particles[particleName] = Convert.ToInt32(Mathf.Floor(value));
        UpdateSpecificParticle(particleName);
    }

    public void AddOrRemove()
    {
        string particleName = gameObject.transform.parent.name;
        if (gameObject.name == "Add")
        {
            if (particles[particleName] + 1 <= 118)
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
        UpdateSpecificParticle(particleName);
    }

    private void UpdateSpecificParticle(string particleName)
    {
        GameObject.Find($"Element Constructor/{particleName}/Input").GetComponent<TMP_InputField>().text = Convert.ToString(particles[particleName]);
    }
}
