using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    public class BobTestNumberTwo : MonoBehaviour
    {
        [SerializeField] private bool activated = false;

        [Header("Prefabs")] [SerializeField] private DynamicTalentLinkUI linkPrefab = default;
        [SerializeField] private DynamicTalentLinkPartUI linkPartPrefab = default;

        [Header("Preferences")] [SerializeField]
        private Transform linksParent = default;

        [SerializeField] private DynamicTalentUI node1 = default;
        [SerializeField] private DynamicTalentUI node2 = default;
        [SerializeField] private float partWidth;

        private void Awake()
        {
            if (!activated) return;

            CreateLink();
        }

        private void CreateLink()
        {
            var node1Pos = node1.transform.position;
            var node2Pos = node2.transform.position;

            var midPoint = new Vector3(node1Pos.x - node2Pos.x,
                Math.Max(node1Pos.y, node2Pos.y), 0f);

            var link = Instantiate(linkPrefab, midPoint, Quaternion.identity);

            var linkTransf = link.transform;
            linkTransf.SetParent(linksParent);
            linkTransf.localPosition = Vector3.zero;

            var linkPartsParent = linkTransf.GetChild(0);
            linkPartsParent.localPosition = Vector3.zero;

            CreateLinkParts(linkPartsParent);
        }

        // node2 is always below node1
        private void CreateLinkParts(Transform partsParent)
        {
            Vector3 pt1NewPos = default;

            var node1RectTransf = (RectTransform) node1.transform;
            var node2RectTransf = (RectTransform) node2.transform;

            var node1Pos = node1RectTransf.anchoredPosition;
            var node2Pos = node2RectTransf.anchoredPosition;

            var xIsDiff = !Mathf.Approximately(node1Pos.x, node2Pos.x);
            var yIsDiff = !Mathf.Approximately(node1Pos.y, node2Pos.y);

            if (xIsDiff)
            {
                var pt1 = Instantiate(linkPartPrefab, partsParent);
                var pt1Transf = (RectTransform) pt1.transform;

                var pt1FillImg = (RectTransform) pt1.FillImage.transform;

                float pt1Width = 0f;

                var halfWidth = node1RectTransf.rect.width / 2;

                if (node2Pos.x > node1Pos.x)
                {
                    // node2 is on right
                    pt1Width = node2Pos.x - node1Pos.x + partWidth / 2 - halfWidth;
                    pt1NewPos = node1Pos + new Vector2(halfWidth + pt1Width / 2, 0f);

                    pt1FillImg.anchorMin = pt1FillImg.anchorMax = pt1FillImg.pivot = new Vector2(0f, 0.5f);
                }
                else
                {
                    // node2 is on left
                    pt1Width = Math.Abs(node2Pos.x - node1Pos.x - partWidth / 2 + halfWidth);
                    pt1NewPos = node1Pos - (Vector2) new Vector3(halfWidth + pt1Width / 2, 0f, 0f);

                    pt1FillImg.anchorMin = pt1FillImg.anchorMax = pt1FillImg.pivot = new Vector2(1f, 0.5f);
                }

                pt1Transf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pt1Width);
                pt1Transf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, partWidth);
                pt1Transf.anchoredPosition = pt1NewPos;

                pt1FillImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pt1Width);
                pt1FillImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, partWidth);

                pt1FillImg.localScale = new Vector3(0.5f, 1f, 1f);
                pt1FillImg.anchoredPosition = Vector3.zero;
            }

            if (yIsDiff)
            {
                var pt2 = Instantiate(linkPartPrefab, partsParent);
                var pt2Transf = (RectTransform) pt2.transform;

                var pt2FillImg = (RectTransform) pt2.FillImage.transform;

                float pt2Height = default;
                Vector3 pt2Pos = default;

                var halfHeight = node2RectTransf.rect.height / 2;
                pt2Height = Math.Abs(node2Pos.y - node1Pos.y) - halfHeight - partWidth / 2;

                if (node2Pos.x > node1Pos.x)
                    pt2Pos = new Vector3(node2Pos.x, pt1NewPos.y - partWidth / 2 - pt2Height / 2);
                else pt2Pos = new Vector3(node2Pos.x, pt1NewPos.y - partWidth / 2 - pt2Height / 2);

                // positioning seems correct but height is wrong
                // will need to anchor the fill as well

                pt2Transf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pt2Height);
                pt2Transf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, partWidth);
                pt2Transf.anchoredPosition = pt2Pos;

                pt2FillImg.anchorMin = pt2FillImg.anchorMax = pt2FillImg.pivot = new Vector2(0.5f, 1f);

                pt2FillImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pt2Height);
                pt2FillImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, partWidth);

                pt2FillImg.localScale = new Vector3(1f, 0.5f, 1f);
                pt2FillImg.anchoredPosition = Vector3.zero;
            }
        }
    }
}