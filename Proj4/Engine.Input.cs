using System;
using System.Collections.Generic;
using Tao.Sdl;

namespace Aura
{
    public partial class Engine
    {
        public void HandleInput(Sdl.SDL_Event sdl_event)
        {
            switch (sdl_event.type)
            {
                case Sdl.SDL_QUIT:
                    Dispose();
                    break;
            }
        }


    }

    public enum KeyState { Pressed, Released }
    public enum KeyModifiers { Shift, Control, Alt }
    public enum ControlKey { Escape, Enter, Backspace, Delete, Insert, F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12 }
    public delegate void QuitEventHandler();
    public delegate void TextKeyboardEventHandler(char key, KeyModifiers modifiers, KeyState state);
    public delegate void ControlKeyboardEventHandler(ControlKey key, KeyState state);
}
