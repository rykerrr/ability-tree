
using System;
using UnityEngine;

namespace Talent_Tree
{
    public class TalentLinkPartUI : MonoBehaviour
    {
        [SerializeField] private Transform fillImage = default;
        [SerializeField] private bool isY = false;

        private float length = 0f;
        
        public Transform FillImage => fillImage;
        public bool IsY => isY;
        public float Length => length;

        private void Awake()
        {
            if (isY) length = ((RectTransform) fillImage).rect.height;
            else length = ((RectTransform) fillImage).rect.width;
        }

        public void SetFill(float fill)
        {
            var scale = fillImage.localScale;

            if (isY) fillImage.localScale = new Vector3(scale.x, fill, scale.z);
            else fillImage.localScale = new Vector3(fill, scale.y, scale.z);
        }
    }
}
