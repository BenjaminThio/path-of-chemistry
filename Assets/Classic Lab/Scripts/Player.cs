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
    public float raycastRange = 10f;
    private string[] allowTags = {"Flask", "Compound Creator & Reducer", "Element Constructor", "Door", "Faucet", "Dusbin", "Chair"};
    public static bool pause = false;
    public LayerMask playerLayerMask;
    private bool isCoroutineRunning;
    private bool isCrosshairPressed;

    public Transform targetedChair = null;
    public bool moveable = true;
    public Vector3 positionBeforeSit = Vector3.zero;

    private void Start()
    {
        db = Database.db;
        PlayerController playerController = GetComponent<PlayerController>();

        raycastRange *= playerController.scale;

        LevelHandler.UpdateLevel();
        GameObject.Find("Crosshair").GetComponent<Button>().onClick.AddListener(PressCrosshair);
        playerController.cameraHeight = playerController.cameraTransform.localPosition.y;
        Sit();
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
                if (platform == "Desktop")
                {
                    GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Left_Click");
                }
                else if (platform == "Mobile")
                {
                    GameObject.Find("Crosshair").GetComponent<Image>().sprite = Resources.Load<Sprite>("Press");
                }
                GameObject.Find("Crosshair").GetComponent<Button>().enabled = true;
                GameObject worldCanvasForTag;
                if (hit.transform.tag == "Compound Creator & Reducer")
                {
                    worldCanvasForTag = Instantiate(Resources.Load<GameObject>($"Tags/{hit.transform.tag}"), hit.transform.parent.transform.GetChild(0).transform, false);
                }
                else
                {
                    if (hit.transform.tag == "Chair")
                    {
                        if (hit.transform.childCount > 1)
                        {
                            if (hit.transform.GetChild(1).tag == "Custom Sit Point")
                            {
                                worldCanvasForTag = Instantiate(Resources.Load<GameObject>($"Tags/{hit.transform.tag}"), hit.transform.GetChild(1), false);
                            }
                            else
                            {
                                worldCanvasForTag = Instantiate(Resources.Load<GameObject>($"Tags/{hit.transform.tag}"), hit.transform, false);
                            }
                        }
                        else
                        {
                            worldCanvasForTag = Instantiate(Resources.Load<GameObject>($"Tags/{hit.transform.tag}"), hit.transform, false);
                        }
                    }
                    else
                    {
                        worldCanvasForTag = Instantiate(Resources.Load<GameObject>($"Tags/{hit.transform.tag}"), hit.transform, false);
                    }
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
                        GameObject flaskInterface = Instantiate(Resources.Load<GameObject>("Inventory/Flask Interface"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
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
                        GameObject elementConstructor = Instantiate(Resources.Load<GameObject>("UI/Element Constructor"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
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
                    else if (hit.transform.tag == "Chair" && targetedChair == null)
                    {
                        Seat targetedSeat = hit.transform.GetChild(0).GetComponent<Seat>();

                        if (targetedSeat.isPlayerInTriggerArea)
                        {
                            targetedChair = hit.transform;

                            Sit();
                        }
                        else
                        {
                            Alert.AddAlert("You may not sit now, the seat is too far away.");
                        }
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

    private void Sit()
    {
        if (targetedChair != null)
        {
            PlayerController playerController = GetComponent<PlayerController>();
            GameObject playerView = GameObject.FindGameObjectWithTag("MainCamera");
            float cameraHeight = playerController.cameraHeight;

            if (playerController.crouch)
            {
                playerController.crouchBeforeSit = true;
            }

            raycastRange = 12f * playerController.scale;

            playerController.crouch = false;
            playerController.shortClicks = 0;
            playerController.doubleClick = false;
            playerController.CrouchCheck();

            moveable = false;
            if (runOnce)
            {
                positionBeforeSit = transform.position;
            }

            targetedChair.GetComponent<MeshCollider>().enabled = false;

            if (targetedChair.transform.childCount > 1)
            {
                if (targetedChair.transform.GetChild(1).tag == "Custom Sit Point")
                {
                    transform.position = new Vector3(targetedChair.transform.GetChild(1).position.x, transform.position.y, targetedChair.transform.GetChild(1).transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(targetedChair.transform.position.x, transform.position.y, targetedChair.transform.position.z);
                }
            }
            else
            {
                transform.position = new Vector3(targetedChair.transform.position.x, transform.position.y, targetedChair.transform.position.z);
            }

            playerView.transform.localPosition = new Vector3(playerView.transform.localPosition.x, cameraHeight / 1.3f, playerView.transform.localPosition.z);

            if (GameObject.FindGameObjectWithTag("Sprint") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("Sprint"));
            }
            if (playerController.footstepSound.isPlaying)
            {
                playerController.footstepSound.Pause();
            }

            GameObject.FindGameObjectWithTag("Action").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sit");
        }
    }

    public void Pause()
    {
        PlayerController playerController = GetComponent<PlayerController>();

        pause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.velocity = Vector3.zero;
        if (playerController.footstepSound.isPlaying)
        {
            playerController.footstepSound.Pause();
        }
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
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().velocity = Vector3.zero;
        Instantiate(Resources.Load<GameObject>("UI/Light"), GameObject.FindGameObjectWithTag("Canvas").transform, false);
        yield return new WaitForSeconds(3f);
        Database.Save();
        Application.Quit();
    }
}
