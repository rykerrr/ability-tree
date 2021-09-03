using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    public class DynamicTalentTreeCreator : MonoBehaviour
    {
        [SerializeField] private bool activated = false;

        [Header("Prefabs")] [SerializeField] private DynamicTalentUI dynamicTalentUIPrefab = default;
        [SerializeField] private DynamicTalentLinkUI dynamicLinkUIPrefab = default;
        [SerializeField] private DynamicTalentLinkPartUI dynamicLinkPartUIPrefab = default;

        [Header("Tree preferences")] [SerializeField]
        private Transform talentsContainer = default;

        [SerializeField] private Transform linksContainer = default;
        [Space(15)] [SerializeField] private RectTransform rootPosition = default;
        [SerializeField] private DynamicTalent root = default;
        [SerializeField] private float xDistanceFromNode = default;
        [SerializeField] private float yDistanceFromNode = default;
        [SerializeField] private float partWidth = 15f;

        [SerializeField] private List<DynamicTalentUI> createdTalentUIs;

        void Awake()
        {
            if (!activated) return;

            CreateTree();
        }

        private void CreateTree()
        {
            // will need level-order traversal left->right
            
            var rootPos = rootPosition.position;

            var rootNode = CreateNodeUI(root, rootPos);
            int nodeCount = 1;

            var curLinkList = new List<DynamicTalentLinkUI>();

            for (int i = 0; i < root.Links.Count; i++)
            {
                var link = root.Links[i];

                var newNode = CreateNodeUI(link.Destination, new Vector3(
                    rootPos.x - ((i + 1) - root.Links.Count / 2f) * xDistanceFromNode
                    , rootPos.y - yDistanceFromNode * nodeCount, 0f));

                var newLink = CreateLinkBetweenNodes(rootNode, newNode, link);

                curLinkList.Add(newLink);
            }

            nodeCount++;

            rootNode.Init(root, 10, curLinkList);
            curLinkList.Clear();
        }

        private DynamicTalentUI CreateNodeUI(DynamicTalent nodeBase, Vector3 pos)
        {
            var talentUiClone = Instantiate(dynamicTalentUIPrefab, pos, Quaternion.identity);
            talentUiClone.transform.SetParent(talentsContainer);

            return talentUiClone;
        }

        private DynamicTalentLinkUI CreateLinkBetweenNodes(DynamicTalentUI node1, DynamicTalentUI node2,
            TalentLink link)
        {
            var node1Pos = node1.transform.position;
            var node2Pos = node2.transform.position;

            var midPoint = new Vector3(node1Pos.x - node2Pos.x,
                Math.Max(node1Pos.y, node2Pos.y), 0f);

            var linkClone = Instantiate(dynamicLinkUIPrefab, midPoint, Quaternion.identity);

            var linkTransf = linkClone.transform;
            linkTransf.SetParent(linksContainer);
            linkTransf.localPosition = Vector3.zero;

            var linkPartsParent = linkTransf.GetChild(0);
            linkPartsParent.localPosition = Vector3.zero;

            CreateLinkParts(node1, node2, linkPartsParent);

            return linkClone;

            // var node1Pos = node1.transform.position;
            // var node2Pos = node2.transform.position;
            //
            // var linkUiClone = Instantiate(dynamicLinkUIPrefab, new Vector3(node1Pos.x - node2Pos.x,
            //     Mathf.Max(node1Pos.y, node2Pos.y), 0f), Quaternion.identity);
            // linkUiClone.transform.SetParent(linksContainer);
            //
            // var parts = CreateLinkParts(node1, node2, linkUiClone.transform.GetChild(0));
            //
            // linkUiClone.Init(node2, link, parts);
            //
            // return linkUiClone;
        }

        private List<DynamicTalentLinkPartUI> CreateLinkParts(DynamicTalentUI node1, DynamicTalentUI node2,
            Transform partsParent)
        {
            List<DynamicTalentLinkPartUI> parts = new List<DynamicTalentLinkPartUI>();

            Vector3 pt1NewPos = default;

            var node1RectTransf = (RectTransform) node1.transform;
            var node2RectTransf = (RectTransform) node2.transform;

            var node1Pos = node1RectTransf.anchoredPosition;
            var node2Pos = node2RectTransf.anchoredPosition;

            var xIsDiff = !Mathf.Approximately(node1Pos.x, node2Pos.x);
            var yIsDiff = !Mathf.Approximately(node1Pos.y, node2Pos.y);

            if (xIsDiff)
            {
                var pt1 = Instantiate(dynamicLinkPartUIPrefab, partsParent);
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

                parts.Add(pt1);
            }

            if (yIsDiff)
            {
                var pt2 = Instantiate(dynamicLinkPartUIPrefab, partsParent);
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

                parts.Add(pt2);
            }

            return parts;
        }
    }
}