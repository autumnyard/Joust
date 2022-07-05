using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.Joust
{
    [CreateAssetMenu(fileName = "Piece Configuration", menuName = "Autumn Yard/Piece Configuration", order = 1)]
    public class PieceConfiguration : ScriptableObject
    {
        [SerializeField] private Color[] colors = new Color[4];
    }
}
