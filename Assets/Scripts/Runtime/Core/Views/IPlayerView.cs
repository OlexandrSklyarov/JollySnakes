
using SA.Runtime.Core.Data.Configs;
using UnityEngine;

namespace SA.Runtime.Core.Views
{
    public interface IPlayerView
    {
        public Rigidbody RB { get; }
        public SnakeConfig Config { get; }         
    }
}