namespace Signals
{
    public static class CardGameSignals
    {
        public sealed class CardClicked { public int Index; public CardClicked(int index){ Index=index; } }
    }
}