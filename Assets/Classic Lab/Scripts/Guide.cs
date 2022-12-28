using UnityEngine;
using TMPro;

public class Guide : MonoBehaviour
{
    public float slider = 0;
    public Gradient gradient;

    private void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().fontSharedMaterial.SetColor("_GlowColor", gradient.Evaluate(slider));
        if (gameObject.transform.childCount > 0)
        {
            gameObject.transform.GetChild(0).GetComponent<TMP_SubMeshUI>().sharedMaterial.SetColor("_GlowColor", gradient.Evaluate(slider));
        }
    }
}
