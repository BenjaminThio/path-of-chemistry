using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static bool menuPause = false;
    
    private void Start()
    {
        //print($"{Application.persistentDataPath}/Path Of Chemistry/Data/Saves.json");

        //StartCoroutine(FadeIn());
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
    }

    private void NewGame()
    {
        if (!menuPause)
        {
            StartCoroutine(IntoGame());
        }
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
        Destroy(GameObject.FindGameObjectWithTag("Menu Canvas"));
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
            Button closeButton = settingsInterface.transform.GetChild(1).GetComponent<Button>();
            closeButton.onClick.AddListener(CloseSettingsInterface);
        }
    }

    public void CloseSettingsInterface()
    {
        Database.Save();
        menuPause = false;
        Destroy(GameObject.FindGameObjectWithTag("Settings Interface"));
    }

    public void AddFunctionToMenuUI()
    {
        GameObject.FindGameObjectWithTag("New Game").GetComponent<Button>().onClick.AddListener(NewGame);
        GameObject.FindGameObjectWithTag("Manual").GetComponent<Button>().onClick.AddListener(Manual);
        GameObject.FindGameObjectWithTag("Settings").GetComponent<Button>().onClick.AddListener(Settings);
        GameObject.FindGameObjectWithTag("Credits").GetComponent<Button>().onClick.AddListener(Credits);
        GameObject.FindGameObjectWithTag("Quit").GetComponent<Button>().onClick.AddListener(Quit);
    }
}
