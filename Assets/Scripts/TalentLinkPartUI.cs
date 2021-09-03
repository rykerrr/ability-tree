using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Talent_Tree
{
    public class TalentLinkPartUI : MonoBehaviour
    {
        [Header("Optional properties")]
        [SerializeField] private float tweenDuration = 1f;
        
        [Header("Required properties and references")]
        [SerializeField] private Transform fillImage = default;
        [SerializeField] private bool isY = false;
        
        [SerializeField] private float length = 0f;
        
        public Action onFillTweenEnd = delegate { };
        
        public Transform FillImage => fillImage;
        public bool IsY => isY;
        public float Length => length;

        public bool IsFilled =>
            isY ? fillImage.localScale.y + Mathf.Epsilon >= 1f : fillImage.localScale.x + Mathf.Epsilon >= 1f;
        
        private readonly Queue<Action> fillCoroutines = new Queue<Action>();
        private WaitForEndOfFrame waitForEndOfFrame;
        
        private void Awake()
        {
            if (isY) length = ((RectTransform) fillImage).rect.height;
            else length = ((RectTransform) fillImage).rect.width;

            waitForEndOfFrame = new WaitForEndOfFrame();
        }

        public void SetFill(float fill)
        {
            // Debug.Log("Setting fill: " + fill);
            
            var scale = fillImage.localScale;

            if (isY) scale = new Vector3(scale.x, fill, scale.z);
            else scale = new Vector3(fill, scale.y, scale.z);

            if (fillCoroutines.Count == 0)
            {
                StartCoroutine(TweenToX(scale));
            }
            else fillCoroutines.Enqueue(() => StartCoroutine(TweenToX(scale)));

            fillCoroutines.Enqueue(() => onFillTweenEnd?.Invoke());
        }

        public void SetFillInstant(float fill)
        {
            var scale = fillImage.localScale;

            if (isY) scale = new Vector3(scale.x, fill, scale.z);
            else scale = new Vector3(fill, scale.y, scale.z);

            fillImage.localScale = scale;
        }
        
        private IEnumerator TweenToX(Vector3 end)
        {
            var t = 0.0f;
            var start = fillImage.localScale;

            while (t < tweenDuration)
            {
                t += Time.deltaTime;

                fillImage.localScale = Vector3.Lerp(start, end, t / tweenDuration);

                yield return waitForEndOfFrame;
            }
            
            if (fillCoroutines.Count > 0)
            {
                fillCoroutines.Dequeue().Invoke();
            }
        }
    }
}
