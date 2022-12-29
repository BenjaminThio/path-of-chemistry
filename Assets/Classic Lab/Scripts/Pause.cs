using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class Pause : MonoBehaviour
{
    public Gradient gradient;

    private Database db;

    private void Start()
    {
        db = Database.db;
    }

    /*
    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Pause Interface") != null)
        {
            float gradientValue = GameObject.FindGameObjectWithTag("Guide").GetComponent<Guide>().slider;
            GameObject.FindGameObjectWithTag("Sensitivity").transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = gradient.Evaluate(gradientValue);
        }    
    }
    */

    public void OpenPauseInterface()
    {
        if (!Player.pause)
        {
            Player.Pause();
            QuantityHandler.pause = true;
            GameObject pauseInterface = Instantiate(Resources.Load<GameObject>("UI/Pause Interface"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
            int sensitivityPercentage = 0;
            if (Player.platform == "Desktop")
            {
                pauseInterface.transform.GetChild(1).GetChild(1).GetComponent<Slider>().minValue = 1;
                sensitivityPercentage = db.desktopSensitivity / 10;
            }
            else if (Player.platform == "Mobile")
            {
                pauseInterface.transform.GetChild(1).GetChild(1).GetComponent<Slider>().minValue = 2;
                sensitivityPercentage = db.mobileSensitivity * 2;
            }
            pauseInterface.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Sensitivity:\n{sensitivityPercentage}%";
            pauseInterface.transform.GetChild(1).GetChild(1).GetComponent<Slider>().value = sensitivityPercentage;
            pauseInterface.transform.GetChild(1).GetChild(1).GetComponent<Slider>().onValueChanged.AddListener(OnSensitivityValueChanged);
            pauseInterface.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(MainMenu);
            pauseInterface.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Quit);
        }
    }

    private void OnSensitivityValueChanged(float value)
    {
        int sensitivityPercentage = 0;
        if (Player.platform == "Desktop")
        {
            int sensitivity = Convert.ToInt32(Math.Floor(value) * 10);
            sensitivityPercentage = sensitivity / 10;
            db.desktopSensitivity = sensitivity;
        }
        else if (Player.platform == "Mobile")
        {
            int sensitivity = Convert.ToInt32(Math.Floor(value) / 2);
            sensitivityPercentage = sensitivity * 2;
            db.mobileSensitivity = sensitivity;
        }
        GameObject.FindGameObjectWithTag("Sensitivity").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Sensitivity:\n{sensitivityPercentage}%";
    }

    private void MainMenu()
    {
        foreach (string particleName in ElementConstructor.particles.Keys.ToArray())
        {
            ElementConstructor.particles[particleName] = 0;
        }
        ElementConstructor.constructedElement = null;
        Alert.messages.Clear();
        Player.runOnce = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Database.Save();
        QuantityHandler.pause = false;
        SceneManager.LoadScene(0);
    }

    private void Quit()
    {
        Database.Save();
        QuantityHandler.pause = false;
        Application.Quit();
    }
}
