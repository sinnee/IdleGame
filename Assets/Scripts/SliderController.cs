using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
	public Slider expSlider;
	public GameObject fillArea;

	public void OnSetFillArea()
	{
		if (expSlider.value > 0)
			fillArea.SetActive(true);
		else
			fillArea.SetActive(false);
	}
}