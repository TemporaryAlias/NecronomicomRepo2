using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneManager : MonoBehaviour {

    enum GameState { MAP, OUTCOMES, STATS, START, END }

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

    [SerializeField] List<TextMeshProUGUI> tweetcontainer = new List<TextMeshProUGUI>();

    [SerializeField] List<TextMeshProUGUI> namecontainer = new List<TextMeshProUGUI>();

    [SerializeField] List<Image> startImages = new List<Image>();

    [SerializeField] GameObject outcomesPanel, statsPanel,endgamepanel;

    [SerializeField] Image backgroundImage, monsterImage, reactionImage;

    [SerializeField] float globalMaliceMult, globalMystMult, globalBenevMult;

    [SerializeField] int bordomThreshold, numberOfTurns;

    [SerializeField] TextMeshProUGUI txt = new TextMeshProUGUI();

    [SerializeField] TextMeshProUGUI title, turnsRemainingText;

    //[SerializeField] List<string> globaltweetlines = new List<string>();

    [SerializeField] List<WindowGraph> graphupdater = new List<WindowGraph>();

    int currentTutIndex, currentPlayerIndex, maliceActionCount, benevActionCount, mystActionCount,turncounter;

    GameState currentGameState = GameState.START;

    void Start() {
        currentTutIndex = 0;
        startImages[currentTutIndex].gameObject.SetActive(true);

        txt.text = "The town is loving all kinds of monsters right now!";

        //ChangeGameState(GameState.MAP);
    }

    void Update() {
        switch (currentGameState) {
            case GameState.MAP:
                turnsRemainingText.text = (numberOfTurns - turncounter + 1).ToString() + " Turns Remaining";
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

            case GameState.START:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    AdvanceTutorial();
                }
                break;

            case GameState.END:
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

            case GameState.END:
                endgamepanel.SetActive(true);
                break;
        }
    }

    public void NextPlayer() {
        StartCoroutine(SwitchPlayer());
    }

    IEnumerator SwitchPlayer() {
        yield return new WaitForSeconds(0.1f);

        if (currentPlayerIndex < players.Count - 1) {
            currentPlayerIndex++;

            players[currentPlayerIndex].currentState = PlayerBehaviour.PlayerState.MOVING;
        } else {
            ChangeGameState(GameState.OUTCOMES);
        }
    }

    IEnumerator CheckForWin() {
        //see if anyone wins
        if (turncounter >= numberOfTurns)
        {
            ChangeGameState(GameState.END);
        }
        //if not....
        else
        {
            yield return new WaitForSeconds(3);
            for (int i = 0; i < graphupdater.Count; i++)
            {
                graphupdater[i].destroyplease();
            }
            
            turncounter++;
            ChangeGameState(GameState.MAP);
        }
    }

    void CreateImage() {
        CalculateReward();

        backgroundImage.sprite = players[currentPlayerIndex].chosenLocation.backgroundImage;

        monsterImage.sprite = players[currentPlayerIndex].GetMonsterPose();

        reactionImage.sprite = players[currentPlayerIndex].GetReactionImage();

        title.text = players[currentPlayerIndex].titleselection;

        graphupdater[currentPlayerIndex].graph();

        for(int i = 0; i < 5; i++)
        {
            tweetcontainer[i].text = players[currentPlayerIndex].gettweetsforcreature()[i];
        }

        for(int i = 0; i < 6; i++)
        {
            namecontainer[i].text = players[currentPlayerIndex].namesforusers()[i];
        }

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
            //txt.text = globaltweetlines[0];
        }

        if (maliceActionCount >= bordomThreshold) {
            globalMaliceMult = 0.5f;

            malice = true;
            //txt.text = globaltweetlines[1];
        }

        if (mystActionCount >= bordomThreshold) {
            globalMystMult = 0.5f;

            myst = true;
            //txt.text = globaltweetlines[2];
        }

        if (malice == false && benev == false && myst == false) {
            txt.text = "MonsterVille is loving all kinds of monsters right now!";
        } else {
            string boredString = "";

            if (malice) {
                boredString += " malice ";
            }

            if (benev) {
                if (boredString != "") {
                    boredString += "and";
                }

                boredString += " benevolence ";
            }

            if (myst) {
                if (boredString != "") {
                    boredString += "and";
                }
                boredString += " mystery ";
            }

            txt.text = "MonsterVille is bored of" + boredString + "right now...";
        }

        foreach (PlayerBehaviour player in players) {
            player.IndicateBordom(malice, benev, myst);
        }

        benevActionCount--;
        maliceActionCount--;
        mystActionCount--;
    }

    void AdvanceTutorial() {
        startImages[currentTutIndex].gameObject.SetActive(false);

        if (currentTutIndex < startImages.Count - 1) {
            currentTutIndex++;
            
            startImages[currentTutIndex].gameObject.SetActive(true);
        } else {
            //ChangeGameState(GameState.MAP);
            StartCoroutine(DelayedStateChange(GameState.MAP));
        }
    }

    IEnumerator DelayedStateChange(GameState newState) {
        yield return new WaitForSeconds(0.1f);

        ChangeGameState(newState);
    }

}
