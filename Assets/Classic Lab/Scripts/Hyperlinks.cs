using UnityEngine;

public class Hyperlinks : MonoBehaviour
{
    public void Facebook()
    {
        if (!Menu.menuPause)
        {
            Application.OpenURL("https://www.facebook.com/benjamin.thio.771");
        }
    }

    public void Instagram()
    {
        if (!Menu.menuPause)
        {
            Application.OpenURL("https://www.instagram.com/benjamin_thio70");
        }
    }

    public void GitHub()
    {
        if (!Menu.menuPause)
        {
            Application.OpenURL("https://github.com/BenjaminThio?tab=repositories");
        }
    }

    public void Youtube()
    {
        if (!Menu.menuPause)
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }
    }
}
