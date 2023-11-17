using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private Database db;

    private void Start()
    {
        db = Database.db;

        SceneManager.LoadScene(db.labIndex + 1);
    }
}
