using System;
using UnityEngine;

namespace Card
{
    public class CardData
    {
        public string Id { get; private set; }
        public Sprite Sprite { get; private set; }

        public CardData(string id, Sprite sprite)
        {
            Id = id;
            Sprite = sprite;
        }
    }
}
