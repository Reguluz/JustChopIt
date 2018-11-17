using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MouseFunction
{
	public class UIMouseFunction : MonoBehaviour ,IPointerClickHandler,IBeginDragHandler,IDragHandler,IDropHandler,IPointerEnterHandler,IPointerExitHandler
	{
		public UnityEvent LeftClick;
		public UnityEvent RightClick;
		public UnityEvent LeftHold;
		public UnityEvent LeftRelease;
		public UnityEvent OnFocus;
		public UnityEvent OffFocus;
	
		// Use this for initialization
		void Start () {
			LeftClick.AddListener(new UnityAction(ButtonLeftClick));
			RightClick.AddListener(new UnityAction(ButtonRightClick));
			LeftHold.AddListener(new UnityAction(ButtonLeftOnPress));
			LeftRelease.AddListener(new UnityAction(ButtonLeftRelease));
			OnFocus.AddListener(new UnityAction(ButtonOnFocus));
			OffFocus.AddListener(new UnityAction(ButtonOffFocus));
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			
				if (eventData.button == PointerEventData.InputButton.Left)
				{
					LeftClick.Invoke();
				}else if (eventData.button == PointerEventData.InputButton.Right)
				{
					RightClick.Invoke();
				}
			
			
		}
	
		public void OnBeginDrag(PointerEventData eventData)
		{
			
				if (eventData.button == PointerEventData.InputButton.Left)
				{
					LeftHold.Invoke();
				}
			
			
			
		}

		public void OnDrag(PointerEventData eventData)
		{
		
		}
		public void OnDrop(PointerEventData eventData)
		{
			
				if (eventData.button == PointerEventData.InputButton.Left)
				{
					LeftRelease.Invoke();
				}
			

		}
		public void OnPointerEnter(PointerEventData eventData)
		{
			OnFocus.Invoke();
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			OffFocus.Invoke();
		}

	

		private void ButtonLeftClick()
		{
		
		}

		private void ButtonRightClick()
		{
		
		}

		private void ButtonLeftOnPress()
		{
		
		}

		private void ButtonLeftRelease()
		{
		
		}

		private void ButtonOnFocus()
		{
		
		}
		private void ButtonOffFocus(){}
	


	
	}


}
