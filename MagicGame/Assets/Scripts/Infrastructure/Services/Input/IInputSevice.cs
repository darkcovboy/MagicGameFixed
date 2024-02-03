using Infrastructure.Services;
using UnityEngine;

namespace Services.Input
{
    public interface IInputSevice : IService
    {
        Vector2 MouseAxis { get; }
        Vector3 Axis { get; }
        bool IsAttackedButtonUp();
    }
}