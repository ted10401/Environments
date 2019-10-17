using UnityEngine;
using UnityEngine.EventSystems;

namespace JSLCore.UI
{
	public class UIEventTrigger : EventTrigger
	{
		public delegate void VoidDelegate (GameObject go);
		public delegate void PositionDelegate (GameObject go, Vector2 position);

		public VoidDelegate onClick;
		public PositionDelegate onDown;
		public VoidDelegate onEnter;
		public VoidDelegate onExit;
		public PositionDelegate onUp;
		public VoidDelegate onSelect;
		public VoidDelegate onUpdateSelect;

        public static UIEventTrigger Get (GameObject go)
		{
			UIEventTrigger listener = go.GetComponent<UIEventTrigger>();

			if (listener == null)
            {
                listener = go.AddComponent<UIEventTrigger>();
            }

			return listener;
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
            onClick?.Invoke(gameObject);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
            onDown?.Invoke(gameObject, eventData.position);
		}

        public override void OnPointerEnter(PointerEventData eventData)
        {
            onEnter?.Invoke(gameObject);
        }

		public override void OnPointerExit(PointerEventData eventData)
		{
            onExit?.Invoke(gameObject);
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
            onUp?.Invoke(gameObject, eventData.position);
		}

		public override void OnSelect(BaseEventData eventData)
		{
            onSelect?.Invoke(gameObject);
		}

		public override void OnUpdateSelected(BaseEventData eventData)
		{
            onUpdateSelect?.Invoke(gameObject);
		}
	}
}