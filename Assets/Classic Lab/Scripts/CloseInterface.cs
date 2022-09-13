using UnityEngine;

public class CloseInterface : MonoBehaviour
{
    public static void CloseFlaskInterface()
    {
        if (!QuantityHandler.pause)
        {
            Destroy(GameObject.FindGameObjectWithTag("Flask Interface"));
            Resume();
        }
    }
    public void CloseCompoundCreatorReducerInterface()
    {
        if (!QuantityHandler.pause)
        {
            Destroy(GameObject.FindGameObjectWithTag("Compound Creator & Reducer Interface"));
            Resume();
        }
    }

    public void CloseElementConstructorInterface()
    {
        if (!QuantityHandler.pause)
        {
            Destroy(GameObject.FindGameObjectWithTag("Element Constructor Interface"));
            Resume();
        }
    }

    public void CloseRecipeInterface()
    {
        if (!QuantityHandler.pause)
        {
            Destroy(GameObject.FindGameObjectWithTag("Recipe Interface"));
            Resume();
        }
    }

    public void CloseAlertInterface()
    {
        if (!QuantityHandler.pause)
        {
            Destroy(GameObject.FindGameObjectWithTag("Alert Interface"));
            Resume();
        }
    }

    public void CloseCompoundReducerInterface()
    {
        if (!QuantityHandler.pause)
        {
            Destroy(GameObject.FindGameObjectWithTag("Compound Reducer Interface"));
            Resume();
        }
    }

    private static void Resume()
    {
        Player.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
