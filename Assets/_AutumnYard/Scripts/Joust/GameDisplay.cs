using UnityEngine;
using UnityEngine.UI;

namespace AutumnYard.Joust
{
    public sealed class GameDisplay : MonoBehaviour
    {
        [SerializeField] private Image[] boardPiecesTop = new Image[3];
        [SerializeField] private Image[] boardPiecesBottom = new Image[3];
        [SerializeField] private Color[] colors = new Color[4];
        [SerializeField] private Text[] points = new Text[2];
        [SerializeField] private Image[] initiatives = new Image[2];

        private void OnValidate()
        {
            if (boardPiecesTop == null || boardPiecesTop.Length == 0) boardPiecesTop = new Image[3];
            if (boardPiecesBottom == null || boardPiecesBottom.Length == 0) boardPiecesBottom = new Image[3];
            if (colors == null || colors.Length == 0) colors = new Color[4];
            if (points == null || points.Length == 0) points = new Text[2];
            if (initiatives == null || initiatives.Length == 0) initiatives = new Image[2];
        }

        internal void Display(in Joust game)
        {
            for (int i = 0; i < 3; i++)
            {
                boardPiecesTop[i].color = colors[(int)game.Board[0, i]];
                boardPiecesBottom[i].color = colors[(int)game.Board[1, i]];
            }

            for (int i = 0; i < 2; i++)
            {
                points[i].text = game.Points[i].ToString();
            }

            initiatives[0].enabled = game.InitiativePlayerA;
            initiatives[1].enabled = !game.InitiativePlayerA;
        }
    }
}
