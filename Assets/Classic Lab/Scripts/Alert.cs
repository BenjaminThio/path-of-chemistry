using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Alert : MonoBehaviour
{
    private static readonly int maxMessages = 50;
    private static List<string> messages = new List<string>();

    public static void OpenAlertInterface()
    {
        if (!Player.pause && GameObject.FindGameObjectWithTag("Alert Interface") == null)
        {
            Player.Pause();
            GameObject alertInterface = Instantiate(Resources.Load<GameObject>("UI/Alert Interface"), GameObject.Find("Canvas").transform, false);
            alertInterface.name = "Alert Interface";
            GameObject.FindGameObjectWithTag("Alert Interface").GetComponent<Animator>().SetTrigger("Glitch");
            UpdateAlert();
        }
    }

    public static void UpdateAlert()
    {
        if (messages.Count == 0)
        {
            GameObject Message = Instantiate(Resources.Load<GameObject>("UI/Message"), GameObject.Find("Alert Interface/Scroll View/Viewport/Content").transform);
            Message.name = "Null Message";
            Message.GetComponent<TextMeshProUGUI>().text = "Nothing Here!";
        }
        else
        {
            foreach (Transform child in GameObject.Find("Alert Interface/Scroll View/Viewport/Content").transform)
            {
                Destroy(GameObject.Find(child.name));
            }
            for (int i = 0; i < messages.Count; i++)
            {
                GameObject Message = Instantiate(Resources.Load<GameObject>("UI/Message"), GameObject.Find("Alert Interface/Scroll View/Viewport/Content").transform);
                Message.name = $"Message ({i})";
                Message.GetComponent<TextMeshProUGUI>().text = $"{i + 1}. {messages[i]}";
            }
        }
    }

    public static void AddAlert(string message)
    {
        if (messages.Count + 1 > maxMessages)
        {
            messages.RemoveAt(0);
        }
        messages.Add(message);
    }
}
