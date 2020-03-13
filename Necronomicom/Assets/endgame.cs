using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endgame : MonoBehaviour
{
   [SerializeField] Image overall, myst, benev, malic;
   [SerializeField] List<PlayerBehaviour> players = new List<PlayerBehaviour>();
   [SerializeField] List<Sprite> monstersprite = new List<Sprite>();
    [SerializeField] bool overallb, mystb, benevb, malicb;
    float max;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (benevb == false)
        {
            max = Mathf.Max(players[0].benevolence, players[1].benevolence, players[2].benevolence);

            for (int i = 0; i < players.Count; i++)
            {
                if (max == players[i].benevolence)
                {
                    benev.sprite = monstersprite[i];
                    benevb = true;
                }
            }
        }

        if (mystb == false)
        {
            max = Mathf.Max(players[0].mystique, players[1].mystique, players[2].mystique);

            for (int i = 0; i < players.Count; i++)
            {
                if (max == players[i].mystique)
                {   
                    myst.sprite = monstersprite[i];
                    mystb = true;
                }
            }
        }

        if (malicb == false)
        {
            max = Mathf.Max(players[0].malice, players[1].malice, players[2].malice);

            for (int i = 0; i < players.Count; i++)
            {
                if (max == players[i].malice)
                {
                    malic.sprite = monstersprite[i];
                    malicb = true;
                }
            }
        }

        if (overallb == false)
        {
            max = Mathf.Max(players[0].malice+players[0].mystique+players[0].benevolence, players[1].malice+players[1].mystique+players[1].benevolence, players[2].malice+players[2].mystique+players[2].benevolence);

            for (int i = 0; i < players.Count; i++)
            {
                if (max == players[i].malice + players[i].mystique + players[i].benevolence)
                {
                    overall.sprite = monstersprite[i];
                    overallb = true;
                }
            }
        }

    }
}
