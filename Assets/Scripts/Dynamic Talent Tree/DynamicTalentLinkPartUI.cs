using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    public class DynamicTalentLinkPartUI : MonoBehaviour
    {
        [Header("Optional properties")]
        [SerializeField] private float tweenDur = 1f;
        
        [Header("Required properties and references")]
        [SerializeField] private Transform fillImage = default;
        [SerializeField] private bool isY = false;
        
        [SerializeField] private float length = 0f;
        
        public Action onFillTweenEnd = delegate { };
        
        public Transform FillImage => fillImage;

        public bool IsY
        {
            get => isY;
            set => isY = value;
        }
        
        public float Length => length;

        public bool IsFilled =>
            isY ? fillImage.localScale.y + Mathf.Epsilon > 1f : fillImage.localScale.x + Mathf.Epsilon > 1f;

        private void Awake()
        {
            DOTween.Init(false, false, LogBehaviour.Verbose);
            DOTween.defaultAutoPlay = AutoPlay.None;
            
            if (isY) length = ((RectTransform) fillImage).rect.height;
            else length = ((RectTransform) fillImage).rect.width;
        }

        public Tweener SetFill(float fill)
        {
            Tweener tweener = null;

            if (isY) tweener = fillImage.transform.DOScaleY(fill, tweenDur).SetSpeedBased();
            else tweener = fillImage.transform.DOScaleX(fill, tweenDur).SetSpeedBased();

            return tweener;
        }

        public void SetFillInstant(float fill)
        {
            var scale = fillImage.localScale;

            if (isY) scale = new Vector3(scale.x, fill, scale.z);
            else scale = new Vector3(fill, scale.y, scale.z);

            fillImage.localScale = scale;
        }
    }
}
