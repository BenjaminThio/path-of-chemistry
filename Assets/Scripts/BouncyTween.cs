using UnityEngine;

public class BouncyTween : MonoBehaviour
{
    public void OnPointerEnter()
    {
        LeanTween.scale(gameObject, new Vector2(1.2f, 1.2f), 0.2f);
    }

    public void OnPointerExit()
    {
        LeanTween.scale(gameObject, Vector2.one, 0.2f);
    }
}
