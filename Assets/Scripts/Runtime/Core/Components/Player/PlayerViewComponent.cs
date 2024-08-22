using SA.Runtime.Core.Data.Configs;
using SA.Runtime.Core.Views;
using UnityEngine;

namespace SA.Runtime.Core.Components
{
    public struct PlayerViewComponent
    {
        public Rigidbody RB;
        public SnakeTongueView Tongue;
        public SnakeConfig Config;
        public Transform TailRoot;
        public MeshRenderer BodyRenderer;
        public Color MyBodyColor;
    }
}