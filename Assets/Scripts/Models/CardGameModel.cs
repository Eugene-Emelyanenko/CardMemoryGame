using System.Collections;
using System.Collections.Generic;
using Base;
using Card;
using Global;
using UnityEngine;
using UnityEngine.Networking;

namespace Models
{
    public class CardGameModel : Model
    {
        private readonly SpriteStorage _storage;

        public List<CardData> GameCards { get; private set; }
        public CardData CardBack { get; private set; }

        public readonly int totalPairs = 3;
        public readonly float showTime = 5f;
        public readonly float waitTime = 1f;

        public CardGameModel(SpriteStorage storage)
        {
            _storage = storage;
        }

        public override void Initialize()
        {
            base.Initialize();
            GameCards = _storage.GetFaces();
            CardBack  = _storage.GetBack();
        }
    }
}