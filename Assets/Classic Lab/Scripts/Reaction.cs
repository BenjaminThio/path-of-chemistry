using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    private Database db;

    private void Start()
    {
        db = Database.db;
    }
    
    public void PlayPourAnimation()
    {
        //Instantiate(Resources.Load<GameObject>("Animations/Flask_Pour"), GameObject.FindGameObjectWithTag("Reaction").transform, false);
        StartCoroutine(React());
    }

    private IEnumerator React()
    {
        Dictionary<string, int> itemProps = Global.CreateVirtualProps(db.flaskItem);
        if (itemProps.Count == 0)
        {
            Alert.AddAlert("Nothing to react!");
            yield break;
        }
        for (int recipeIndex = 0; recipeIndex < Recipe.experiments.Length; recipeIndex++)
        {
            if (itemProps.Count.Equals(Recipe.experiments[recipeIndex].Count) && Global.ContainsProps(Recipe.experiments[recipeIndex], itemProps))
            {
                GameObject flaskPour = GameObject.FindGameObjectWithTag("Flask");
                if (recipeIndex == db.level - 1)
                {
                    CloseInterface.CloseFlaskInterface();
                    flaskPour.layer = LayerMask.NameToLayer("Uninteractable");
                    int counter = 0;
                    for (int i = 0; i < db.flaskItem.Length; i++)
                    {
                        if (db.flaskItem[i] != null)
                        {
                            counter++;
                            GameObject beaker = GameObject.Find($"Cylinder Beaker ({i + 1})");
                            Transform beakerLiquid = beaker.transform.GetChild(0);
                            Transform flaskLiquid = flaskPour.transform.GetChild(0);
                            beakerLiquid.GetComponent<Animator>().enabled = true;
                            beaker.GetComponent<Animator>().SetTrigger("Pour");
                            flaskLiquid.GetComponent<Animator>().SetTrigger($"Fill{counter}");
                            beakerLiquid.GetComponent<Animator>().SetTrigger("Fill");
                            yield return new WaitForSeconds(7f);
                            flaskLiquid.GetComponent<Animator>().enabled = false;
                            flaskLiquid.GetComponent<MeshRenderer>().material.SetFloat("Fill", 0.25f * counter);
                        }
                    }
                    for (int i = 1; i <= db.flaskItem.Length; i++)
                    {
                        if (GameObject.Find($"Cylinder Beaker ({i})/Liquid") != null)
                        {
                            Destroy(GameObject.Find($"Cylinder Beaker ({i})/Liquid"));
                        }
                    }
                    db.level += 1;
                    LevelHandler.UpdateLevel();
                }
                else if (recipeIndex > db.level - 1)
                {
                    Alert.AddAlert("Experiement locked.");
                    yield break;
                }
                db.flaskItem = new Dictionary<string, object>[db.flaskItem.Length];
                if (GameObject.Find("Flask Interface/Flask") != null)
                {
                    Global.UpdateInventory("Flask", db.flaskItem);
                }
                StartCoroutine(GameObject.FindGameObjectWithTag("Experience").GetComponent<Experience>().AddExp((recipeIndex + 1) * 5));
                yield break;
            }
        }
        Alert.AddAlert("Please refer to the experiment's recipes of Chemidex!");
    }
}