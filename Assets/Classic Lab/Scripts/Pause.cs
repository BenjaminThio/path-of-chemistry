using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public void OpenPauseInterface()
    {
        if (!Player.pause)
        {
            Player.pause = true;
            GameObject pauseInterface = Instantiate(Resources.Load<GameObject>("UI/Pause Interface"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
            pauseInterface.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(MainMenu);
            pauseInterface.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Quit);
        }
    }

    private void MainMenu()
    {
        Database.Save();
        Player.pause = false;
        SceneManager.LoadScene(0);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
