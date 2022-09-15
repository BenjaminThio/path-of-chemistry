using UnityEngine;
using TMPro;

public class Title : MonoBehaviour
{
    public float slider = 0;
    public Gradient gradient;

    private void Update()
    {
        GameObject.FindGameObjectWithTag("Title").GetComponent<TextMeshProUGUI>().fontSharedMaterial.SetColor("_GlowColor", gradient.Evaluate(slider));
    }
}
