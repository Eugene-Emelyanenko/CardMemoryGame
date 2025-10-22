namespace Card
{
    [System.Serializable]
    public class CardJsonEntry
    {
        public string id;
        public string url;
    }

    [System.Serializable]
    public class CardJsonRoot
    {
        public CardJsonEntry[] images;
    }
}