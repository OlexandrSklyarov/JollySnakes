using Cinemachine;
using SA.Runtime.Core.Data.Configs;
using SA.Runtime.Core.Services;
using SA.Runtime.Core.Services.Factories;
using SA.Runtime.Core.Services.Input;
using SA.Runtime.Core.Services.Time;

namespace SA.Runtime.Core.Data
{
    public class SharedData
    {
        public GameConfig GameConfig;
        public TimeService TimeService;
        public IInputService InputService;
        public CinemachineFreeLook FollowCamera;
        public IUnitFactory UnitFactory;
        public IPhysicsOverlapService OverlapService;
        public EmittersRoot EmittersRoot;
    }
}