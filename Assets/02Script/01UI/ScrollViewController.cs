using UnityEngine;
using UnityEngine.UI;

public class DynamicScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public float scaleFactor = 0.5f;
    public float minScale = 0.5f;
    public float snapSpeed = 10f;

    private bool isSnapping = false;
    private int targetIndex = 0;

    void Update()
    {
        if (isSnapping)
        {
            SnapTo(targetIndex);
        }

        AdjustScales();
    }

    public void OnScroll()
    {
        if (!isSnapping)
        {
            float scrollPos = scrollRect.horizontalNormalizedPosition * (content.childCount - 1);
            int nearestIndex = Mathf.RoundToInt(scrollPos);
            targetIndex = nearestIndex;
            isSnapping = true;
        }
    }

    private void SnapTo(int index)
    {
        float targetPos = (float)index / (content.childCount - 1);
        scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetPos, Time.deltaTime * snapSpeed);
        if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - targetPos) < 0.001f)
        {
            scrollRect.horizontalNormalizedPosition = targetPos;
            isSnapping = false;
        }
    }

    private void AdjustScales()
    {
        float scrollViewCenter = scrollRect.GetComponent<RectTransform>().rect.width / 2;
        float contentStartX = content.anchoredPosition.x;

        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform child = content.GetChild(i).GetComponent<RectTransform>();
            float childCenter = contentStartX + child.anchoredPosition.x + child.rect.width / 2;
            float distance = Mathf.Abs(scrollViewCenter - childCenter);

            float scale = 1 - (distance / scrollViewCenter) * scaleFactor;
            scale = Mathf.Max(scale, minScale);

            child.localScale = new Vector3(scale, scale, 1);
        }
    }
}