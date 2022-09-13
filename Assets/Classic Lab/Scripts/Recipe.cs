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
            new KeyValuePair<string, int>("H2O", 1),
            new Dictionary<string, int>()
            {
                {"H", 2},
                {"O", 1}
            }
        },
        {
            new KeyValuePair<string, int>("NaCl", 1),
            new Dictionary<string, int>()
            {
                {"Na", 1},
                {"Cl", 1}
            }
        },
        {
            new KeyValuePair<string, int>("HCl", 1),
            new Dictionary<string, int>()
            {
                {"H", 2},
                {"Cl", 1}
            }
        },
        {
            new KeyValuePair<string, int>("NH3", 1),
            new Dictionary<string, int>()
            {
                {"N", 1},
                {"H", 3}
            }
        },
        {
            new KeyValuePair<string, int>("H2O2", 1),
            new Dictionary<string, int>()
            {
                {"H", 2},
                {"O", 2}
            }
        },
        {
            new KeyValuePair<string, int>("NaI", 1),
            new Dictionary<string, int>()
            {
                {"Na", 1},
                {"I", 2}
            }
        },
        {
            new KeyValuePair<string, int>("Na2S", 1),
            new Dictionary<string, int>()
            {
                {"Na", 2},
                {"S", 1}
            }
        },
        {
            new KeyValuePair<string, int>("KI", 1),
            new Dictionary<string, int>()
            {
                {"K", 1},
                {"I", 1}
            }
        },
        {
            new KeyValuePair<string, int>("N2H4", 1),
            new Dictionary<string, int>()
            {
                {"N", 2},
                {"H", 4}
            }
        },
        {
            new KeyValuePair<string, int>("AgNO3", 1),
            new Dictionary<string, int>()
            {
                {"Ag", 1},
                {"N", 1},
                {"O", 3}
            }
        },
        {
            new KeyValuePair<string, int>("Na3P", 1),
            new Dictionary<string, int>()
            {
                {"Na", 3},
                {"P", 1}
            }
        },
        {
            new KeyValuePair<string, int>("NaH", 1),
            new Dictionary<string, int>()
            {
                {"Na", 1},
                {"H", 1}
            }
        },
        {
            new KeyValuePair<string, int>("IO3", 1),
            new Dictionary<string, int>()
            {
                {"I", 1},
                {"O", 3}
            }
        },
        {
            new KeyValuePair<string, int>("C3H8O", 1),
            new Dictionary<string, int>()
            {
                {"C", 3},
                {"H", 8},
                {"O", 1}
            }
        },
        {
            new KeyValuePair<string, int>("HNO3", 1),
            new Dictionary<string, int>()
            {
                {"H", 1},
                {"N", 1},
                {"O", 3}
            }
        },
        {
            new KeyValuePair<string, int>("NaClO", 1),
            new Dictionary<string, int>()
            {
                {"Na", 1},
                {"Cl", 1},
                {"O", 1}
            }
        },
        {
            new KeyValuePair<string, int>("NaCN", 1),
            new Dictionary<string, int>()
            {
                {"Na", 1},
                {"C", 1},
                {"N", 1}
            }
        },
        {
            new KeyValuePair<string, int>("C2H3NaO2", 1),
            new Dictionary<string, int>()
            {
                {"C", 2},
                {"H", 3},
                {"Na", 1},
                {"O", 2}
            }
        },
        {
            new KeyValuePair<string, int>("C18H35NaO2", 1),
            new Dictionary<string, int>()
            {
                {"C", 18},
                {"H", 35},
                {"Na", 1},
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
        {"Experiment", experiments.Length},
        {"Element", ReadOnly.elements.Length}
    };

    private void Start()
    {
        db = Database.db;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Player.Pause();
            GetRecipe("Compound");
            GameObject.FindGameObjectWithTag("Recipe Interface").GetComponent<Animator>().SetTrigger("Glitch");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Player.Pause();
            GetRecipe("Experiment");
            GameObject.FindGameObjectWithTag("Recipe Interface").GetComponent<Animator>().SetTrigger("Glitch");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Player.Pause();
            GetRecipe("Element");
            GameObject.FindGameObjectWithTag("Recipe Interface").GetComponent<Animator>().SetTrigger("Glitch");
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
            selectedSlotNum = 1;
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
            if (recipeActive == "Element")
            {
                string symbol = Convert.ToString(ReadOnly.elements[i - 1]["symbol"]);
                GameObject.Find($"Recipe Slot ({i})/Text").GetComponent<TextMeshProUGUI>().text = symbol;
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
        if (recipeActive != "Element")
        {
            for (int itemNum = 0; itemNum < recipeItems.Count; itemNum++)
            {
                KeyValuePair<string, int> item = recipeItems.ElementAt(itemNum);
                info += $"{item.Key} x {item.Value}";
                if (itemNum < recipeItems.Count - 1)
                {
                    info += " + ";
                }
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
        else if (recipeActive == "Element")
        {
            Dictionary<string, object> selectedElement = ReadOnly.elements[selectedSlotNum - 1];
            foreach (string key in selectedElement.Keys)
            {
                if (key != "symbol")
                {
                    info += $"{key.ToUpper()}: {selectedElement[key]}\n";
                }
            }
            GameObject.Find("Recipe Interface/Info").GetComponent<TextMeshProUGUI>().text = info;
        }
    }
}
