using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public enum PlayerState { WAITING, MOVING, PICKING }
    public enum PlayerAction { BENEV, MALICE, MYST, FAILURE, CAPTURED }

    [SerializeField] Sprite benevSprite, maliceSprite, mysSprite, defeatSprite, victorySprite, captureSprite, profileSprite, defaultSprite, crowdBenevSprite, crowdMaliceSprite, crowdMystSprite, FBICapSprite, failedCheckSprite;

    [SerializeField] GameObject statIconsHolder, maliceBoredIcon, benevBoredIcon, mystBoredIcon;

    [SerializeField] List<GameObject> locationList = new List<GameObject>();

    [SerializeField] SpriteRenderer playerIcon;

    [SerializeField] List<string> benevTweetlist = new List<string>();
    [SerializeField] List<string> maliceTweetlist = new List<string>();
    [SerializeField] List<string> mystTweetlist = new List<string>();
    [SerializeField] List<string> defeatTweetlist = new List<string>();
    [SerializeField] List<string> failTweetlist = new List<string>();

    [SerializeField] List<string> title = new List<string>();

    [SerializeField] List<string> twitternames = new List<string>();

    public string titleselection;

    Transform currentPos;

    int currentLocationIndex;

    public bool captured;

    public float benevolence, malice, mystique;

    public PlayerState currentState = PlayerState.WAITING;

    public PlayerAction chosenAction;

    public LocationBehaviour chosenLocation;

    void Start() {
        currentLocationIndex = 0;

        transform.position = locationList[currentLocationIndex].transform.position;

        chosenLocation = null;

        playerIcon.enabled = false;

        captured = false;
    }
    
    void Update() {
        switch (currentState) {
            case PlayerState.MOVING:
                if (captured) {
                    captured = false;

                    SceneManager.instance.NextPlayer();
                    currentState = PlayerState.WAITING;
                    break;
                }

                playerIcon.enabled = true;
                statIconsHolder.SetActive(false);

                if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    if (currentLocationIndex >= locationList.Count - 1) {
                        currentLocationIndex = 0;
                    } else {
                        currentLocationIndex++;
                    }

                    transform.position = locationList[currentLocationIndex].transform.position;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    if (currentLocationIndex <= 0) {
                        currentLocationIndex = locationList.Count - 1;
                    } else {
                        currentLocationIndex--;
                    }

                    transform.position = locationList[currentLocationIndex].transform.position;
                }

                if (Input.GetKeyDown(KeyCode.Space)) {
                    chosenLocation = locationList[currentLocationIndex].GetComponent<LocationBehaviour>();

                    chosenLocation.population += 2;
                    titleselection = title[currentLocationIndex];
                    currentState = PlayerState.PICKING;
                }
                break;

            case PlayerState.PICKING:
                statIconsHolder.SetActive(true);

                if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    if (chosenLocation.CaptureCheck()) {
                        chosenAction = PlayerAction.CAPTURED;

                        captured = true;
                    } else if (chosenLocation.InfluenceCheck(benevolence)) {
                        chosenAction = PlayerAction.BENEV;
                    } else {
                        chosenAction = PlayerAction.FAILURE;
                    }

                    currentState = PlayerState.WAITING;

                    SceneManager.instance.NextPlayer();
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    if (chosenLocation.CaptureCheck()) {
                        chosenAction = PlayerAction.CAPTURED;

                        captured = true;
                    } else if (chosenLocation.InfluenceCheck(mystique)) {
                        chosenAction = PlayerAction.MYST;
                    } else {
                        chosenAction = PlayerAction.FAILURE;
                    }

                    currentState = PlayerState.WAITING;

                    SceneManager.instance.NextPlayer();
                }

                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    if (chosenLocation.CaptureCheck()) {
                        chosenAction = PlayerAction.CAPTURED;

                        captured = true;
                    } else if (chosenLocation.InfluenceCheck(malice)) {
                        chosenAction = PlayerAction.MALICE;
                    } else {
                        chosenAction = PlayerAction.FAILURE;
                    }

                    currentState = PlayerState.WAITING;

                    SceneManager.instance.NextPlayer();
                }


                break;

            case PlayerState.WAITING:
                playerIcon.enabled = false;
                statIconsHolder.SetActive(false);
                break;
        }
    }

    public Sprite GetMonsterPose() {
        Sprite imageToReturn;

        switch (chosenAction) {
            case PlayerAction.BENEV:
                imageToReturn = benevSprite;
                break;

            case PlayerAction.MALICE:
                imageToReturn = maliceSprite;
                break;

            case PlayerAction.MYST:
                imageToReturn = mysSprite;
                break;

            case PlayerAction.FAILURE:
                imageToReturn = defeatSprite;
                break;

            case PlayerAction.CAPTURED:
                imageToReturn = captureSprite;
                break;

            default:
                imageToReturn = defaultSprite;
                break;
        }

        return imageToReturn;
    }

    public Sprite GetReactionImage() {
        Sprite imageToReturn;

        switch (chosenAction) {
            case PlayerAction.BENEV:
                imageToReturn = crowdBenevSprite;
                break;

            case PlayerAction.MALICE:
                imageToReturn = crowdMaliceSprite;
                break;

            case PlayerAction.MYST:
                imageToReturn = crowdMystSprite;
                break;

            case PlayerAction.FAILURE:
                imageToReturn = failedCheckSprite;
                break;

            case PlayerAction.CAPTURED:
                imageToReturn = FBICapSprite;
                break;

            default:
                imageToReturn = crowdMystSprite;
                break;
        }

        return imageToReturn;
    }

    public List<string> gettweetsforcreature()
    {
        List<string> tweets = new List<string>();
        switch (chosenAction)
        {
            case PlayerAction.BENEV:
                for(int i=0; i < 5; i++)
                {
                 int choice= Mathf.RoundToInt( Random.Range(0, benevTweetlist.Count));
                    tweets.Add(benevTweetlist[choice]);
                }
                break;

            case PlayerAction.MALICE:
                for (int i = 0; i < 5; i++)
                {
                    int choice = Mathf.RoundToInt(Random.Range(0, maliceTweetlist.Count));
                    tweets.Add(maliceTweetlist[choice]);
                }
                break;

            case PlayerAction.MYST:
                for (int i = 0; i < 5; i++)
                {
                    int choice = Mathf.RoundToInt(Random.Range(0, mystTweetlist.Count));
                    tweets.Add(mystTweetlist[choice]);
                }
                break;

            case PlayerAction.FAILURE:
                for (int i = 0; i < 5; i++)
                {
                    int choice = Mathf.RoundToInt(Random.Range(0, failTweetlist.Count));
                    tweets.Add(failTweetlist[choice]);
                }
                break;

            case PlayerAction.CAPTURED:
                for (int i = 0; i < 5; i++)
                {
                    int choice = Mathf.RoundToInt(Random.Range(0, defeatTweetlist.Count));
                    tweets.Add(defeatTweetlist[choice]);
                }
                break;

            default:
                for (int i = 0; i < 5; i++)
                {
                    int choice = Mathf.RoundToInt(Random.Range(-1, 40));
                    //tweets.Add(tweetlist[choice]);
                }
                break;
        }

        return tweets;
    }

    public List<string> namesforusers()
    {
        List<string> twitnames = new List<string>();
          for (int i = 0; i < 6; i++)
          {
             int choice = Mathf.RoundToInt(Random.Range(0, 10));
             twitternames.Add(twitternames[choice]);
          }
        
        return twitternames;
    }



    public void IndicateBordom(bool boredOfMalice, bool boredOfBenev, bool boredOfMyst) {
        if (boredOfBenev)
            benevBoredIcon.SetActive(true);
        else
            benevBoredIcon.SetActive(false);

        if (boredOfMyst)
            mystBoredIcon.SetActive(true);
        else
            mystBoredIcon.SetActive(false);

        if (boredOfMalice)
            maliceBoredIcon.SetActive(true);
        else
            maliceBoredIcon.SetActive(false);
    }

}
