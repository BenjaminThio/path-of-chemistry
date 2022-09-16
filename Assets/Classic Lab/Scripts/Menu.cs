using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    void Start()
    {
        AddFunctionToMenuUI();
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(6, 5.09707f, 72);
    }

    public void Play()
    {
        StartCoroutine(IntoGame());
    }

    private IEnumerator IntoGame()
    {
        Destroy(GameObject.FindGameObjectWithTag("Menu UI"));
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(5f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = false;
        GameObject inGameCanvas  = Instantiate(Resources.Load<GameObject>("Canvas/InGame Canvas"));
        inGameCanvas.name = "Canvas";
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerController>().enabled = true;
        Destroy(GameObject.FindGameObjectWithTag("Menu Canvas"));
    }

    private void Credits()
    {
        Destroy(GameObject.FindGameObjectWithTag("Menu UI"));
        Instantiate(Resources.Load<GameObject>("UI/Credits Background"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
        GameObject.FindGameObjectWithTag("Back").GetComponent<Button>().onClick.AddListener(Back);
    }

    private void Back()
    {
        Destroy(GameObject.FindGameObjectWithTag("Credits Background"));
        Instantiate(Resources.Load<GameObject>("UI/Menu UI"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
        AddFunctionToMenuUI();
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void AddFunctionToMenuUI()
    {
        GameObject menuUi = GameObject.FindGameObjectWithTag("Menu UI");
        menuUi.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Play);
        menuUi.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Credits);
        menuUi.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Quit);
    }
}
