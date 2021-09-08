using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    public class DynamicTalentTreeCreator : MonoBehaviour
    {
        [SerializeField] private bool activated = false;

        [Header("References")] 
        [SerializeField] private DynamicTalentOverviewUI dynamicTalentOverviewUI = default;

        [Header("Content resize")]
        [SerializeField] private RectTransform contentToResize = default;
        [SerializeField] private float bonusWidth = 40f;
        [SerializeField] private float bonusHeight = 40f;
        
        [Header("Prefabs")] 
        [SerializeField] private DynamicTalentUI dynamicTalentUIPrefab = default;
        [SerializeField] private DynamicTalentLinkUI dynamicLinkUIPrefab = default;
        [SerializeField] private DynamicTalentLinkPartUI dynamicLinkPartUIPrefab = default;

        [Header("Tree preferences")] 
        [SerializeField] private Transform talentsContainer = default;
        [SerializeField] private Transform linksContainer = default;
        
        [Space(15)] 
        [SerializeField] private RectTransform treeRootPositionTransform = default;
        [SerializeField] private DynamicTalent root = default;
        [SerializeField] private float xDistanceFromNode = default;
        [SerializeField] private float yDistanceFromNode = default;
        [SerializeField] private float partWidth = 15f;

        [SerializeField] private List<DynamicTalentUI> createdTalentUIs = new List<DynamicTalentUI>();
        
        
        [Tooltip("0 - max X, 1 - max Y, 2 - min X, 3 - min Y / (0 - right, 1 - top, 2 - left, 3 - bottom)")]
        [SerializeField] private RectTransform[] contentCorners = new RectTransform[4];

        private readonly List<DynamicTalentLinkUI> createdLinkUIs = new List<DynamicTalentLinkUI>();
        private Vector3 treeRootPosition = default;

        private void Awake()
        {
            if (!activated) return;

            treeRootPosition = treeRootPositionTransform.position;
            
            CreateTreeFromRoot();

            var treeNodeHeight = GetTreeHeight(root);

            contentToResize.position = new Vector3(contentToResize.position.x, treeRootPosition.y 
                                                                               - yDistanceFromNode * treeNodeHeight / 2);
            
            InitContentCorners();
            ResizeStretchedContent();
        }

        private void ResizeStretchedContent()
        {
            contentToResize.offsetMax = new Vector3(contentToResize.offsetMax.x, 
                contentToResize.offsetMax.y + 120f);
            
            var offsetBot = contentCorners[3].position.y;
            var topAnchor = new Vector2(0.5f, 1f);
            
            SetAnchorAndPivotOfTalentElements(topAnchor);
            
            contentToResize.offsetMin = new Vector2(contentToResize.offsetMin.x, 
                contentToResize.offsetMin.y + offsetBot);
            
            SetAnchorAndPivotOfTalentElements(Vector2.one / 2f);
        }

        private void SetAnchorAndPivotOfTalentElements(Vector2 value)
        {
            foreach (var link in createdLinkUIs)
            {
                var transf = (RectTransform) link.transform;

                transf.SetAnchors(value, value);
                transf.SetPivot(value);
            }

            foreach (var node in createdTalentUIs)
            {
                var transf = (RectTransform) node.transform;

                transf.SetAnchors(value, value);
                transf.SetPivot(value);
            }
        }

        private void ResizeContentBasedOnLimits()
        {
            var rightLim = contentCorners[0].position.x;
            var topLim = contentCorners[1].position.y;
            var leftLim = contentCorners[2].position.x;
            var botLim = contentCorners[3].position.y;

            var width = Math.Abs(rightLim - leftLim);
            var height = Math.Abs(topLim - botLim);

            contentToResize.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + bonusWidth);
            contentToResize.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + bonusHeight);
            
            contentToResize.ForceUpdateRectTransforms();

            var leftOverWidth = Math.Abs(contentToResize.rect.width / 2f - leftLim);
            var leftOverHeight = Math.Abs(contentToResize.rect.height / 2f - botLim);

            if (leftOverWidth > 10f)
            {
                contentToResize.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + bonusWidth
                + leftOverWidth);
            }

            if (leftOverHeight > 10f)
            {
                contentToResize.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + bonusHeight
                + leftOverHeight);
            }
        }

        private void InitContentCorners()
        {
            // max x
            contentCorners[0] = (RectTransform) createdTalentUIs
                .OrderByDescending(x => ((RectTransform) x.transform).position.x).First().transform;
            // max y
            contentCorners[1] = (RectTransform) createdTalentUIs
                .OrderByDescending(x => ((RectTransform) x.transform).position.y).First().transform;
            // min x
            contentCorners[2] =
                (RectTransform) createdTalentUIs.OrderBy(x => ((RectTransform) x.transform).position.x).First().transform;
            // min y
            contentCorners[3] =
                (RectTransform) createdTalentUIs.OrderBy(x => ((RectTransform) x.transform).position.y).First().transform;
        }

        private void CreateTreeFromRoot()
        {
            var linkList = new List<DynamicTalentLinkUI>();
            
            var rootUi = CreateNodeUI(root, treeRootPosition);

            treeRootPosition = rootUi.transform.position;

            createdTalentUIs.Add(rootUi);

            for (int i = 0; i < root.Links.Count; i++)
            {
                var link = root.Links[i];

                linkList.AddRange(CreateSubtree(i, root, rootUi, link.Destination, link, 1));
            }
            
            rootUi.Init(root, 10, linkList, UnlockState.Unlockable);
        }

        private List<DynamicTalentLinkUI> CreateSubtree(int index, DynamicTalent parentTalent, 
            DynamicTalentUI parent, DynamicTalent nodeToCreate, TalentLink linkToParent, int depth = 1)
        {
            var linkList = new List<DynamicTalentLinkUI>();

            if (nodeToCreate == null)
            {
                return linkList;
            }
            
            var thisNodesLinks = new List<DynamicTalentLinkUI>();

            var parentPos = parent.transform.position;
            
            var signedInd = parentTalent.Links.Count / 2 - index;
            var thisNodeUi = CreateNodeUI(nodeToCreate, new Vector3(
                parentPos.x + signedInd * xDistanceFromNode
                , treeRootPosition.y - yDistanceFromNode * depth, 0f));
            
            createdTalentUIs.Add(thisNodeUi);

            var newLinkUi = CreateLinkBetweenNodes(parent, thisNodeUi, linkToParent);
            linkList.Add(newLinkUi);

            for (int i = 0; i < nodeToCreate.Links.Count; i++)
            {
                var link = nodeToCreate.Links[i];

                thisNodesLinks.AddRange(CreateSubtree(i, nodeToCreate, thisNodeUi, link.Destination,
                    link, depth + 1));
            }
            thisNodeUi.Init(nodeToCreate, 10, thisNodesLinks);
            
            return linkList;
        }

        private DynamicTalentUI CreateNodeUI(DynamicTalent nodeBase, Vector3 pos)
        {
            var talentUiClone = Instantiate(dynamicTalentUIPrefab, pos, Quaternion.identity);

            var talentUiRectTransf = (RectTransform) talentUiClone.transform;

            talentUiRectTransf.SetParent(talentsContainer);
            talentUiClone.name = nodeBase.Name;

            // talentUiRectTransf.anchorMin = new Vector3(0.5f, 0f);
            // talentUiRectTransf.anchorMax = new Vector3(0.5f, 0f);
            // talentUiRectTransf.pivot = new Vector3(0.5f, 0f);

            talentUiClone.GetComponent<SelectDynamicTalentButton>().DynamicTalentOverviewUI = dynamicTalentOverviewUI;
            
            return talentUiClone;
        }

        private DynamicTalentLinkUI CreateLinkBetweenNodes(DynamicTalentUI parent, DynamicTalentUI destination,
            TalentLink link)
        {
            var parentPos = parent.transform.position;
            var destinationPos = destination.transform.position;

            var midPoint = new Vector3(parentPos.x - destinationPos.x,
                Math.Max(parentPos.y, destinationPos.y), 0f);

            var linkClone = Instantiate(dynamicLinkUIPrefab, midPoint, Quaternion.identity);

            var linkTransf = (RectTransform) linkClone.transform;
            
            linkTransf.SetParent(linksContainer);
            linkTransf.localPosition = Vector3.zero;
            
            createdLinkUIs.Add(linkClone);

            var linkPartsParent = linkTransf.GetChild(0);
            linkPartsParent.localPosition = Vector3.zero;

            var parts = CreateLinkParts(parent, destination, linkPartsParent);
            linkClone.Init(destination, link, parts);

            return linkClone;
        }

        private List<DynamicTalentLinkPartUI> CreateLinkParts(DynamicTalentUI node1, DynamicTalentUI node2,
            Transform partsParent)
        {
            List<DynamicTalentLinkPartUI> parts = new List<DynamicTalentLinkPartUI>();
            
            var node1RectTransf = (RectTransform) node1.transform;
            var node2RectTransf = (RectTransform) node2.transform;

            var node1Pos = node1RectTransf.anchoredPosition;
            var node2Pos = node2RectTransf.anchoredPosition;

            var xIsDiff = !Mathf.Approximately(node1Pos.x, node2Pos.x);
            var yIsDiff = !Mathf.Approximately(node1Pos.y, node2Pos.y);

            Vector3 pt1NewPos = node1Pos;

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
                pt2.IsY = true;
                
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

        private int GetTreeHeight(DynamicTalent root)
        {
            if (root == null) return -1;

            int[] heights = new int[root.Links.Count];

            for (int i = 0; i < root.Links.Count; i++)
            {
                var link = root.Links[i];

                heights[i] = GetTreeHeight(link.Destination) + 1;
            }

            return heights.Length > 0 ? heights.Max() : 0;
        }
    }
}