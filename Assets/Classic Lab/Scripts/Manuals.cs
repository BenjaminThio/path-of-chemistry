using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manuals : MonoBehaviour
{
    private int manualPageNumber = 1;
    private string currentManualName = "Element Constructor";
    private Dictionary<string, int> manualsPageCount = new Dictionary<string, int>()
    {
        {"Element Constructor", 12},
        {"Compound Creator", 14},
        {"Compound Reducer", 14},
        {"Lab Flask", 10},
        {"Faucet", 3},
    };
    private Dictionary<string, string[]> manualsData = new Dictionary<string, string[]>()
    {
        {
            "Element Constructor", new string[]
            {
                "Element constructor is used to build elements by choosing the number of protons, electrons, and neutrons.",
                "First, hover over the element consructor and then click on it.",
                "Element constructor interface will then show up.",
                "The highlight area is where you can control the number of protons.",
                "The highlight area is where you can control the number of electrons.",
                "The highlight area is where you can control the number of neutrons.",
                "To know the number of proton, electron and neutron of a certain element, refer to Chemidex.",
                "To know the number of proton, electron and neutron of a certain element, refer to Chemidex.",
                "To know the number of proton, electron and neutron of a certain element, refer to Chemidex.",
                "After referring to Chemidex, choose the accurate number of protons, electrons, and neutrons of the element you want to construct in the element constructor.",
                "To get constructed elements, click on the highlight area.",
                "The constructed elements will then return to the hotbar."
            }
        },
        {
            "Compound Creator", new string[]
            {
                "Compound creator is used to create compounds by combining elements.",
                "First, hover over the compound creator and reducer. Then, click on it.",
                "Mode interface will then show up.",
                "Click on \"Creator\" button to enter \"Creator\" mode.",
                "Compound creator interface will then show up.",
                "To know what elements can be combined together, refer to Chemidex.",
                "To know what elements can be combined together, refer to Chemidex.",
                "To know what elements can be combined together, refer to Chemidex.",
                "After referring to Chemidex, add the elements needed for the compound you want to create into the compound creator.",
                "After referring to Chemidex, add the elements needed for the compound you want to create into the compound creator.",
                "After that, click on the \"Create\" button to create compound.",
                "The created compound will then keep in the compound creator.",
                "Click on the highlight area to get created compound.",
                "The created compound will then return to the hotbar."
            }
        },
        {
            "Compound Reducer", new string[]
            {
                "Compound reducer is used to reduce compounds to elements.",
                "First, hover over the compound creator and reducer. Then, click on it.",
                "Mode interface will then show up.",
                "Click on \"Reducer\" button to enter \"Reducer\" mode.",
                "Compound reducer interface will then show up.",
                "To know what elements can be produced after reducing a certain compound, refer to Chemidex.",
                "To know what elements can be produced after reducing a certain compound, refer to Chemidex.",
                "To know what elements can be produced after reducing a certain compound, refer to Chemidex.",
                "After referring to Chemidex, add the compounds you want to reduce into the compound creator.",
                "After referring to Chemidex, add the compounds you want to reduce into the compound creator.",
                "After that, click on the \"Reduce\" button to reduce compound.",
                "The reduced elements will then keep in the compound reducer.",
                "Click on the highlight area to get reduced elements.",
                "The reduced elements will then return to the hotbar."
            }
        },
        {
            "Lab Flask", new string[]
            {
                "Lab flask is used to design your own experiments by combining substances and observing the results.",
                "First, hover over the lab flask and then click on it.",
                "Lab flask interface will then show up",
                "To know what substances can be combined to design experiments, refer to Chemidex.",
                "To know what substances can be combined to design experiments, refer to Chemidex.",
                "To know what substances can be combined to design experiments, refer to Chemidex.",
                "After referring to Chemidex, add the substances needed for the experiment you want to design into the lab flask.",
                "After referring to Chemidex, add the substances needed for the experiment you want to design into the lab flask.",
                "After that, click on the \"React\" button to allow substances react with each other.",
                "And now you just need to observe the result."
            }
        },
        {
            "Faucet", new string[]
            {
                "Faucet is used to clear hotbar items.",
                "First, select the items you want to clear in the hotbar and hover over the faucet. Then, click on it.",
                "The selected items in the hotbar will then clear."
            }
        }
    };

    private void Start()
    {
        for (int i = 0; i < GameObject.FindGameObjectWithTag("Manuals").transform.childCount; i++)
        {
            string manualName = manualsData.ElementAt(i).Key;
            GameObject.FindGameObjectWithTag("Manuals").transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => switchManual(manualName));
        }
        GameObject.FindGameObjectWithTag("Previous").GetComponent<Button>().onClick.AddListener(Previous);
        GameObject.FindGameObjectWithTag("Next").GetComponent<Button>().onClick.AddListener(Next);
        GameObject.Find("Back").GetComponent<Button>().onClick.AddListener(Back);
        Uninteractable();
    }

    private void switchManual(string manualName)
    {
        currentManualName = manualName;
        manualPageNumber = 1;
        Uninteractable();
    }

    private void Previous()
    {
        if (manualPageNumber - 1 > 0)
        {
            manualPageNumber -= 1;
        }
        else if (manualPageNumber - 1 == 0)
        {
            manualPageNumber = manualsPageCount[currentManualName];
        }
        Uninteractable();
    }

    private void Next()
    {
        if (manualPageNumber + 1 <= manualsPageCount[currentManualName])
        {
            manualPageNumber += 1;
        }
        else if (manualPageNumber + 1 > manualsPageCount[currentManualName])
        {
            manualPageNumber = 1;
        }
        Uninteractable();
    }

    private void Back()
    {
        Destroy(GameObject.FindGameObjectWithTag("Manual Interface"));
        Instantiate(Resources.Load<GameObject>("UI/Menu UI"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
        Instantiate(Resources.Load<GameObject>("UI/Hyperlinks"), GameObject.FindGameObjectWithTag("Menu Canvas").transform, false);
        GameObject.FindGameObjectWithTag("Menu Canvas").GetComponent<Menu>().AddFunctionToMenuUI();
    }

    private void Uninteractable()
    {
        GameObject.FindGameObjectWithTag("Manual Page Number").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Numbers/number-{manualPageNumber}");
        GameObject.FindGameObjectWithTag("Manual Image").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Manuals/{currentManualName}/Tutorial{manualPageNumber}");
        if (manualsData[currentManualName][manualPageNumber - 1].Length <= 115)
        {
            GameObject.FindGameObjectWithTag("Explanation").GetComponent<TextMeshProUGUI>().fontSize = 50;
        }
        else if (manualsData[currentManualName][manualPageNumber - 1].Length > 115)
        {
            GameObject.FindGameObjectWithTag("Explanation").GetComponent<TextMeshProUGUI>().fontSize = 40;
        }
        GameObject.FindGameObjectWithTag("Explanation").GetComponent<TextMeshProUGUI>().text = manualsData[currentManualName][manualPageNumber - 1];
        for (int i = 0; i < GameObject.FindGameObjectWithTag("Manuals").transform.childCount; i++)
        {
            if (manualsData.ElementAt(i).Key == currentManualName)
            {
                GameObject.FindGameObjectWithTag("Manuals").transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                GameObject.FindGameObjectWithTag("Manuals").transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }
    }
}
