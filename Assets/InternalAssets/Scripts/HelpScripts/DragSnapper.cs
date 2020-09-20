using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragSnapper : UIBehaviour, IEndDragHandler, IBeginDragHandler
{
    public static event Action<bool> SwipeAction;

    public ScrollRect scrollRect;
    public SnapDirection direction;
    public int itemCount;

    public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public float speed;
    public bool isDrag = false;

    private Vector2 _drugingValue = Vector2.zero;


    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;

        _drugingValue = eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        if (_drugingValue.x >= 0)
        {
            if (_drugingValue.y > 0 && _drugingValue.y >= _drugingValue.x)
            {
                return;
            }
            else if (_drugingValue.y < 0 && -_drugingValue.y >= _drugingValue.x)
            {
                return;
            }
                SwipeAction(true);
        }
        else
        {
            if (_drugingValue.y > 0 && _drugingValue.y >= -_drugingValue.x)
            {
                return;
            }
            else if (_drugingValue.y < 0 && _drugingValue.y <= _drugingValue.x)
            {
                return;
            }
            SwipeAction(false);

        }

    }
    public void Update()
    {

    }

    private IEnumerator SnapRect()
    {
        if (scrollRect == null)
            throw new System.Exception("Scroll Rect can not be null");

        if (itemCount == 0)
            throw new System.Exception("Item count can not be zero");

        float startNormal = direction == SnapDirection.Horizontal ? scrollRect.horizontalNormalizedPosition : scrollRect.verticalNormalizedPosition;
        float delta = 1f / (float)(itemCount - 1);
        int target = Mathf.RoundToInt(startNormal / delta);
        float endNormal = delta * target;
        float duration = Mathf.Abs((endNormal - startNormal) / speed);


        float timer = 0f;
        while (timer < 1f)
        {
            timer = Mathf.Min(1f, timer + Time.deltaTime / duration);
            float value = Mathf.Lerp(startNormal, endNormal, curve.Evaluate(timer));

            if (direction == SnapDirection.Horizontal)
            {
                scrollRect.horizontalNormalizedPosition = value;
            }
            else
            {
                scrollRect.verticalNormalizedPosition = value;

            }

            yield return new WaitForEndOfFrame();
        }
    }
}


public enum SnapDirection
{
    Horizontal,
    Vertical,
}