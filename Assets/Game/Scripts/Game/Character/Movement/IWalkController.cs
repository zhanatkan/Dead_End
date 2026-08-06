using System;
using Game.Scripts.Game.Character.Base;
using UnityEngine;

namespace Game.Scripts.Game.Character.Movement
{
    public interface IWalkController
    {
        Vector2 Direction { get; set; }
        float WalkSpeed { get; set; }
        
        void Update();
    }
}