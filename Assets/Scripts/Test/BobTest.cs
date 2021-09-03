using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Talent_Tree
{
	public class BobTest : MonoBehaviour
	{
		[SerializeField] private Transform bobTransform = default;
		
	    void Awake()
	    {
		    prevResult = bobTransform.localScale;
		    
		    DOTween.Init(false, true, LogBehaviour.Verbose);
		    DOTween.defaultAutoPlay = AutoPlay.None;
	    }

	    private void Update()
	    {
		    if (Input.GetKeyDown(KeyCode.Space))
		    {
			    // tween
			    var bobScale = bobTransform.localScale;
			    var scaleX = bobScale.x;

			    DOTween.To(() => scaleX, val => scaleX = val, 5, 2f)
				    .onUpdate += () => bobTransform.localScale = new Vector3(scaleX, bobScale.y, bobScale.z);
		    }
	    }

	    #region debug
	    [SerializeField] private float newScale = default;
	    [SerializeField] private float dur = default;
	    [SerializeField] private bool isY = false;

	    // This should work for queueing the tweening for parts
	    private readonly Queue<Tweener> tweeners = new Queue<Tweener>();
	    
	    private Tweener curTweener = default;
	    private Vector3 prevResult = default;

	    [ContextMenu("Add a tween")]
	    private void AddTween()
	    {
		    
		    
		    EnqueueTween(isY, newScale, dur);
	    }

	    private void EnqueueTween(bool isY, float newScale, float dur)
	    {
		    Vector3 endVal = new Vector3(isY ? prevResult.x : newScale, isY ? newScale : prevResult.y, prevResult.z);
		    prevResult = endVal;
		    
		    Tweener tween = bobTransform.DOScale(endVal, dur);

		    tween.onComplete += () =>
		    {
			    if (tweeners.Count > 0)
			    {
				    curTweener = tweeners.Dequeue();
				    curTweener.Play();
			    }
			    else curTweener = null;
		    };
		    
		    tweeners.Enqueue(tween);

		    if (tweeners.Count > 0 && curTweener == null)
		    {
			    curTweener = tweeners.Dequeue(); 
			    curTweener.Play();
		    }
	    }

	    #endregion
	}
}
