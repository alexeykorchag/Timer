using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ButtonPressExtension : Button
{
	[SerializeField]
	private float updateTime = 1f;

	public event Action onDown;
	public event Action onUp;
	public event Action onPressed;

	public float PressTime { get; private set; }

	private Coroutine coroutine;

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		onDown?.Invoke();

		coroutine = StartCoroutine(UpdatePressTime());
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		onUp?.Invoke();

		StopCoroutine(coroutine);
	}


	private IEnumerator UpdatePressTime()
    {
		var wfs = new WaitForSecondsRealtime(updateTime);

		PressTime = 0;
		while (true)
        {
			yield return wfs;

			PressTime += updateTime;
			onPressed?.Invoke();
		}
    }

}
