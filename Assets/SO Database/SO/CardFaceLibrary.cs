using UnityEngine;

[CreateAssetMenu(fileName = "CardFaceLibrary", menuName = "Scriptable Objects/CardFaceLibrary")]
public class CardFaceLibrary : ScriptableObject
{
    public Sprite[] cardSprites = new Sprite[8];

    public Color[] cardColors = new Color[4];
}