using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Zenject;
using Base;

namespace Models
{
    public class MainMenuModel : Model
    {
        public string TargetGameScene { get; private set; } = "Game";
    }
}