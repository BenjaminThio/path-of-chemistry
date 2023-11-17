using System;
using System.Collections;
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
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Pause();
            QuantityHandler.pause = true;
            GameObject pauseInterface = Instantiate(Resources.Load<GameObject>("UI/Pause Interface"), GameObject.FindGameObjectWithTag("Canvas").transform, false);

            float maxSensitivity;

            if (Player.platform == "Desktop")
            {
                maxSensitivity = 600f;

            }
            else
            {
                maxSensitivity = 25f;
            }

            float sensitivity;

            if (Player.platform == "Desktop")
            {
                sensitivity = db.desktopSensitivity;
            }
            else
            {
                sensitivity = db.mobileSensitivity;
            }

            int sensitivityPercentage = Convert.ToInt32(sensitivity / maxSensitivity * 100f);

            pauseInterface.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Sensitivity:\n{sensitivityPercentage}%";
            pauseInterface.transform.GetChild(1).GetChild(1).GetComponent<Slider>().value = sensitivityPercentage;
            pauseInterface.transform.GetChild(1).GetChild(1).GetComponent<Slider>().onValueChanged.AddListener(OnSensitivityValueChanged);
            pauseInterface.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(MainMenu);
            pauseInterface.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Quit);
        }
    }

    private void OnSensitivityValueChanged(float value)
    {
        float maxSensitivity;

        if (Player.platform == "Desktop")
        {
            maxSensitivity = 600f;

        }
        else
        {
            maxSensitivity = 25f;
        }

        float sensitivityPerUnit = maxSensitivity / 100f;
        float sensitivity = sensitivityPerUnit * value;
        int sensitivityPercentage = Convert.ToInt32(sensitivity / maxSensitivity * 100f);

        if (Player.platform == "Desktop")
        {
            db.desktopSensitivity = sensitivity;

        }
        else if (Player.platform == "Mobile")
        {
            db.mobileSensitivity = sensitivity;
        }

        GameObject.FindGameObjectWithTag("Sensitivity").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Sensitivity:\n{sensitivityPercentage}%";
    }

    private void MainMenu()
    {
        StartCoroutine(FadeOutAndMainMenu());
    }

    private void Quit()
    {
        Database.Save();
        QuantityHandler.pause = false;
        Application.Quit();
    }

    private IEnumerator FadeOutAndMainMenu()
    {
        GameObject light = Instantiate(Resources.Load<GameObject>("UI/Light"), GameObject.FindGameObjectWithTag("Canvas").transform, false);

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
        light.GetComponent<Animator>().SetFloat("SpeedMultiplier", 2f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(db.labIndex + 1);
    }
}
