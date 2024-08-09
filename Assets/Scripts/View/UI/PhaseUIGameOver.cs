using TMPro;
using UnityEngine;

public class PhaseUIGameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private string[] dialogTexts;

    public void SetDialogToRandom()
    {
        dialogText.text = dialogTexts[Random.Range(0, dialogTexts.Length)];
    }
}