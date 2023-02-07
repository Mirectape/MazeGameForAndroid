using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ResultsScript : MonoBehaviour
{
    private TMP_Text resultsText;

    private void Start()
    {
        resultsText = transform.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        resultsText.text = $"{GameManager.Instance.GetBonusPoints()} / 7\n" +
            "Time: " + $"{GameManager.Instance.totalTime}";
    }
}
