using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    //private bool onMenu = true;

    void Start()
    {
        gameObject.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Play);
        gameObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Quit);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(6, 5, 72);
    }

    private void BackToMenu()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
        GameObject.FindGameObjectWithTag("Main Camera").GetComponent<PlayerController>().enabled = false;
    }

    public void Play()
    {
        StartCoroutine(IntoGame());
    }

    private IEnumerator IntoGame()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            
        }
        Destroy(GameObject.Find("Title"));
        Destroy(GameObject.Find("Play"));
        Destroy(GameObject.Find("Credits"));
        Destroy(GameObject.Find("Quit"));
        //Destroy(GameObject.FindGameObjectWithTag("Menu UI"));
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

    private void Quit()
    {
        Application.Quit();
    }
}
