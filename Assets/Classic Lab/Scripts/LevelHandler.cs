using System;
using UnityEngine;
using TMPro;

public class LevelHandler : MonoBehaviour
{
    public static void UpdateLevel()
    {
        GameObject.Find("Level").GetComponent<TextMeshProUGUI>().text = Convert.ToString(Database.db.level);
        GameObject.Find("Guide").GetComponent<TextMeshProUGUI>().text = Convert.ToString(Database.db.level);
    }
}
