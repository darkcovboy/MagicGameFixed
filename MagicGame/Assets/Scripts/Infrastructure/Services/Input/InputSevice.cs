using UnityEngine;

namespace Services.Input
{
    public class InputSevice : IInputSevice
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        private const int LeftMouseButton = 0;
        private const string MouseX = "Mouse X";
        private const string MouseY = "Mouse Y";

        public Vector2 MouseAxis { get; }
        public Vector3 Axis => UnityAxis();

        private static Vector3 UnityAxis()
        {
            return new Vector3(UnityEngine.Input.GetAxis(Horizontal),0f, UnityEngine.Input.GetAxis(Vertical));
        }
        
        private static Vector2 UnityAxisMouse() =>
        new Vector2(UnityEngine.Input.GetAxis(MouseX), UnityEngine.Input.GetAxis(MouseY));

        public bool IsAttackedButtonUp() => UnityEngine.Input.GetMouseButton(LeftMouseButton);
    }
}