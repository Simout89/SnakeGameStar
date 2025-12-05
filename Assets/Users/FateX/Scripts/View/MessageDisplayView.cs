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
        [SerializeField] private Color _defaultColor = Color.white;
        
        private Vector3 originPos;
        private Queue<MessageData> messageQueue = new Queue<MessageData>();
        private bool isShowing = false;
        
        private struct MessageData
        {
            public string Text;
            public Color Color;
        }
        
        private void Awake()
        {
            originPos = _text.transform.localScale;
        }

        public void ShowText(string text)
        {
            ShowText(text, _defaultColor);
        }

        public void ShowText(string text, Color color)
        {
            messageQueue.Enqueue(new MessageData { Text = text, Color = color });
            
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
            MessageData data = messageQueue.Dequeue();
            
            _text.transform.localPosition = originPos;
            _text.gameObject.SetActive(true);
            _text.text = data.Text;
            _text.color = data.Color;
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