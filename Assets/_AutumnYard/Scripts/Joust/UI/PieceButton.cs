using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AutumnYard.Joust
{
    [RequireComponent(typeof(Button))]
    public class PieceButton : MonoBehaviour
    {
        private Button _button;
        [SerializeField] private Player player;
        [SerializeField] private int bout;
        [SerializeField] private Image image;

        private void Awake()
        {
            _button = GetComponent<Button>();

            if (image == null) image = GetComponentInChildren<Image>();
        }

        public void TogglePiece()
        {
            GameDirector.Instance.TogglePiece(player, bout);
        }
        public void SetColor(Color color)
        {
            image.color = color;
        }
    }
}
