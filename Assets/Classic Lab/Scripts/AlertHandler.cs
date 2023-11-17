using System.Collections;
using TMPro;
using UnityEngine;

public class AlertHandler : MonoBehaviour
{
    public void AddMessage(string messageContent)
    {
        Transform lastMessage = null;

        if (transform.childCount > 0)
        {
           lastMessage = transform.GetChild(transform.childCount - 1);
        }

        GameObject newAlertMessage = Instantiate(Resources.Load<GameObject>("UI/Message"), transform);

        newAlertMessage.GetComponent<TextMeshProUGUI>().text = messageContent;
        if (lastMessage != null)
        {
            StartCoroutine(AlignNewMessageAfterOneFrame(newAlertMessage, lastMessage));
        }
    }

    private IEnumerator AlignNewMessageAfterOneFrame(GameObject newMessage, Transform lastMessage)
    {
        yield return null;

        newMessage.transform.localPosition = new Vector2(0, lastMessage.transform.localPosition.y - lastMessage.GetComponent<RectTransform>().sizeDelta.y);

        float totalMessageHeight = 0;
        foreach (Transform message in transform)
        {
            totalMessageHeight += message.GetComponent<RectTransform>().sizeDelta.y;
        }

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, totalMessageHeight);
    }
}
