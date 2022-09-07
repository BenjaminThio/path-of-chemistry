using UnityEngine;

public class CloseInterface : MonoBehaviour
{
    public void CloseFlaskInterface()
    {
        Destroy(GameObject.FindGameObjectWithTag("Flask Interface"));
        Resume();
    }
    public void CloseCompoundCreatorReducerInterface()
    {
        Destroy(GameObject.FindGameObjectWithTag("Compound Creator & Reducer Interface"));
        Resume();
    }

    public void CloseElementConstructorInterface()
    {
        Destroy(GameObject.FindGameObjectWithTag("Element Constructor Interface"));
        Resume();
    }

    public void CloseRecipeInterface()
    {
        Destroy(GameObject.FindGameObjectWithTag("Recipe Interface"));
        Resume();
    }

    public void CloseAlertInterface()
    {
        Destroy(GameObject.FindGameObjectWithTag("Alert Interface"));
        Resume();
    }

    private void Resume()
    {
        Player.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
