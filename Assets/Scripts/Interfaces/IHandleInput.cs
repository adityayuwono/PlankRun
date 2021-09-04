using System;

namespace Assets.Scripts.Interfaces
{
    public interface IHandleInput
    {
        void ProcessForRotation(Action<float> OnRotate);
    }
}
