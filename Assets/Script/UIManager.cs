using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SaveTheDoggyLevelManager;
using DG.Tweening;
using System;

namespace SaveTheDogUIManager
{
    public class UIManager : MonoBehaviour
    {
        public Canvas canvasGroup;
        public GameObject winGroup;
        public GameObject lostGroup;
        public GameObject timer;
        public List<Image> starItem;
        public TextMeshProUGUI timeText;
        public Image progressEffect;
        public LevelManager levelManager;
        public Slider sliderAmount;
        private Tween currentTween;

        public void ShowingLostUI()
        {
            canvasGroup.gameObject.SetActive(true);
            lostGroup.gameObject.SetActive(true);
        }
        public void ShowingWinUI()
        {
            canvasGroup.gameObject.SetActive(true);
            winGroup.gameObject.SetActive(true);
        }
        public void DisappearUI()
        {
            canvasGroup.gameObject.SetActive(false);
            winGroup.gameObject.SetActive(false);
            lostGroup.gameObject.SetActive(false);
        }
        public void SetUpCountDownTime()
        {
            progressEffect.fillAmount = 1;
            sliderAmount.value = 1;
            timeText.text = "10";
            if (currentTween != null)
            {
                currentTween.Kill();
            }
            timer.SetActive(false);
            foreach (Image item in starItem)
            {
                item.color = Color.yellow;
            }
        }
        public void ActiveTimer()
        {
            timer.SetActive(true);
        }
        public void ShutDownCurrentTween()
        {
            currentTween.Kill();
        }
        public void ProgressBarEffect()
        {
            currentTween = progressEffect.DOFillAmount(0, 10f).SetEase(Ease.Linear).OnUpdate(() =>
             {
                 UpdateProgressbar(progressEffect.fillAmount);
             }).OnComplete(() =>
             {
                 timeText.text = "0";
             });
        }
        public void UpdateProgressbar(float fillAmount)
        {
            if (fillAmount > 0)
            {
                timeText.text = ((int)(10 * fillAmount + 1)).ToString("F0");
            }
        }
        public void AdjustSlider(bool canDraw)
        {
            if (canDraw)
            {
                sliderAmount.value -= 0.003f;
                if (sliderAmount.value < 0.75)
                {
                    starItem[0].color = Color.black;
                }
                if (sliderAmount.value < 0.5)
                {
                    starItem[1].color = Color.black;
                }
                if (sliderAmount.value < 0.25)
                {
                    starItem[2].color = Color.black;
                }
            }
        }
    }
}

