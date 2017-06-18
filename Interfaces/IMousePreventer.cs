namespace RazerDeathadderFix.Interfaces
{
    using System;

    public interface IMousePreventer
    {
        bool IsMouseBounce(IntPtr wParam);

        void Deactivate();

        event EventHandler OnMouseBounce;
    };
}
