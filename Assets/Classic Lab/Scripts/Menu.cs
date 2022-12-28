//using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject menuUi;

    void Start()
    {
        //print($"{Application.persistentDataPath}/Path Of Chemistry/Data/Saves.json");
        AddFunctionToMenuUI();
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(6, 5.09707f, 72);
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

    public void Play()
    {
        StartCoroutine(IntoGame());
    }

    private IEnumerator IntoGame()
    {
        Destroy(GameObject.FindGameObjectWithTag("Menu UI"));
        Destroy(GameObject.FindGameObjectWithTag("Hyperlinks"));
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(5f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = false;
        GameObject inGameCanvas  = Instantiate(Resources.Load<GameObject>("Canvas/InGame Canvas"));
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
        Destroy(GameObject.FindGameObjectWithTag("Menu UI"));
        Destroy(GameObject.FindGameObjectWithTag("Hyperlinks"));
        Instantiate(Resources.Load<GameObject>("UI/Credits Background"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
        GameObject.FindGameObjectWithTag("Back").GetComponent<Button>().onClick.AddListener(Back);
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
        Destroy(GameObject.FindGameObjectWithTag("Menu UI"));
        Destroy(GameObject.FindGameObjectWithTag("Hyperlinks"));
        Instantiate(Resources.Load<GameObject>("UI/Manual Interface"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
    }

    private void Quit()
    {
        Application.Quit();
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
        GameObject.FindGameObjectWithTag("Play").GetComponent<Button>().onClick.AddListener(Play);
        GameObject.FindGameObjectWithTag("Manual").GetComponent<Button>().onClick.AddListener(Manual);
        GameObject.FindGameObjectWithTag("Credits").GetComponent<Button>().onClick.AddListener(Credits);
        GameObject.FindGameObjectWithTag("Quit").GetComponent<Button>().onClick.AddListener(Quit);
    }
}
