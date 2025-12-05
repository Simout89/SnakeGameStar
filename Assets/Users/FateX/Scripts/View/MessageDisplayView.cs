using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Users.FateX.Scripts.View
{
    public class MessageDisplayView: MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private Vector3 originPos;
        private Queue<string> messageQueue = new Queue<string>();
        private bool isShowing = false;
        
        private void Awake()
        {
            originPos = _text.transform.localScale;
        }

        public void ShowText(string text)
        {
            messageQueue.Enqueue(text);
            
            if (!isShowing)
            {
                ShowNextMessage();
            }
        }

        private void ShowNextMessage()
        {
            if (messageQueue.Count == 0)
            {
                isShowing = false;
                return;
            }

            isShowing = true;
            string text = messageQueue.Dequeue();
            
            _text.transform.localPosition = originPos;
            _text.gameObject.SetActive(true);
            _text.text = text;
            _text.transform.localScale = Vector3.zero;
            
            var seq = DOTween.Sequence();
            seq.Append(_text.transform.DOScale(Vector3.one, 0.2f));
            seq.Join(_text.DOFade(1, 0.2f));
            seq.AppendInterval(4f);
            seq.Append(_text.DOFade(0, 1f));
            // seq.Join(_text.transform.DOScale(Vector3.zero, 1f));
            seq.OnComplete(ShowNextMessage);
        }
    }
}