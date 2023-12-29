using VContainer.Unity;

namespace SA.Runtime.Core.Services.Time
{
    public sealed class TimeService : ITickable
    {
        public float Time;
        public float DeltaTime;
        public float FixedDeltaTime;
        public float UnscaledDeltaTime;
        public float UnscaledTime;       

        void ITickable.Tick()
        {
            Time = UnityEngine.Time.time;
            UnscaledTime = UnityEngine.Time.unscaledTime;
            DeltaTime = UnityEngine.Time.deltaTime;
            FixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
            UnscaledDeltaTime = UnityEngine.Time.unscaledDeltaTime;
        }
    }
}