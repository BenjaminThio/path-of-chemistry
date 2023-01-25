//*** Path Of Chemistry: Reborn Edition ***//
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public static string platform = "Mobile";
    public static bool runOnce = false;

    private Database db;
    private float raycastRange = 10f;
    private string[] allowTags = {"Flask", "Compound Creator & Reducer", "Element Constructor", "Door", "Faucet", "Dusbin"};
    public static bool pause = false;
    public LayerMask playerLayerMask;
    private bool isCoroutineRunning;
    private bool isCrosshairPressed;

    private void Start()
    {
        db = Database.db;
        LevelHandler.UpdateLevel();
        GameObject.Find("Crosshair").GetComponent<Button>().onClick.AddListener(PressCrosshair);
        if (!runOnce)
        {
            Alert.AddAlert($"Game saves path: {Application.persistentDataPath}");
            runOnce = true;
        }
    }

    private void Update()
    {
        if (!isCoroutineRunning)
        {
            StartCoroutine(Save());
        }
        RaycastHit hit;
        if (!pause)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastRange, playerLayerMask))
            {
                if (!Global.IsItemExist(hit.transform.tag, allowTags))
                {
                    GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crosshair");
                    GameObject.Find("Crosshair").GetComponent<Button>().enabled = false;
                    if (GameObject.FindGameObjectWithTag("Water") != null)
                    {
                        Destroy(GameObject.FindGameObjectWithTag("Water"));
                    }
                    return;
                }
                if (GameObject.FindGameObjectWithTag("Tag") != null)
                {
                    Destroy(GameObject.FindGameObjectWithTag("Tag"));
                }
                GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Press");
                GameObject.Find("Crosshair").GetComponent<Button>().enabled = true;
                GameObject worldCanvasForTag;
                if (hit.transform.tag == "Compound Creator & Reducer")
                {
                    worldCanvasForTag = Instantiate(Resources.Load<GameObject>($"Tags/{hit.transform.tag}"), hit.transform.parent.transform.GetChild(0).transform, false);
                }
                else
                {
                    worldCanvasForTag = Instantiate(Resources.Load<GameObject>($"Tags/{hit.transform.tag}"), hit.transform, false);
                }
                if (hit.transform.tag == "Flask")
                {
                    if (Database.db.level <= Recipe.experiments.Length)
                    {
                        worldCanvasForTag.transform.GetChild(1).GetComponent<TextMeshPro>().text = $"Level {db.level}/{Recipe.experiments.Length}";
                    }
                    else
                    {
                        worldCanvasForTag.transform.GetChild(1).GetComponent<TextMeshPro>().text = $"Level Cleared";
                    }
                }
                if (hit.transform.tag == "Faucet" && GameObject.FindGameObjectWithTag("Water") == null)
                {
                    Instantiate(Resources.Load<GameObject>("Water/Water"), hit.transform, false);
                }
                if (/*platform == "Mobile" && */isCrosshairPressed || platform == "Desktop" && Input.GetMouseButtonDown(1))
                {
                    if (hit.transform.tag == "Flask")
                    {
                        Pause();
                        GameObject flaskInterface = Instantiate(Resources.Load<GameObject>("Inventory/Flask Interface"), GameObject.Find("Canvas").transform, false);
                        flaskInterface.name = "Flask Interface";
                        GameObject.FindGameObjectWithTag("Flask Interface").GetComponent<Animator>().SetTrigger("Glitch");
                    }
                    else if (hit.transform.tag == "Compound Creator & Reducer")
                    {
                        Pause();
                        GameObject ccrConfirmInterface = Instantiate(Resources.Load<GameObject>("UI/CCR Confirm"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
                        ccrConfirmInterface.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OpenCompoundCreatorInterface());
                        ccrConfirmInterface.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => OpenCompoundReducerInterface());
                    }
                    else if (hit.transform.tag == "Element Constructor")
                    {
                        Pause();
                        GameObject elementConstructor = Instantiate(Resources.Load<GameObject>("UI/Element Constructor"), GameObject.Find("Canvas").transform, false);
                        elementConstructor.name = "Element Constructor";
                        GameObject.FindGameObjectWithTag("Element Constructor Interface").GetComponent<Animator>().SetTrigger("Glitch");
                    }
                    else if (hit.transform.tag == "Faucet")
                    {
                        if (db.hotbarItem[db.slotNum - 1] != null)
                        {
                            db.hotbarItem[db.slotNum - 1] = null;
                            GameObject.FindGameObjectWithTag("Hand").GetComponent<Hand>().ChangeItemOnHand();
                        }
                        else
                        {
                            Alert.AddAlert("Nothing to throw.");
                        }
                        Global.UpdateInventory("Hotbar", db.hotbarItem);
                    }
                    else if (hit.transform.tag == "Dusbin")
                    {
                        if (db.hotbarItem[db.slotNum - 1] != null)
                        {
                            Alert.AddAlert("Don't throw chemical materials into dusbin!");
                        }
                        else
                        {
                            Alert.AddAlert("Nothing to throw.");
                        }
                    }
                    else if (hit.transform.tag == "Door")
                    {
                        StartCoroutine(QuitLight());
                    }
                    isCrosshairPressed = false;
                }
            }
            else
            {
                GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crosshair");
                GameObject.Find("Crosshair").GetComponent<Button>().enabled = false;
                if (GameObject.FindGameObjectWithTag("Tag") != null)
                {
                    Destroy(GameObject.FindGameObjectWithTag("Tag"));
                }
                if (GameObject.FindGameObjectWithTag("Water") != null)
                {
                    Destroy(GameObject.FindGameObjectWithTag("Water"));
                }
            }
        }
        if (platform == "Desktop")
        {
            if (!pause)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    Hotbar hotbar = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<Hotbar>();
                    if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                    {
                        if (db.slotNum + 1 <= db.hotbarItem.Length)
                        {
                            db.slotNum += 1;
                        }
                        else
                        {
                            db.slotNum = 1;
                        }
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                    {
                        if (db.slotNum - 1 > 0)
                        {
                            db.slotNum -= 1;
                        }
                        else
                        {
                            db.slotNum += db.hotbarItem.Length - 1;
                        }
                    }
                    hotbar.UpdateSlot();
                    if (db.hotbarItem[db.slotNum - 1] != null)
                    {
                        hotbar.ItemNameAppear(Convert.ToString(db.hotbarItem[db.slotNum - 1]["Item"]), true);
                    }
                    GameObject.FindGameObjectWithTag("Hand").GetComponent<Hand>().ChangeItemOnHand();
                }
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                Alert.OpenAlertInterface();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject.FindGameObjectWithTag("Pause").GetComponent<Pause>().OpenPauseInterface();
            }
        }
        else if (platform != "Mobile" && platform != "Desktop")
        {
            print("Platform not exist!");
        }
    }

    public static void Pause()
    {
        pause = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator Save()
    {
        isCoroutineRunning = true;
        Database.Save();
        yield return new WaitForSeconds(1f);
        isCoroutineRunning = false;
    }

    private void OpenCompoundCreatorInterface()
    {
        Destroy(GameObject.FindGameObjectWithTag("CCR Confirm"));
        GameObject compoundCreatorInterface = Instantiate(Resources.Load<GameObject>("Inventory/Compound Creator Interface"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
        compoundCreatorInterface.name = "Compound Creator Interface";
        GameObject.FindGameObjectWithTag("Compound Creator & Reducer Interface").GetComponent<Animator>().SetTrigger("Glitch");
    }

    private void OpenCompoundReducerInterface()
    {
        Destroy(GameObject.FindGameObjectWithTag("CCR Confirm"));
        GameObject compoundCreatorInterface = Instantiate(Resources.Load<GameObject>("Inventory/Compound Reducer Interface"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
        compoundCreatorInterface.name = "Compound Reducer Interface";
        GameObject.FindGameObjectWithTag("Compound Reducer Interface").GetComponent<Animator>().SetTrigger("Glitch");
    }

    private void PressCrosshair()
    {
        isCrosshairPressed = true;
    }

    private IEnumerator QuitLight()
    {
        pause = true;
        Instantiate(Resources.Load<GameObject>("UI/Light"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
        yield return new WaitForSeconds(3f);
        Database.Save();
        Application.Quit();
    }
}
