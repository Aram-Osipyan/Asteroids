using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UiTextModifier: MonoBehaviour
{
    [SerializeField] private Text text;

    public void UpdateText(float newNumber)
    {
        text.text = newNumber.ToString(CultureInfo.InvariantCulture);
    }
}
