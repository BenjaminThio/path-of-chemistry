using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Database db;
    private float raycastRange = 10f;
    private string[] allowTags = {"Flask", "Compound Creator & Reducer", "Element Constructor", "Door", "Faucet", "Dusbin"};
    public static bool pause = false;
    public LayerMask playerLayerMask;

    private void Start()
    {
        db = Database.db;
        LevelHandler.UpdateLevel();
    }

    private void Update()
    {
        RaycastHit hit;
        if (!pause)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastRange, playerLayerMask))
            {
                if (!Global.IsItemExist(hit.transform.tag, allowTags))
                {
                    GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crosshair");
                    return;
                }
                GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Press");
                if (Input.GetMouseButtonDown(1))
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
                        Application.Quit();
                    }
                }
            }
            else
            {
                GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crosshair");
            }
        }
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
                    hotbar.ItemNameAppear(Convert.ToString(db.hotbarItem[db.slotNum - 1]["Item"]));
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Alert.OpenAlertInterface();
        }
    }

    public static void Pause()
    {
        pause = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Save()
    {
        Database.Save();
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
}
