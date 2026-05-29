using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPageSlider : MonoBehaviour,IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;
    public int totalPages = 3;
    public float scrollSpeed = 10f;

    private int currentPage = 0;
    private float targetPos;
    private bool isDragging = false;

    void Start()
    {
        targetPos = 0f;
    }

    void Update()
    {
        if (!isDragging)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(
                scrollRect.horizontalNormalizedPosition,
                targetPos,
                Time.deltaTime * scrollSpeed
            );
        }
    }

    // BUTTONS STILL WORK
    public void NextPage()
    {
        currentPage = Mathf.Clamp(currentPage + 1, 0, totalPages - 1);
        SetTargetFromPage();
    }

    public void PreviousPage()
    {
        currentPage = Mathf.Clamp(currentPage - 1, 0, totalPages - 1);
        SetTargetFromPage();
    }

    void SetTargetFromPage()
    {
        targetPos = (float)currentPage / (totalPages - 1);
    }

    // DRAG DETECTION
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // Find closest page
        float currentPos = scrollRect.horizontalNormalizedPosition;
        float pageSize = 1f / (totalPages - 1);

        currentPage = Mathf.RoundToInt(currentPos / pageSize);
        currentPage = Mathf.Clamp(currentPage, 0, totalPages - 1);

        SetTargetFromPage();
    }
}