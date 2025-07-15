
namespace CardData
{ 
    public enum CardColor { black, Blue, Green, Oragne }
    public enum CardIcon { icon1, icon2, icon3, icon4, icon5, icon6, icon7, icon8 }

    // card data structure:
    [System.Serializable]
    public struct CardData
    {
        public CardColor color;
        public CardIcon icon;

        public int Id => ((int)color) * 8 + (int)icon;  // unique ID based on color and icon
    }
}