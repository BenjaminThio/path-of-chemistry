using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ElementConstructor : MonoBehaviour
{
    private static Dictionary<string, int> particles = new Dictionary<string, int>()
    {
        {"Proton", 1},
        {"Electron", 1},
        {"Neutron", 0}
    };
    private readonly Dictionary<string, int> particlesMaxValue = new Dictionary<string, int>()
    {
        {"Proton", 120},
        {"Electron", 120},
        {"Neutron", 180}
    };
    public static string constructedElement;

    private void Start()
    {
        string particleName = gameObject.name;
        GameObject.Find($"Element Constructor/{particleName}/Slider").GetComponent<Slider>().maxValue = particlesMaxValue[particleName];
        GameObject.Find($"Element Constructor/{particleName}/Add").GetComponent<Button>().onClick.AddListener(AddOrRemove);
        GameObject.Find($"Element Constructor/{particleName}/Remove").GetComponent<Button>().onClick.AddListener(AddOrRemove);
        AddSliderListener(false);
        AddInputListener(false);
        UpdatePreview();
    }

    private void AddSliderListener(bool requireRemoveAllListeners)
    {
        string particleName = gameObject.name;
        Slider slider = GameObject.Find($"Element Constructor/{particleName}/Slider").GetComponent<Slider>();
        if (requireRemoveAllListeners)
        {
            slider.onValueChanged.RemoveAllListeners();
        }
        slider.value = particles[particleName];
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        string particleName = gameObject.name;
        particles[particleName] = Convert.ToInt32(Mathf.Floor(value));
        AddInputListener(true);
        UpdatePreview();
    }

    private void AddInputListener(bool requireRemoveAllListeners)
    {
        string particleName = gameObject.name; 
        TMP_InputField input = GameObject.Find($"Element Constructor/{particleName}/Input").GetComponent<TMP_InputField>();
        if (requireRemoveAllListeners)
        {
            input.onValueChanged.RemoveAllListeners();
        }
        input.text = Convert.ToString(particles[particleName]);
        input.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void OnInputValueChanged(string value)
    {
        string particleName = gameObject.name;
        if (!Global.IsDigit(value))
        {
            UpdateParticle(Global.Digitize(value));
            if (value == "")
            {
                GameObject.Find($"Element Constructor/{particleName}/Input").GetComponent<TMP_InputField>().MoveTextEnd(true);
            }
        }
        else if (Convert.ToInt64(value) > particlesMaxValue[particleName])
        {
            UpdateParticle(particlesMaxValue[particleName]);
        }
        else
        {
            UpdateParticle(Convert.ToInt32(value));
        }
    }

    private void UpdateParticle(int value)
    {
        string particleName = gameObject.name;
        particles[particleName] = value;
        AddSliderListener(true);
        AddInputListener(true);
        UpdatePreview();
    }

    public void AddOrRemove()
    {
        string particleName = gameObject.name;
        string buttonPressedName = EventSystem.current.currentSelectedGameObject.name;
        if (buttonPressedName == "Add")
        {
            if (particles[particleName] + 1 <= particlesMaxValue[particleName])
            {
                particles[particleName] += 1;
            }
        }
        else if (buttonPressedName == "Remove")
        {
            if (particles[particleName] - 1 >= 0)
            {
                particles[particleName] -= 1;
            }
        }
        AddSliderListener(true);
        AddInputListener(true);
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        Image preview = GameObject.Find("Preview/Item").GetComponent<Image>();
        foreach (Dictionary<string, object> element in ReadOnly.elements)
        {
            if (particles["Proton"] == Convert.ToInt32(element["protons"]) && particles["Electron"] == Convert.ToInt32(element["electrons"]) && particles["Neutron"] == Convert.ToInt32(element["neutrons"]))
            {
                constructedElement = Convert.ToString(element["symbol"]);
                preview.sprite = Resources.Load<Sprite>($"Elements/{constructedElement}");
                return;
            }
            else
            {
                constructedElement = null;
                preview.sprite = Resources.Load<Sprite>("None");
            }
        }
    }
}
