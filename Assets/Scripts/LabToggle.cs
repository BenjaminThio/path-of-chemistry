using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LabToggle : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Image preview;
    public Button confirmButton;

    private Database db;
    private int labIndex;
    private readonly string[] labNames =
    {
        "Revamped",
        "Classic",
        "Home",
        "Industry"
    };

    private void Start()
    {
        db = Database.db;

        labIndex = db.labIndex;
        UpdatePreview();
    }

    public void Previous()
    {
        if (labIndex - 1 >= 0)
        {
            labIndex -= 1;
        }
        else
        {
            labIndex = labNames.Length - 1;
        }

        UpdatePreview();
    }

    public void Next()
    {
        if (labIndex + 1 < labNames.Length)
        {
            labIndex += 1;
        }
        else
        {
            labIndex = 0;
        }

        UpdatePreview();
    }

    public void Confirm()
    {
        if (labIndex != db.labIndex)
        {
            db.labIndex = labIndex;

            Database.Save();
            UpdatePreview();
            StartCoroutine(FadeOut());
        }
    }

    private void UpdatePreview()
    {
        string labName = labNames[labIndex];

        title.text = $"{labName} Lab";
        preview.sprite = Resources.Load<Sprite>($"Labs/{labName}");

        TextMeshProUGUI confirmButtonText = confirmButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (labIndex > 0)
        {
            confirmButton.interactable = false;
            confirmButtonText.text = "Not Available";
            confirmButton.GetComponent<RectTransform>().sizeDelta = new Vector2(400, confirmButton.GetComponent<RectTransform>().sizeDelta.y);
            confirmButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(confirmButton.GetComponent<RectTransform>().sizeDelta.x - 25, confirmButtonText.GetComponent<RectTransform>().sizeDelta.y);
            return;
        }

        if (labIndex == db.labIndex)
        {
            confirmButton.interactable = false;
            confirmButtonText.text = "In Used";
            confirmButton.GetComponent<RectTransform>().sizeDelta = new Vector2(225, confirmButton.GetComponent<RectTransform>().sizeDelta.y);
            confirmButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(confirmButton.GetComponent<RectTransform>().sizeDelta.x - 25, confirmButtonText.GetComponent<RectTransform>().sizeDelta.y);
        }
        else
        {
            confirmButton.interactable = true;
            confirmButtonText.text = "Confirm";
            confirmButton.GetComponent<RectTransform>().sizeDelta = new Vector2(225, confirmButton.GetComponent<RectTransform>().sizeDelta.y);
            confirmButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(confirmButton.GetComponent<RectTransform>().sizeDelta.x - 25, confirmButtonText.GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    private IEnumerator FadeOut()
    {
        GameObject light = Instantiate(Resources.Load<GameObject>("UI/Light"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);

        light.GetComponent<Animator>().SetFloat("SpeedMultiplier", 2f);
        yield return new WaitForSeconds(1.5f);
        Menu.menuPause = false;
        SceneManager.LoadScene(db.labIndex + 1);
    }
}
