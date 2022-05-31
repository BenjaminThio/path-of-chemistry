using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Alert : MonoBehaviour
{
    private static readonly int maxMessages = 50;
    private static List<string> messages = new List<string>();

    private void Start()
    {
        UpdateAlert();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddAlert("Test 1");
            UpdateAlert();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            AddAlert("Test 2");
            UpdateAlert();
        }
    }

    private void UpdateAlert()
    {
        if (messages.Count == 0)
        {
            GameObject Message = Instantiate(Resources.Load<GameObject>("UI/Message"), GameObject.Find("Alert/Scroll View/Viewport/Content").transform);
            Message.name = "Null Message";
            Message.GetComponent<TextMeshProUGUI>().text = "Nothing Here!";
        }
        else
        {
            foreach (Transform child in GameObject.Find("Alert/Scroll View/Viewport/Content").transform)
            {
                Destroy(GameObject.Find(child.name));
            }
            for (int i = 0; i < messages.Count; i++)
            {
                GameObject Message = Instantiate(Resources.Load<GameObject>("UI/Message"), GameObject.Find("Alert/Scroll View/Viewport/Content").transform);
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
