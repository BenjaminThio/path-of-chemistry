//using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static bool menuPause = false;
    private Database db;
    
    private void Start()
    {
        db = Database.db;
        //print($"{Application.persistentDataPath}/Path Of Chemistry/Data/Saves.json");

        GameObject.FindGameObjectWithTag("Reaction").GetComponent<Reaction>().ResetFlaskLiquidFillValue();
        StartCoroutine(FadeIn());
        AddFunctionToMenuUI();
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>() != null)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(6, 5.09707f, 72);
        }
    }

    private IEnumerator FadeIn()
    {
        GameObject light = Instantiate(Resources.Load<GameObject>("UI/Light Reverse"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);

        light.GetComponent<Animator>().SetFloat("SpeedMultiplier", 2f);
        yield return new WaitForSeconds(1.5f);
        Destroy(light);
        /*
        yield return new WaitForSeconds(1.2f);
        light.GetComponent<Image>().raycastTarget = false;
        yield return new WaitForSeconds(.3f);
        */
    }

    /*
    private void NewGame()
    {
        string filePath = $"{Application.persistentDataPath}/Path Of Chemistry/Data/Saves.json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Database.Save();
        }
        Play();
    }
    */

    private void Play()
    {
        if (!menuPause)
        {
            StartCoroutine(IntoGame());
        }
    }

    private void NewGame()
    {
        if (!menuPause)
        {
            Reset();
            StartCoroutine(IntoGame());
        }
    }

    private void Reset()
    {
        db.played = true;
        db.level = 1;
        db.expLevel = 0;
        db.exp = 0;
        db.slotNum = 1;
        db.hotbarItem = new Dictionary<string, object>[]{
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            };
        db.flaskItem = new Dictionary<string, object>[]{
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            };
        db.compoundCreatorItem = new Dictionary<string, object>[]{
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            };
        db.compoundReducerOriginalItem = new Dictionary<string, object>[]{
                null
            };
        db.compoundReducerReducedItem = new Dictionary<string, object>[]{
                null,
                null,
                null,
                null,
                null
            };
        Database.Save();
        GameObject.FindGameObjectWithTag("Rack").GetComponent<FillLiquidToCylinderBeaker>().FillLiquid();
    }

    private IEnumerator IntoGame()
    {
        Destroy(GameObject.FindGameObjectWithTag("Menu UI"));
        Destroy(GameObject.FindGameObjectWithTag("Hyperlinks"));
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>() != null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = true;
            yield return new WaitForSeconds(5f);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = false;
        }
        GameObject inGameCanvas = Instantiate(Resources.Load<GameObject>("Canvas/InGame Canvas"));
        inGameCanvas.name = "Canvas";
        if (Player.platform == "Desktop")
        {
            Instantiate(Resources.Load<GameObject>("UI/Controls Panel"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
            GameObject.FindGameObjectWithTag("Pause").GetComponent<Image>().enabled = false;
            GameObject.FindGameObjectWithTag("Pause").GetComponent<Button>().enabled = false;
            GameObject.FindGameObjectWithTag("Alert Button").GetComponent<Button>().enabled = false;
            GameObject.FindGameObjectWithTag("Action").GetComponent<Button>().enabled = false;
        }
        else if (Player.platform == "Mobile")
        {
            Instantiate(Resources.Load<GameObject>("Joystick/Fixed Joystick"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().enabled = true;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = true;
        Destroy(GameObject.FindGameObjectWithTag("Menu Canvas"));
        Player.pause = false;
    }

    private void Credits()
    {
        if (!menuPause)
        {
            Destroy(GameObject.FindGameObjectWithTag("Menu UI"));
            Destroy(GameObject.FindGameObjectWithTag("Hyperlinks"));
            Instantiate(Resources.Load<GameObject>("UI/Credits Background"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
            GameObject.FindGameObjectWithTag("Back").GetComponent<Button>().onClick.AddListener(Back);
        }
    }

    private void Back()
    {
        Destroy(GameObject.FindGameObjectWithTag("Credits Background"));
        Instantiate(Resources.Load<GameObject>("UI/Menu UI"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
        Instantiate(Resources.Load<GameObject>("UI/Hyperlinks"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
        AddFunctionToMenuUI();
    }

    private void Manual()
    {
        if (!menuPause)
        {
            Destroy(GameObject.FindGameObjectWithTag("Menu UI"));
            Destroy(GameObject.FindGameObjectWithTag("Hyperlinks"));
            Instantiate(Resources.Load<GameObject>("UI/Manual Interface"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
        }
    }

    private void Quit()
    {
        if (!menuPause)
        {
            Application.Quit();
        }
    }

    private void Settings()
    {
        if (!menuPause)
        {
            menuPause = true;
            GameObject settingsInterface = Instantiate(Resources.Load<GameObject>("UI/Settings Interface"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
            Transform sensitivityGroup = settingsInterface.transform.GetChild(2).GetChild(0);
            TextMeshProUGUI sensitivityText = sensitivityGroup.GetChild(0).GetComponent<TextMeshProUGUI>();
            Slider sensitivitySlider = sensitivityGroup.GetChild(1).GetComponent<Slider>();
            Button closeButton = settingsInterface.transform.GetChild(1).GetComponent<Button>();

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

            sensitivityText.text = $"Sensitivity:\n{sensitivityPercentage}%";
            sensitivitySlider.value = sensitivityPercentage;
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityValueChanged);
            closeButton.onClick.AddListener(CloseSettingsInterface);
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

    public void CloseSettingsInterface()
    {
        Database.Save();
        menuPause = false;
        Destroy(GameObject.FindGameObjectWithTag("Settings Interface"));
    }

    public void AddFunctionToMenuUI()
    {
        string filePath = $"{Application.persistentDataPath}/Path Of Chemistry/Data/Saves.json";
        /*
        if (File.Exists(filePath))
        {
            GameObject playButton = Instantiate(Resources.Load<GameObject>("UI/Play"), GameObject.FindGameObjectWithTag("UI Buttons").transform, false);
            playButton.transform.SetAsFirstSibling();
        }
        if (GameObject.FindGameObjectWithTag("Play") != null)
        {
            GameObject.FindGameObjectWithTag("Play").GetComponent<Button>().onClick.AddListener(Play);
        }
        GameObject.FindGameObjectWithTag("New Game").GetComponent<Button>().onClick.AddListener(NewGame);
        */
        if (db.played)
        {
            GameObject play = Instantiate(Resources.Load<GameObject>("UI/Play"), GameObject.FindGameObjectWithTag("UI Buttons").transform);
            play.transform.SetAsFirstSibling();
            GameObject.FindGameObjectWithTag("Play").GetComponent<Button>().onClick.AddListener(Play);            
        }
        GameObject.FindGameObjectWithTag("New Game").GetComponent<Button>().onClick.AddListener(NewGame);
        GameObject.FindGameObjectWithTag("Manual").GetComponent<Button>().onClick.AddListener(Manual);
        GameObject.FindGameObjectWithTag("Settings").GetComponent<Button>().onClick.AddListener(Settings);
        GameObject.FindGameObjectWithTag("Credits").GetComponent<Button>().onClick.AddListener(Credits);
        GameObject.FindGameObjectWithTag("Quit").GetComponent<Button>().onClick.AddListener(Quit);
    }
}
