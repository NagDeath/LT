using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragDropScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public static Action<bool> onDragDelegate;

    public bool dragOnSurfaces = true;

    private GameObject draggingObj;
    private RectTransform draggingPlane;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GlobalContainer.GetInstance().IsAnimEnded)
        {
            onDragDelegate?.Invoke(false);

            var canvas = FindInParents<Canvas>(gameObject);
            if (canvas == null)
                return;

            // We have clicked something that can be dragged.
            // What we want to do is create an icon for this.
            var prefab = Resources.Load<GameObject>(gameObject.name);

            draggingObj = Instantiate(prefab);

            draggingObj.transform.SetParent(canvas.transform, false);
            draggingObj.transform.SetAsLastSibling();

            if (dragOnSurfaces)
                draggingPlane = transform as RectTransform;
            else
                draggingPlane = canvas.transform as RectTransform;

            SetDraggedPosition(eventData);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (draggingObj != null)
            SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            draggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = draggingObj.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = draggingPlane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingObj != null)
            Destroy(draggingObj);
    }

    public static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GlobalContainer.GetInstance().IsAnimEnded)
        {
            switch (gameObject.name)
            {
                case "MeteorCard":
                    GlobalContainer.GetInstance().Cataclysm = Cataclysm.Meteor;
                    break;
                case "TornadoCard":
                    GlobalContainer.GetInstance().Cataclysm = Cataclysm.Tornado;
                    break;
                case "EarthquakeCard":
                    GlobalContainer.GetInstance().Cataclysm = Cataclysm.Earthquake;
                    break;
                case "LightningCard":
                    GlobalContainer.GetInstance().Cataclysm = Cataclysm.Lightning;
                    break;
                default:
                    break;
            }
            MaterialChangeScript.changeToStandartMatDelegate?.Invoke();
        }
    }
}
