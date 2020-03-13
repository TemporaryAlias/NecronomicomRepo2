using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    public List<int> valueList = new List<int>() { };
    [SerializeField] PlayerBehaviour player;
   [SerializeField] List<GameObject> circle = new List<GameObject>();

    private void CreateCircle(Vector2 anchoredPosition)
    {
       GameObject marker = new GameObject("circle", typeof(Image));
        marker.transform.SetParent(graphContainer, false);
        marker.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = marker.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        circle.Add( marker);
    }

    private void ShowGraph(List<int> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 100f;
        float xSize = 50f;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight * 5;
            CreateCircle(new Vector2(xPosition, yPosition));
        }
    }

    public void graph()
    {
        valueList[0] = Mathf.RoundToInt(player.benevolence);
        valueList[1] = Mathf.RoundToInt(player.malice);
        valueList[2] = Mathf.RoundToInt(player.mystique);
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        ShowGraph(valueList);
    }

    public void destroyplease()
    {
        for (int i = 0; i < circle.Count; i++)
        {
            Destroy(circle[i]);
        }

    }
}
