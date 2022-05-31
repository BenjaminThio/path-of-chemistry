using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Recipe : MonoBehaviour
{
    private Database db;
    private int selectedSlotNum = 1;
    private string recipeActive;
    public static readonly Dictionary<KeyValuePair<string, int>, Dictionary<string, int>> compounds = new Dictionary<KeyValuePair<string, int>, Dictionary<string, int>>()
    {
        {
            new KeyValuePair<string, int>("At", 5),
            new Dictionary<string, int>()
            {
                {"H", 2},
                {"O", 2}
            }
        }
    };
    public static readonly Dictionary<string, int>[] experiments =
    {
        new Dictionary<string, int>()
        {
            {"H", 1}
        },
        new Dictionary<string, int>()
        {
            {"H", 2}
        },
        new Dictionary<string, int>()
        {
            {"Cm", 1},
            {"Og", 1}
        },
        new Dictionary<string, int>()
        {
            {"He", 1}
        },
        new Dictionary<string, int>()
        {
            {"O", 5}
        }
    };
    private readonly Dictionary<string, int> recipeLength = new Dictionary<string, int>()
    {
        {"Compound", compounds.Count},
        {"Experiment", experiments.Length}
    };

    private void Start()
    {
        db = Database.db;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GetRecipe("Compound");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            GetRecipe("Experiment");
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (GameObject.Find("Recipe Interface") != null)
            {
                Destroy(GameObject.Find("Recipe Interface"));
            }
        }
    }

    public void ToggleRecipeSlot()
    {
        if (!QuantityHandler.pause)
        {
            string selectedSlotName = EventSystem.current.currentSelectedGameObject.name;
            int digitizedSelectedSlotName = Global.Digitize(selectedSlotName);
            if (digitizedSelectedSlotName != selectedSlotNum)
            {
                selectedSlotNum = digitizedSelectedSlotName;
                UpdateRecipe();
            }
        }
    }

    private void GetRecipe(string recipeName)
    {
        if (GameObject.Find("Recipe Interface") == null)
        {
            recipeActive = recipeName;
            GameObject recipeInterface = Instantiate(Resources.Load<GameObject>("UI/Recipe Interface"), GameObject.Find("Canvas").transform, false);
            recipeInterface.name = "Recipe Interface";
            for (int i = 1; i <= recipeLength[recipeName]; i++)
            {
                GameObject recipeSlot = Instantiate(Resources.Load<GameObject>("UI/Recipe Slot"), GameObject.Find("Recipe Interface/Scroll View/Viewport/Content").transform);
                recipeSlot.name = $"Recipe Slot ({i})";
                GameObject.Find($"Recipe Slot ({i})").GetComponent<Button>().onClick.AddListener(ToggleRecipeSlot);
            }
            UpdateRecipe();
        }
    }

    private void UpdateRecipe()
    {
        string info = "";
        Dictionary<string, int> recipeItems = new Dictionary<string, int>();
        if (recipeActive == "Compound")
        {
            recipeItems = compounds[compounds.ElementAt(selectedSlotNum - 1).Key];
        }
        else if (recipeActive == "Experiment")
        {
            recipeItems = experiments[selectedSlotNum - 1];
        }
        for (int i = 1; i <= recipeLength[recipeActive]; i++)
        {
            if (i == selectedSlotNum)
            {
                GameObject.Find($"Recipe Slot ({i})").GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                GameObject.Find($"Recipe Slot ({i})").GetComponent<Image>().color = Color.grey;
            }
            if (recipeActive == "Compound")
            {
                GameObject.Find($"Recipe Slot ({i})/Text").GetComponent<TextMeshProUGUI>().text = compounds.ElementAt(i - 1).Key.Key;
            }
            else if (recipeActive == "Experiment")
            {
                if (i <= db.level)
                {
                    GameObject.Find($"Recipe Slot ({i})/Text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(i);
                }
                else
                {
                    GameObject.Find($"Recipe Slot ({i})/Text").GetComponent<TextMeshProUGUI>().text = "Locked";
                }
            }
        }
        for (int itemNum = 0; itemNum < recipeItems.Count; itemNum++)
        {
            KeyValuePair<string, int> item = recipeItems.ElementAt(itemNum);
            info += $"{item.Key} x {item.Value}";
            if (itemNum < recipeItems.Count - 1)
            {
                info += " + ";
            }
        }
        if (recipeActive == "Compound")
        {
            GameObject.Find("Recipe Interface/Info").GetComponent<TextMeshProUGUI>().text = info;
        }
        else if (recipeActive == "Experiment")
        {
            if (selectedSlotNum <= db.level)
            {
                GameObject.Find("Recipe Interface/Info").GetComponent<TextMeshProUGUI>().text = info;
            }
            else
            {
                GameObject.Find("Recipe Interface/Info").GetComponent<TextMeshProUGUI>().text = "?";
            }
        }
    }
}
