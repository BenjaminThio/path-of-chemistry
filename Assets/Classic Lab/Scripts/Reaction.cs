using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    private Database db;

    public GameObject explosion;
    public GameObject smoke;
    public GameObject fire;
    public GameObject toxicGas;
    public GameObject cyanideGas;
    public GameObject flammableGas;
    public GameObject elephantToothpaste;
    public GameObject freeze;

    public int maxFillTimes = 9;
    public float transitionDurationPerFill = 3f;
    public float minFillValue = -1f;
    public float maxFillValue = 1.3f;
    private int fillPhase = 0;

    private void Start()
    {
        db = Database.db;
    }
    
    public void PlayPourAnimation()
    {
        //Instantiate(Resources.Load<GameObject>("Animations/Flask_Pour"), GameObject.FindGameObjectWithTag("Reaction").transform, false);
        StartCoroutine(React());
    }

    IEnumerator Fill(GameObject flask, int fillTimes)
    {
        yield return new WaitForSeconds(2f);

        Transform flaskLiquid = flask.transform.GetChild(0);
        
        LeanTween.value(gameObject, GetFillValueWithPhase(fillPhase - 1), GetFillValueWithPhase(fillPhase), transitionDurationPerFill).setOnUpdate(
            (float value) => {
                flaskLiquid.GetComponent<Renderer>().material.SetFloat("Fill", value);
            }
        );

        yield return new WaitForSeconds(2f);
    }

    private float GetFillValueWithPhase(int fillPhase)
    {
        Transform flaskLiquid = GameObject.FindGameObjectWithTag("Flask").transform.GetChild(0);
        float perFill = ((maxFillValue * GetScaleMultiplication(flaskLiquid)) - (minFillValue * GetScaleMultiplication(flaskLiquid))) / maxFillTimes;

        return (minFillValue * GetScaleMultiplication(flaskLiquid)) + (perFill * (fillPhase));
    }

    public void ResetFlaskLiquidFillValue()
    {
        Transform flaskLiquid = GameObject.FindGameObjectWithTag("Flask").transform.GetChild(0);

        flaskLiquid.GetComponent<Renderer>().sharedMaterial.SetFloat("Fill", (minFillValue * GetScaleMultiplication(flaskLiquid)));
        fillPhase = 0;
    }

    private float GetScaleMultiplication(Transform flaskLiquid)
    {
        Transform flask = flaskLiquid.parent;
        Transform reaction = flask.parent;

        return flask.localScale.y * reaction.localScale.y;
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
                if (recipeIndex >= 0 && recipeIndex < db.level - 1 || recipeIndex == db.level - 1)
                {
                    CloseInterface.CloseFlaskInterface();
                    flaskPour.layer = LayerMask.NameToLayer("Uninteractable");
                    ResetFlaskLiquidFillValue();
                    int counter = 0;
                    for (int i = 0; i < db.flaskItem.Length; i++)
                    {
                        if (db.flaskItem[i] != null)
                        {
                            GameObject beaker = GameObject.Find($"Cylinder Beaker ({i + 1})");

                            if (beaker.GetComponent<Animator>() != null)
                            {
                                Transform beakerLiquid = beaker.transform.GetChild(0);
                                beaker.GetComponent<Animator>().SetTrigger("Pour");
                                beakerLiquid.GetComponent<Animator>().enabled = true;
                                beakerLiquid.GetComponent<Animator>().SetTrigger("Fill");
                            }
                            else
                            {
                                Alert.AddAlert($"The developer lazy animate the animation of beaker {i + 1}.");
                            }
                            counter++;
                            fillPhase++;

                            StartCoroutine(Fill(flaskPour, fillPhase));

                            yield return new WaitForSeconds(7f);
                        }
                    }
                    
                    for (int i = 1; i <= db.flaskItem.Length; i++)
                    {
                        if (GameObject.Find($"Cylinder Beaker ({i})/Liquid") != null)
                        {
                            Destroy(GameObject.Find($"Cylinder Beaker ({i})/Liquid"));
                        }
                    }
                    //Explosion
                    GameObject effect = null;
                    if (recipeIndex == 0 || recipeIndex == 7 || recipeIndex == 13)
                    {
                        effect = Instantiate(explosion, GameObject.FindGameObjectWithTag("Effect").transform, false);
                        yield return new WaitForSeconds(2f);
                    }
                    //Smoke
                    else if (recipeIndex == 1)
                    {
                        effect = Instantiate(smoke, GameObject.FindGameObjectWithTag("Effect").transform, false);
                        yield return new WaitForSeconds(9f);
                    }
                    //Fire
                    else if (recipeIndex == 2 || recipeIndex == 10)
                    {
                        effect = Instantiate(fire, GameObject.FindGameObjectWithTag("Effect").transform, false);
                        yield return new WaitForSeconds(13f);
                    }
                    //Toxic Gas
                    else if (recipeIndex == 3 || recipeIndex == 8 || recipeIndex == 9 || recipeIndex == 11)
                    {
                        effect = Instantiate(toxicGas, GameObject.FindGameObjectWithTag("Effect").transform, false);
                        yield return new WaitForSeconds(12f);
                    }
                    //Cyanide Gas
                    else if (recipeIndex == 4)
                    {
                        effect = Instantiate(cyanideGas, GameObject.FindGameObjectWithTag("Effect").transform, false);
                        yield return new WaitForSeconds(2f);
                    }
                    //Flammable Gas
                    else if (recipeIndex == 5)
                    {
                        effect = Instantiate(flammableGas, GameObject.FindGameObjectWithTag("Effect").transform, false);
                        yield return new WaitForSeconds(11f);
                    }
                    //Elephant Toothpaste
                    else if (recipeIndex == 6)
                    {
                        effect = Instantiate(elephantToothpaste, GameObject.FindGameObjectWithTag("Effect").transform, false);
                        yield return new WaitForSeconds(6f);
                    }
                    //Hot Ice
                    else if (recipeIndex == 12)
                    {
                        effect = Instantiate(freeze, GameObject.FindGameObjectWithTag("Effect").transform, false);
                        yield return new WaitForSeconds(2f);
                    }
                    //Under Construction
                    else if (recipeIndex == 14 || recipeIndex == 15)
                    {
                        Alert.AddAlert("Under Construction!");
                    }
                    if (effect != null)
                    {
                        Destroy(effect);
                    }
                }
                if (recipeIndex == db.level - 1)
                {
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
                ResetFlaskLiquidFillValue();
                GameObject.FindGameObjectWithTag("Experience").GetComponent<Experience>().AddExpToQueue((recipeIndex + 1) * 5);
                flaskPour.layer = LayerMask.NameToLayer("Interactable");
                yield break;
            }
        }
        Alert.AddAlert("Please refer to the experiment's recipes of Chemidex!");
    }
}
/*
flaskLiquid.GetComponent<Animator>().enabled = false;
flaskLiquid.GetComponent<MeshRenderer>().material.SetFloat("Fill", 0.25f * counter);
flaskLiquid.GetComponent<Animator>().enabled = true;
*/
