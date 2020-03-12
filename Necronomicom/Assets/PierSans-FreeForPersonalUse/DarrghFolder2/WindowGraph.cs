using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    public List<int> valueList = new List<int>() {};
    [SerializeField] PlayerBehaviour player;

    private void CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
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

    public void Update()
    {
        valueList[0] = Mathf.RoundToInt(player.benevolence);
        valueList[1] = Mathf.RoundToInt(player.malice);
        valueList[2] = Mathf.RoundToInt(player.mystique);
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();


        ShowGraph(valueList);
    }
}
