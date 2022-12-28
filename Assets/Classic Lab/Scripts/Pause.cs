using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Pause : MonoBehaviour
{

    private Database db;

    private void Start()
    {
        db = Database.db;
    }

    public void OpenPauseInterface()
    {
        if (!Player.pause)
        {
            Player.Pause();
            GameObject pauseInterface = Instantiate(Resources.Load<GameObject>("UI/Pause Interface"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
            int sensitivityPercentage = 0;
            if (Player.platform == "Desktop")
            {
                sensitivityPercentage = db.desktopSensitivity / 10;
            }
            else if (Player.platform == "Mobile")
            {
                sensitivityPercentage = db.mobileSensitivity;
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
        int sensitivity = 0;
        int sensitivityPercentage = 0;
        if (Player.platform == "Desktop")
        {
            sensitivity = Convert.ToInt32(Math.Floor(value) * 10);
            sensitivityPercentage = sensitivity / 10;
            db.desktopSensitivity = sensitivity;
        }
        else if (Player.platform == "Mobile")
        {
            sensitivity = Convert.ToInt32(Math.Floor(value));
            sensitivityPercentage = sensitivity;
            db.mobileSensitivity = sensitivity;
        }
        GameObject.FindGameObjectWithTag("Sensitivity").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Sensitivity:\n{sensitivityPercentage}%";
    }

    private void MainMenu()
    {
        Database.Save();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    private void Quit()
    {
        Database.Save();
        Application.Quit();
    }
}
