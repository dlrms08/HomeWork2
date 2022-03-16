using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Image background;
    private Image controller;
    private Vector2 touchPos;
    private Vector2 lookPos;

    void Awake()
    {
        background = GetComponent<Image>();
        controller = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Touch Begin : " + eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        touchPos = Vector2.zero;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background.rectTransform, eventData.position, eventData.pressEventCamera, out touchPos))
        {
            touchPos.x /= background.rectTransform.sizeDelta.x;
            touchPos.y /= background.rectTransform.sizeDelta.y;

            touchPos = new Vector2(touchPos.x * 2, touchPos.y * 2);

            touchPos = (touchPos.magnitude > 1) ? touchPos.normalized : touchPos;

            controller.rectTransform.anchoredPosition = new Vector2(
                touchPos.x * background.rectTransform.sizeDelta.x / 2,
                touchPos.y * background.rectTransform.sizeDelta.y / 2);

            //Debug.Log("Touch & Drag : " + eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Touch Ended : " + eventData);
        controller.rectTransform.anchoredPosition = Vector2.zero;
        touchPos = Vector2.zero;
    }

    public float Horizontal()
    {
        return touchPos.x;
    }

    public float Vertical()
    {
        return touchPos.y;
    }
}
 