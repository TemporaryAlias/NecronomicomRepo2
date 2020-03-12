using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

    enum GameState { MAP, OUTCOMES, STATS }

    public static SceneManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    [SerializeField] List<PlayerBehaviour> players = new List<PlayerBehaviour>();

    [SerializeField] List<LocationBehaviour> locations = new List<LocationBehaviour>();

    [SerializeField] GameObject outcomesPanel, statsPanel;

    [SerializeField] Image backgroundImage, monsterImage, reactionImage;

    [SerializeField] float globalMaliceMult, globalMystMult, globalBenevMult;

    [SerializeField] int bordomThreshold;

    int currentPlayerIndex, maliceActionCount, benevActionCount, mystActionCount;

    GameState currentGameState;

    void Start() {
        ChangeGameState(GameState.MAP);
    }

    void Update() {
        switch (currentGameState) {
            case GameState.MAP:
                break;

            case GameState.STATS:
                break;

            case GameState.OUTCOMES:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    if (currentPlayerIndex < players.Count - 1) {
                        currentPlayerIndex++;

                        CreateImage();
                    } else {
                        ChangeGameState(GameState.STATS);
                    }
                }
                break;
        }
    }

    void ChangeGameState(GameState newState) {
        switch(newState) {
            case GameState.MAP:
                currentPlayerIndex = 0;

                outcomesPanel.SetActive(false);
                statsPanel.SetActive(false);

                foreach (LocationBehaviour location in locations) {
                    location.UpdatePopulation();
                }

                players[currentPlayerIndex].currentState = PlayerBehaviour.PlayerState.MOVING;

                currentGameState = GameState.MAP;
                break;

            case GameState.OUTCOMES:
                currentPlayerIndex = 0;

                //CalculateReward();

                outcomesPanel.SetActive(true);

                CreateImage();

                currentGameState = GameState.OUTCOMES;
                break;

            case GameState.STATS:
                outcomesPanel.SetActive(false);
                statsPanel.SetActive(true);

                GaugeInterest();
                StartCoroutine(CheckForWin());

                currentGameState = GameState.STATS;
                break;
        }
    }

    public void NextPlayer() {
        if (currentPlayerIndex < players.Count - 1) {
            currentPlayerIndex++;

            if (currentGameState == GameState.OUTCOMES) {
                //CalculateReward();
            }

            players[currentPlayerIndex].currentState = PlayerBehaviour.PlayerState.MOVING;
        } else {
            ChangeGameState(GameState.OUTCOMES);
        }
    }

    IEnumerator CheckForWin() {
        //see if anyone wins

        //if not....

        yield return new WaitForSeconds(1);

        ChangeGameState(GameState.MAP);
    }

    void CreateImage() {
        CalculateReward();

        backgroundImage.sprite = players[currentPlayerIndex].chosenLocation.backgroundImage;
        monsterImage.sprite = players[currentPlayerIndex].GetMonsterPose();
        reactionImage.sprite = players[currentPlayerIndex].GetReactionImage();
    }

    void CalculateReward() {
        switch (players[currentPlayerIndex].chosenAction) {
            case PlayerBehaviour.PlayerAction.BENEV:
                players[currentPlayerIndex].benevolence += players[currentPlayerIndex].chosenLocation.InfluenceReward(globalBenevMult);

                benevActionCount++;
                break;

            case PlayerBehaviour.PlayerAction.MALICE:
                players[currentPlayerIndex].malice += players[currentPlayerIndex].chosenLocation.InfluenceReward(globalMaliceMult);

                maliceActionCount++;
                break;

            case PlayerBehaviour.PlayerAction.MYST:
                players[currentPlayerIndex].mystique += players[currentPlayerIndex].chosenLocation.InfluenceReward(globalMystMult);
                mystActionCount++;
                break;

            case PlayerBehaviour.PlayerAction.FAILURE:
                break;

            case PlayerBehaviour.PlayerAction.CAPTURED:
                break;
        }
    }

    void GaugeInterest() {
        globalBenevMult = 1;
        globalMaliceMult = 1;
        globalMystMult = 1;

        bool malice = false, benev = false, myst = false;

        if (benevActionCount >= bordomThreshold) {
            globalBenevMult = 0.5f;

            benev = true;
        }

        if (maliceActionCount >= bordomThreshold) {
            globalMaliceMult = 0.5f;

            malice = true;
        }

        if (mystActionCount >= bordomThreshold) {
            globalMystMult = 0.5f;

            myst = true;
        }

        foreach (PlayerBehaviour player in players) {
            player.IndicateBordom(malice, benev, myst);
        }

        benevActionCount--;
        maliceActionCount--;
        mystActionCount--;
    }

}
