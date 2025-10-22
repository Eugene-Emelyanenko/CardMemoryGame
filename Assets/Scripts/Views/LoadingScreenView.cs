using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Base;

namespace Views
{
    public class LoadingScreenView : View
    {
        [Header("UI Elements")]
        [SerializeField] private Image loadingBar;
        [SerializeField] private TextMeshProUGUI loadingText;
        
        public void SetProgress(float v)
        {
            loadingBar.fillAmount = v;
            loadingText.text = $"{Mathf.RoundToInt(v * 100)}%";
        }
    }
}
