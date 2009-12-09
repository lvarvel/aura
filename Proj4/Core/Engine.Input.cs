using System;
using System.Collections.Generic;
using Tao.Sdl;
using Aura.Graphics.Assets;

namespace Aura.Core
{
    public partial class Engine
    {
        public void HandleInput(Sdl.SDL_Event sdl_event)
        {
            switch (sdl_event.type)
            {
                case Sdl.SDL_QUIT:
                    Dispose();
                    return;
                case Sdl.SDL_KEYDOWN:
                    if (sdl_event.key.keysym.sym == Sdl.SDLK_ESCAPE)
                    {
                        Dispose();
                        return;
                    }
                    else if (sdl_event.key.keysym.sym == 'n')
                    {
                        rotation += (float)(Math.PI / 100);
                        Vector3 newPos = new Vector3((float)Math.Cos(rotation), 0, (float)Math.Sin(rotation));
                        newPos = newPos * 25;
                        newPos.Y = 10;
                        CameraManager.Current.position = newPos;
                    }
                    else if (sdl_event.key.keysym.sym == 'm')
                    {
                        rotation -= (float)(Math.PI / 100);

                        Vector3 newPos = new Vector3((float)Math.Cos(rotation), 0, (float)Math.Sin(rotation));
                        newPos = newPos * 25;
                        newPos.Y = 10;
                        CameraManager.Current.position = newPos;
                    }
                    break;
                case Sdl.SDL_MOUSEBUTTONDOWN:
                    if (sdl_event.button.button == Sdl.SDL_BUTTON_LEFT)
                    {
                        this.mrExplody = new Explosion(new Vector3(1, 1, 1), 0f, 5.0f);
                        return;
                    }
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
