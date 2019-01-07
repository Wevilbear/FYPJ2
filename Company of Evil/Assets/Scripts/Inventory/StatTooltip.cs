using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class StatTooltip : MonoBehaviour
{
    [SerializeField] Text StatNameText;
    [SerializeField] Text StatModifiersLabelText;
    [SerializeField] Text StatModifiersText;

    private StringBuilder sb = new StringBuilder();

    public void ShowTooltip(CharacterStat stat, string stateName)
    {
        StatNameText.text = stateName;

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void GetStatTopText(CharacterStat stat, string statName)
    {
        sb.Length = 0;
        sb.Append(statName);
        sb.Append(" ");
        sb.Append(stat.Value);
        sb.Append(" (");
        sb.Append(stat.BaseValue);

        if (stat.Value > stat.BaseValue)
            sb.Append("+");
    }
}

