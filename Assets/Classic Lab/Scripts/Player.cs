using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Database db;
    private float raycastRange = 10f;
    private string[] allowTags = {"Flask", "Compound Creator & Reducer", "Element Constructor", "Door"};
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
                if (!Global.isExist(hit.transform.tag, allowTags))
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
                        GameObject compoundCreatorInterface = Instantiate(Resources.Load<GameObject>("Inventory/Compound Creator Interface"), GameObject.Find("Canvas").transform, false);
                        compoundCreatorInterface.name = "Compound Creator Interface";
                        GameObject.FindGameObjectWithTag("Compound Creator & Reducer Interface").GetComponent<Animator>().SetTrigger("Glitch");
                    }
                    else if (hit.transform.tag == "Element Constructor")
                    {
                        Pause();
                        GameObject elementConstructor = Instantiate(Resources.Load<GameObject>("UI/Element Constructor"), GameObject.Find("Canvas").transform, false);
                        elementConstructor.name = "Element Constructor";
                        GameObject.FindGameObjectWithTag("Element Constructor Interface").GetComponent<Animator>().SetTrigger("Glitch");
                    }
                }
            }
            else
            {
                GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Crosshair");
            }
        }
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            Alert.OpenAlertInterface();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Alert.AddAlert("Test 1");
            Alert.UpdateAlert();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Alert.AddAlert("Test 2");
            Alert.UpdateAlert();
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
}
