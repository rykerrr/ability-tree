using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Talent_Tree.Dynamic_Talent_Tree
{
    public class DynamicTalentLinkUI : MonoBehaviour
    {
        [SerializeField] private DynamicTalentUI destination = default;
        [SerializeField] private int weightRequiredInBase = 1;
        [SerializeField] private List<DynamicTalentLinkPartUI> parts = default;
        
        public DynamicTalentUI Destination => destination;
        public bool CanTraverse(int points) => points >= weightRequiredInBase;
        
        private float fullLength = 0;

        private readonly Queue<Tweener> tweeners = new Queue<Tweener>();
        private Tweener curTweener = default;

        private void Awake()
        {
            DOTween.Init(false, false, LogBehaviour.Verbose);
            DOTween.defaultAutoPlay = AutoPlay.None;
        }
        
        public void Init(DynamicTalentUI destination, TalentLink talentLink, List<DynamicTalentLinkPartUI> parts)
        {
            this.destination = destination;
            weightRequiredInBase = talentLink.WeightRequiredInBase;

            this.parts = parts;
            InitParts();
        }
        
        private void InitParts()
        {
            foreach (var part in parts)
            {
                fullLength += part.Length;

                part.SetFillInstant(0);
            }
        }
        
        public void UpdateFill(int baseLevel)
        {
            var fill = Mathf.Clamp01((float)baseLevel / weightRequiredInBase);
            var lenWithFill = fill * fullLength;
            
            foreach (var part in parts)
            {
                if (part.IsFilled) continue;
                
                if (lenWithFill >= part.Length)
                {
                    lenWithFill -= part.Length;
                    
                    var tweener = part.SetFill(1);
                    
                    tweener.onComplete += () =>
                    {
                        if (tweeners.Count > 0)
                        {
                            curTweener = tweeners.Dequeue();
                            curTweener.Play();
                        }
                        else curTweener = null;
                    };
                    
                    tweeners.Enqueue(tweener);
                }
                else
                {
                    var fillLength = lenWithFill / part.Length;

                    var tweener = part.SetFill(fillLength);
                    
                    tweener.onComplete += () =>
                    {
                        if (tweeners.Count > 0)
                        {
                            curTweener = tweeners.Dequeue();
                            curTweener.Play();
                        }
                        else curTweener = null;
                    };
                    
                    tweeners.Enqueue(tweener);
                    
                    break;
                }
            }
            
            if (tweeners.Count > 0 && curTweener == null)
            {
                curTweener = tweeners.Dequeue(); 
                curTweener.Play();
            }
        }
    }
}