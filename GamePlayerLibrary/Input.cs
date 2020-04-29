using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Game_Player
{
    /// <summary>
    /// Possible Keystates to which to compare an Input.GetState() call.
    /// </summary>
    public enum InputState 
    { 
        /// <summary>
        /// Indicated that the key has been up for more than one frame.
        /// </summary>
        Lifted, 

        /// <summary>
        /// Indicates that the key has been down for more than one frame.
        /// </summary>
        Held, 

        /// <summary>
        /// Indicated that the hey was just pressed down.
        /// </summary>
        Triggered, 

        /// <summary>
        /// Indicated that the key was just lifted up.
        /// </summary>
        Released
    }

    /// <summary>
    /// A class that handles input from the keyboard.
    /// </summary>
    public class Input
    {
        const int KEYHOLDTIME = 20;
        const int KEYCHECK = 4;


        private static int dir = 0;
        private static int primaryDir = 0;

        static Dictionary<Keys, InputState> states = new Dictionary<Keys, InputState>();
        static Dictionary<Keys, int> held = new Dictionary<Keys, int>();

        public static int MouseX { get { return Mouse.GetState().X; } }
        public static int MouseY { get { return Mouse.GetState().Y; } }

        public static int MouseScroll { get; private set; }
        private static int lastMouseScroll = 0;

        public static InputState LeftMouseState { get; private set; }
        public static InputState MiddleMouseState { get; private set; }
        public static InputState RightMouseState { get; private set; }

        static Input()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                states[key] = InputState.Lifted;
            }
        }

        public static Vector2 GetMousePosition()
        {
            MouseState mouse = Mouse.GetState();
            return new Vector2(mouse.X, mouse.Y);
        }

        /// <summary>
        /// Updates the Input Class. Generally this is done through <c>Globals.GameSystem.Update()</c>,
        /// and does not need to be explicitly called.
        /// </summary>
        public static void Update()
        {
            MouseState mouseState = Mouse.GetState();
            LeftMouseState = UpdateInputState(mouseState.LeftButton == ButtonState.Pressed, LeftMouseState);
            //Console.WriteLine(LeftMouseState);
            RightMouseState = UpdateInputState(mouseState.RightButton == ButtonState.Pressed, RightMouseState);
            MiddleMouseState = UpdateInputState(mouseState.MiddleButton == ButtonState.Pressed, MiddleMouseState);

            MouseScroll = mouseState.ScrollWheelValue - lastMouseScroll;
            lastMouseScroll = mouseState.ScrollWheelValue;

            KeyboardState keystate = Keyboard.GetState();
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                bool down = keystate.IsKeyDown(key);
                states[key] = UpdateInputState(down, states[key]);
                held[key] = down ? held[key] + 1 : 0;
            }
        }

        private static InputState UpdateInputState(bool isDown, InputState oldState)
        {
            if (isDown)
            {
                if (oldState == InputState.Released || oldState == InputState.Lifted) return InputState.Triggered;
                return InputState.Held;
            }
            else
            {
                if (oldState == InputState.Held || oldState == InputState.Triggered) return InputState.Released;
                return InputState.Lifted;
            }
        }

        public static InputState GetKeystate(Keys key)
        {
            return states[key];
        }

        public static int HoldLength(Keys key)
        {
            return held[key];
        }
        
        /// <summary>
        /// Indicates if the given key is in the Held state. This includes all time after the key is
        /// pressed and continues to be pressed.
        /// Note, this is a shortcut method equivalent to: "State(key) == KeyStates.Held;"
        /// </summary>
        /// <param name="key">They key to check.</param>
        /// <returns>The indicating boolean.</returns>
        public static Boolean Held(Keys key)
        { 
            return GetKeystate(key) == InputState.Held; 
        }

        public static Boolean Down(Keys key)
        {
            return Held(key) || Triggered(key);
        } 

        /// <summary>
        /// Indicates if the given key is in the Triggered state. This includes only the
        /// initial frame the key is pressed.
        /// Note, this is a shortcut method equivalent to: "State(key) == KeyStates.Triggered;"
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>The indicating boolean.</returns>
        public static Boolean Triggered(Keys key)
        { 
            return GetKeystate(key) == InputState.Triggered; 
        }

        /// <summary>
        /// This method is used to trigger an event while a key is being held. Instead of simply 
        /// returning whether the key is being held or not, (~60 times a second) it will return 
        /// true once upon triggering, then after waiting 20 frames, once every 10 frames (~6
        /// time a second).
        /// Use this method for scrolling, etc. Imagine a cursor in a text document.
        /// </summary>
        /// <param name="key">They key to check.</param>
        /// <returns>The indicating boolean.</returns>
        public static Boolean Repeated(Keys key)
        {           
            int hold = HoldLength(key);
            return (hold % KEYCHECK == 0) && (hold > KEYHOLDTIME) || Triggered(key);
        }

        public static Vector2 GetWASDDir()
        {
            return GetDir(Keys.W, Keys.A, Keys.S, Keys.D);
        }

        public static Vector2 GetArrowDir()
        {
            return GetDir(Keys.Up, Keys.Left, Keys.Down, Keys.Right);
        }

        private static Vector2 GetDir(Keys up, Keys left, Keys down, Keys right)
        {
            Vector2 point = new Vector2();
            // Remember that up is -Y in screen coordinates
            if (Down(up)) point.Y--;
            if (Down(down)) point.Y++;
            if (Down(left)) point.X--;
            if (Down(right)) point.X++;
            return point;
        }
    }
}
