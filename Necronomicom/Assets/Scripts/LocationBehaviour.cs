using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationBehaviour : MonoBehaviour {

    public Sprite backgroundImage;

    public int population, captureChance;

    float rewardMultiplier;

    [SerializeField] GameObject highPopSprite, lowCatchChanceSprite, highCatchChanceSprite;

    [SerializeField] float baseInfluenceReward, requiredInfluence, highPopMultiplier, criticalPopMultiplier;

    [SerializeField] int highPopThreshold, criticalPopulation, maxPopulation;

    [SerializeField] TextMeshProUGUI tweet;

    [SerializeField] List<string> status = new List<string>();

    public bool InfluenceCheck(float influence) {
        if (influence < requiredInfluence) {
            return false;
        } else {
            return true;
        }
    }

    public bool CaptureCheck() {
        int rand = Random.Range(1, 100);

        if (rand < captureChance) {
            return true;
        } else {
            return false;
        }
    }

    public float InfluenceReward(float globalStatMultipler) {
        PopulationCheck();

        return baseInfluenceReward * rewardMultiplier * globalStatMultipler;
    }

    void PopulationCheck() {
        if (population >= highPopThreshold && population < criticalPopulation) {
            tweet.text = status[1];
            rewardMultiplier = highPopMultiplier;
            captureChance = 25;
            
            highPopSprite.SetActive(true);
            lowCatchChanceSprite.SetActive(true);
            highCatchChanceSprite.SetActive(false);
            
        } else if (population < highPopThreshold) {
            tweet.text = status[0];
            rewardMultiplier = 1f;
            captureChance = 0;
            
            highPopSprite.SetActive(false);
            lowCatchChanceSprite.SetActive(false);
            highCatchChanceSprite.SetActive(false);
            
        } else if (population >= criticalPopulation) {
            tweet.text = status[2];
            rewardMultiplier = criticalPopMultiplier;
            captureChance = 50;
            
            highPopSprite.SetActive(true);
            lowCatchChanceSprite.SetActive(false);
            highCatchChanceSprite.SetActive(true);
            
        }
    }

    public void UpdatePopulation() {
        if (population > maxPopulation) {
            population = maxPopulation;
        }

        if (population > 0) {
            population--;
        }

        PopulationCheck();
    }

}
