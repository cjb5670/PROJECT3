/*************************************************
 * InputManager.cs
 * 
 * This file contains:
 * - The InputManager class.
 * - The Controls enum.
 * - The ControlResponse enum.
 * ***********************************************/

/////////////////////
// Using statements.
/////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TRSS.Input
{

    #region InputManager class.

    /// <summary>
    /// The input manager singleton allows access to common input commands.
    /// </summary>
    public class InputManager : MonoBehaviour
    {

        #region Static members.

        // Singleton management.

        /// <summary>
        /// Global instance of the input manager.
        /// </summary>
        private static InputManager instance = null;

        /// <summary>
        /// Returns true when instance isn't equal to null.
        /// </summary>
        /// <returns>Returns a boolean of true if the instance exists.</returns>
        public static bool HasInstance()
        {
            return (instance != null);
        }

        /// <summary>
        /// Returns reference to the singleton instance of the input manager.
        /// </summary>
        /// <returns>Returns an InputManager object.</returns>
        public static InputManager GetInstance()
        {
            if (!HasInstance())
            {
                instance = Create(null);
            }

            return instance;
        }
        
        /// <summary>
        /// Creates and adds the input manager to supplied game object. If there is no object supplied, a new game object will be provided.
        /// </summary>
        /// <param name="_parent">GameObject to which this component is being added to.</param>
        /// <returns>Returns reference to the input manager component.</returns>
        public static InputManager Create(GameObject _parent = null)
        {
            InputManager manager = instance;

            if (manager == null)
            {
                manager = Services.CreateComponent<InputManager>(_parent);
            }

            return manager;
        }

        #endregion

        #region Data Members

        #region Properties

        /// <summary>
        /// Property flag that will return whether or not its been initialized.
        /// </summary>
        public bool Initialized
        {
            get { return this.m_initialized; }
        }

        /// <summary>
        /// Returns collection of controls.
        /// </summary>
        public List<Controls> Controls
        {
            get
            {
                if (this.m_controls == null)
                {
                    this.m_controls = InitializeControls();
                }
                return this.m_controls;
            }
        }

        /// <summary>
        /// Returns collection of control types, mapped to controls.
        /// </summary>
        public Dictionary<Controls, ControlType> ControlTypes
        {
            get
            {
                if (this.m_controlTypes == null)
                {
                    this.m_controlTypes = InitializeControlTypes();
                }
                return this.m_controlTypes;
            }
        }

        /// <summary>
        /// Returns collection of control keycodes, mapped to controls.
        /// </summary>
        public Dictionary<Controls, KeyCode> KeyCodes
        {
            get
            {
                if (this.m_keycodes == null)
                {
                    this.m_keycodes = InitializeKeyCodes();
                }
                return this.m_keycodes;
            }
        }

        /// <summary>
        /// Returns collection of control commands, mapped to controls.
        /// </summary>
        public Dictionary<Controls, string> Commands
        {
            get
            {
                if (this.m_inputs == null)
                {
                    this.m_inputs = InitializeCommands();
                }
                return this.m_inputs;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Initialization flag.
        /// </summary>
        private bool m_initialized = false;

        /// <summary>
        /// Controls collection.
        /// </summary>
        private List<Controls> m_controls = null;

        /// <summary>
        /// Control type map.
        /// </summary>
        private Dictionary<Controls, ControlType> m_controlTypes = null;

        /// <summary>
        /// KeyCode map.
        /// </summary>
        private Dictionary<Controls, KeyCode> m_keycodes = null;

        /// <summary>
        /// Button and axes input map.
        /// </summary>
        private Dictionary<Controls, string> m_inputs = null;

        #endregion

        #endregion

        #region Input Methods

        #region General Input 

        /// <summary>
        /// Return trigger state as true or false.
        /// </summary>
        /// <param name="_control">Control to check for.</param>
        /// <param name="_response">Response mode to check for.</param>
        /// <returns>Returns a true, if the control has a value that is different from zero.</returns>
        public bool IsTriggered(Controls _control, ControlResponse _response)
        {
            float value = 0.0f;
            return IsTriggeredValue(_control, _response, out value);
        }

        /// <summary>
        /// Checks to see if a particular input value has been activated, matching the control response mode.
        /// </summary>
        /// <param name="_control">Control to check for.</param>
        /// <param name="_response">Response mode to check for.</param>
        /// <param name="_value">Value that returns the input value if the response type happens to be an axis.</param>
        /// <returns>Returns a true, if the control has a value that is different from zero.</returns>
        public bool IsTriggeredValue(Controls _control, ControlResponse _response, out float _value)
        {
            _value = 0.0f; // Set this value to zero, as a default.

            // First, check response types.
            switch (_response)
            {
                case ControlResponse.HELD:
                    return IsTriggerHeld(_control, out _value);
                case ControlResponse.PRESSED:
                    return IsTriggerPressed(_control, out _value);
                case ControlResponse.RELEASED:
                    return IsTriggerReleased(_control, out _value);
                case ControlResponse.UP:
                    return IsTriggerUp(_control, out _value);
                case ControlResponse.AXIS:
                    return IsTriggerAxis(_control, out _value);
                case ControlResponse.AXIS_RAW:
                    return IsTriggerAxisRaw(_control, out _value);
                case ControlResponse.NONE:
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks to see if the control matches the response mode and returns the appropriate value.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <param name="_value">Value to return given input state.</param>
        /// <returns>Returns a boolean, if value is non-zero.</returns>
        private bool IsTriggerAxis(Controls _control, out float _value)
        {
            // If this is an axis, return the axis value.
            if (IsAxis(_control))
            {
                _value = GetValue(_control);
                return (_value != 0.0f);
            }

            _value = 0.0f; // Set to zero by default.
            return false; // If not held, return false.
        }
        
        /// <summary>
        /// Checks to see if the control matches the response mode and returns the appropriate value.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <param name="_value">Value to return given input state.</param>
        /// <returns>Returns a boolean, if value is non-zero.</returns>
        private bool IsTriggerAxisRaw(Controls _control, out float _value)
        {
            // If this is an axis, return the axis value.
            if (IsAxis(_control))
            {
                _value = GetRawValue(_control);
                return (_value != 0.0f);
            }

            _value = 0.0f; // Set to zero by default.
            return false; // If not held, return false.
        }

        /// <summary>
        /// Checks to see if the control matches the response mode and returns the appropriate value.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <param name="_value">Value to return given input state.</param>
        /// <returns>Returns a boolean, if value is non-zero.</returns>
        private bool IsTriggerHeld(Controls _control, out float _value)
        {
            _value = 0.0f; // Set this value to zero, as a default.

            if (IsHeld(_control))
            {
                _value = 1.0f;
                return true;
            }

            // If not held, return false.
            return false;
        }

        /// <summary>
        /// Checks to see if the control matches the response mode and returns the appropriate value.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <param name="_value">Value to return given input state.</param>
        /// <returns>Returns a boolean, if value is zero.</returns>
        private bool IsTriggerUp(Controls _control, out float _value)
        {
            _value = 1.0f; // Set this value to zero, as a default.

            if (IsUp(_control))
            {
                _value = 0.0f;
                return true;
            }

            // If not held, return false.
            return false;
        }

        /// <summary>
        /// Checks to see if the control matches the response mode and returns the appropriate value.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <param name="_value">Value to return given input state.</param>
        /// <returns>Returns a boolean, if value is zero.</returns>
        private bool IsTriggerReleased(Controls _control, out float _value)
        {
            _value = 1.0f; // Set this value to zero, as a default.

            if (IsReleased(_control))
            {
                _value = 0.0f;
                return true;
            }

            // If not held, return false.
            return false;
        }

        /// <summary>
        /// Checks to see if the control matches the response mode and returns the appropriate value.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <param name="_value">Value to return given input state.</param>
        /// <returns>Returns a boolean, if value is non-zero.</returns>
        private bool IsTriggerPressed(Controls _control, out float _value)
        {
            _value = 0.0f; // Set this value to zero, as a default.

            if (IsPressed(_control))
            {
                _value = 1.0f;
                return true;
            }

            // If not held, return false.
            return false;
        }

        /// <summary>
        /// Returns true if the control is a type of key or button and is being held down.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns boolean based on control state.</returns>
        private bool IsHeld(Controls _control)
        {
            // If control is a key, mouse button, or a joystick button.
            if (IsAKeyOrButton(_control))
            {
                // If it is a joystick button:
                if (HasCommand(_control))
                {
                    return UnityEngine.Input.GetButton(GetCommand(_control));
                }

                // If it is a keyboard or mouse button.
                if (HasKeyCode(_control))
                {
                    return UnityEngine.Input.GetKey(GetKeyCode(_control));
                }                
            }

            // An axis cannot be 'held' in the same way a traditional button or key can be.
            return false;
        }

        /// <summary>
        /// Returns true if the control is a type of key or button and is not being held down.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns boolean based on control state.</returns>
        private bool IsUp(Controls _control)
        {
            // If control is a key, mouse button, or a joystick button.
            if (IsAKeyOrButton(_control))
            {
                // Return the opposite of the IsHeld() method, if a key, mouse button, or joystick button.
                return !IsHeld(_control);
            }

            // An axis cannot be 'released' in the same way a traditional button or key can be.
            return false;
        }

        /// <summary>
        /// Returns true if the control is a type of key or button and has been released.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns boolean based on control state.</returns>
        private bool IsReleased(Controls _control)
        {
            // If control is a key, mouse button, or a joystick button.
            if (IsAKeyOrButton(_control))
            {
                // If it is a joystick button:
                if (HasCommand(_control))
                {
                    return UnityEngine.Input.GetButtonUp(GetCommand(_control));
                }

                // If it is a keyboard or mouse button.
                if (HasKeyCode(_control))
                {
                    return UnityEngine.Input.GetKeyUp(GetKeyCode(_control));
                }
            }

            // An axis cannot be 'released' in the same way a traditional button or key can be.
            return false;
        }
        
        /// <summary>
        /// Returns true if the control is a type of key or button and has been pressed down.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns boolean based on control state.</returns>
        private bool IsPressed(Controls _control)
        {
            // If control is a key, mouse button, or a joystick button.
            if (IsAKeyOrButton(_control))
            {
                // If it is a joystick button:
                if (HasCommand(_control))
                {
                    return UnityEngine.Input.GetButtonDown(GetCommand(_control));
                }

                // If it is a keyboard or mouse button.
                if (HasKeyCode(_control))
                {
                    return UnityEngine.Input.GetKeyDown(GetKeyCode(_control));
                }
            }

            // An axis cannot be 'released' in the same way a traditional button or key can be.
            return false;
        }
        
        /// <summary>
        /// Returns true if the axis is non-zero.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns boolean based on control state.</returns>
        private bool IsNonZero(Controls _control)
        {
            if (IsAxis(_control))
            {
                return (GetValue(_control) != 0.0f) ;           
            }

            // If control is a key, mouse button, or a joystick button.
            return false;
        }
        
        /// <summary>
        /// Returns true if the axis is zero.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns boolean based on control state.</returns>
        private bool IsZero(Controls _control)
        {
            if (IsAxis(_control))
            {
                return (GetValue(_control) == 0.0f);
            }

            // If control is a key, mouse button, or a joystick button.
            return false;
        }

        /// <summary>
        /// Returns axis value.
        /// </summary>
        /// <param name="_control">Control to check axis for.</param>
        /// <returns>Returns the value of the axis.</returns>
        private float GetValue(Controls _control)
        {
            // If it is an axis, return the value for it.
            if (IsAxis(_control))
            {
                return UnityEngine.Input.GetAxis(GetCommand(_control));
            }

            // If control is a key, mouse button, or a joystick button.
            return 0.0f;
        }
        
        /// <summary>
        /// Returns raw axis value.
        /// </summary>
        /// <param name="_control">Control to check axis for.</param>
        /// <returns>Returns the raw value of the axis.</returns>
        private float GetRawValue(Controls _control)
        {
            // If it is an axis, return the value for it.
            if (IsAxis(_control))
            {
                return UnityEngine.Input.GetAxisRaw(GetCommand(_control));
            }

            // If control is a key, mouse button, or a joystick button.
            return 0.0f;
        }

        #endregion

        #region Control methods.

        /// <summary>
        /// Returns true if associated with a keycode.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns boolean based on control type.</returns>
        public bool HasKeyCode(Controls _control)
        {
            return (IsAKey(_control) || IsMouseButton(_control));
        }

        /// <summary>
        /// Returns the keycode to check for.
        /// </summary>
        /// <param name="_control">Control to get associated key code for.</param>
        /// <returns>Returns the key code associated with the input control.</returns>
        public KeyCode GetKeyCode(Controls _control)
        {
            if (HasKeyCode(_control))
            {
                return this.KeyCodes[_control];
            }

            // If not a key nor mouse button, return the null keycode.
            return KeyCode.None;
        }

        /// <summary>
        /// Returns true if associated with a command string.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns boolean based on control type.</returns>
        public bool HasCommand(Controls _control)
        {
            return (IsAxis(_control) || IsJoystickButton(_control));
        }

        /// <summary>
        /// Returns the command string to check for.
        /// </summary>
        /// <param name="_control">Control to get associated command string for.</param>
        /// <returns>Returns the command string associated with the input control.</returns>
        public string GetCommand(Controls _control)
        {
            if (HasCommand(_control))
            {
                return this.Commands[_control];
            }

            // If not an axis nor joystick button, return an empty string.
            return "";
        }

        #endregion

        #region ControlType methods.

        /// <summary>
        /// Returns true if control is a type of button or key.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns true if it is a button or key.</returns>
        public bool IsAKeyOrButton(Controls _control)
        {
            return (IsAKey(_control) || IsMouseButton(_control) || IsJoystickButton(_control));
        }

        /// <summary>
        /// Returns true if control is a type of axis.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns true if it is an axis.</returns>
        public bool IsAxis(Controls _control)
        {
            return (IsMouseAxis(_control) || IsJoystickAxis(_control));
        }

        /// <summary>
        /// Determines if this is a type of key.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns true if it is a key.</returns>
        private bool IsAKey(Controls _control)
        {
            return (GetControlType(_control) == ControlType.Keyboard);
        }

        /// <summary>
        /// Determines if this is a type of mouse button.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns true if it is a mouse button.</returns>
        private bool IsMouseButton(Controls _control)
        {
            return (GetControlType(_control) == ControlType.MouseButton);
        }

        /// <summary>
        /// Determines if this is a type of mouse axis.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns true if it is a mouse axis.</returns>
        private bool IsMouseAxis(Controls _control)
        {
            return (GetControlType(_control) == ControlType.MouseAxis);
        }

        /// <summary>
        /// Determines if this is a type of joystick button.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns true if it is a joystick button.</returns>
        private bool IsJoystickButton(Controls _control)
        {
            return (GetControlType(_control) == ControlType.JoystickButton);
        }

        /// <summary>
        /// Determines if this is a type of joystick axis.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns true if it is a joystick axis.</returns>
        private bool IsJoystickAxis(Controls _control)
        {
            return (GetControlType(_control) == ControlType.JoystickAxis);
        }

        /// <summary>
        /// Returns the control type of the input control.
        /// </summary>
        /// <param name="_control">Control to check.</param>
        /// <returns>Returns a ControlType value.</returns>
        private ControlType GetControlType(Controls _control)
        {
            if (this.ControlTypes.ContainsKey(_control))
            {
                return this.ControlTypes[_control];
            }

            // Control isn't contained.
            return ControlType.NULL;
        }

        #endregion
                
        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Run, once at the start.
        /// </summary>
        public void Start()
        {
            // Check if instance already exists.
            if (!HasInstance())
            {
                instance = this;
            }
            else
            {
                // If instance already exists, destroy this component, if it isn't this one.
                if (instance != this)
                {
                    Destroy(this);
                    return;
                }
            }            
        }

        /// <summary>
        /// If the input manager hasn't been initialized, initialize it.
        /// </summary>
        public void Update()
        {
            if (!this.Initialized)
            {
                this.Initialize();
            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialize the InputManager.
        /// </summary>
        public void Initialize()
        {
            if (!this.Initialized)
            {
                // Create the collection mapping controls to control types.
                this.m_controls = this.Controls;
                this.m_controlTypes = this.ControlTypes;
                this.m_keycodes = this.KeyCodes;
                this.m_inputs = this.Commands;
                this.m_initialized = true;
            }
        }

        /// <summary>
        /// Initialize a collection of controls.
        /// </summary>
        /// <returns>Returns collection of controls.</returns>
        private List<Controls> InitializeControls()
        {
            List<Controls> response = new List<Controls>();

            #region Keyboard Controls.

            response.Add(Input.Controls.LEFT);
            response.Add(Input.Controls.RIGHT);
            response.Add(Input.Controls.UP);
            response.Add(Input.Controls.DOWN);

            response.Add(Input.Controls.KB_W);
            response.Add(Input.Controls.KB_A);
            response.Add(Input.Controls.KB_S);
            response.Add(Input.Controls.KB_D);

            response.Add(Input.Controls.KB_ESCAPE);
            response.Add(Input.Controls.KB_PAUSE);
            response.Add(Input.Controls.KB_SPACE);
            response.Add(Input.Controls.KB_BACKSPACE);
            response.Add(Input.Controls.KB_LSHIFT);

            #endregion

            #region Mouse Controls.

            response.Add(Input.Controls.LMB);
            response.Add(Input.Controls.RMB);
            response.Add(Input.Controls.MX);
            response.Add(Input.Controls.MY);

            #endregion

            #region Joystick Controls

            response.Add(Input.Controls.A);
            response.Add(Input.Controls.X);
            response.Add(Input.Controls.B);
            response.Add(Input.Controls.Y);

            response.Add(Input.Controls.LB);
            response.Add(Input.Controls.RB);
            response.Add(Input.Controls.LS);
            response.Add(Input.Controls.RS);

            response.Add(Input.Controls.BACK);
            response.Add(Input.Controls.SELECT);
            response.Add(Input.Controls.OPTIONS);
            response.Add(Input.Controls.START);
            
            response.Add(Input.Controls.LS_X);
            response.Add(Input.Controls.LS_Y);
            response.Add(Input.Controls.RS_X);
            response.Add(Input.Controls.RS_Y);
            response.Add(Input.Controls.DPAD_X);
            response.Add(Input.Controls.DPAD_Y);

            response.Add(Input.Controls.LTRIG);
            response.Add(Input.Controls.RTRIG);
            response.Add(Input.Controls.TRIGGERS);
            
            #endregion

            return response;
        }

        /// <summary>
        /// Initialize a collection of control types.
        /// </summary>
        /// <returns>Returns collection of control types.</returns>
        private Dictionary<Controls, ControlType> InitializeControlTypes()
        {
            Dictionary<Controls, ControlType> response = new Dictionary<Input.Controls, ControlType>();

            #region Keyboard Keys

            response.Add(Input.Controls.LEFT, Input.ControlType.Keyboard);
            response.Add(Input.Controls.RIGHT, Input.ControlType.Keyboard);
            response.Add(Input.Controls.UP, Input.ControlType.Keyboard);
            response.Add(Input.Controls.DOWN, Input.ControlType.Keyboard);

            response.Add(Input.Controls.KB_W, Input.ControlType.Keyboard);
            response.Add(Input.Controls.KB_A, Input.ControlType.Keyboard);
            response.Add(Input.Controls.KB_S, Input.ControlType.Keyboard);
            response.Add(Input.Controls.KB_D, Input.ControlType.Keyboard);

            response.Add(Input.Controls.KB_ESCAPE, Input.ControlType.Keyboard);
            response.Add(Input.Controls.KB_PAUSE, Input.ControlType.Keyboard);
            response.Add(Input.Controls.KB_SPACE, Input.ControlType.Keyboard);
            response.Add(Input.Controls.KB_BACKSPACE, Input.ControlType.Keyboard);
            response.Add(Input.Controls.KB_LSHIFT, Input.ControlType.Keyboard);

            #endregion

            #region Mouse Buttons

            response.Add(Input.Controls.LMB, Input.ControlType.MouseButton);
            response.Add(Input.Controls.RMB, Input.ControlType.MouseButton);

            #endregion

            #region Mouse Axes

            response.Add(Input.Controls.MX, Input.ControlType.MouseAxis);
            response.Add(Input.Controls.MY, Input.ControlType.MouseAxis);

            #endregion

            #region Joystick Buttons

            response.Add(Input.Controls.A, Input.ControlType.JoystickButton);
            response.Add(Input.Controls.X, Input.ControlType.JoystickButton);
            response.Add(Input.Controls.B, Input.ControlType.JoystickButton);
            response.Add(Input.Controls.Y, Input.ControlType.JoystickButton);

            response.Add(Input.Controls.LB, Input.ControlType.JoystickButton);
            response.Add(Input.Controls.RB, Input.ControlType.JoystickButton);
            response.Add(Input.Controls.LS, Input.ControlType.JoystickButton);
            response.Add(Input.Controls.RS, Input.ControlType.JoystickButton);

            response.Add(Input.Controls.BACK, Input.ControlType.JoystickButton);
            response.Add(Input.Controls.SELECT, Input.ControlType.JoystickButton);
            response.Add(Input.Controls.OPTIONS, Input.ControlType.JoystickButton);
            response.Add(Input.Controls.START, Input.ControlType.JoystickButton);

            #endregion

            #region Joystick Axes

            response.Add(Input.Controls.LS_X, Input.ControlType.JoystickAxis);
            response.Add(Input.Controls.LS_Y, Input.ControlType.JoystickAxis);
            response.Add(Input.Controls.RS_X, Input.ControlType.JoystickAxis);
            response.Add(Input.Controls.RS_Y, Input.ControlType.JoystickAxis);
            response.Add(Input.Controls.DPAD_X, Input.ControlType.JoystickAxis);
            response.Add(Input.Controls.DPAD_Y, Input.ControlType.JoystickAxis);

            response.Add(Input.Controls.LTRIG, Input.ControlType.JoystickAxis);
            response.Add(Input.Controls.RTRIG, Input.ControlType.JoystickAxis);
            response.Add(Input.Controls.TRIGGERS, Input.ControlType.JoystickAxis);

            #endregion

            return response;
        }

        /// <summary>
        /// Initialize a collection of control keycodes.
        /// </summary>
        /// <returns>Returns collection of keycodes.</returns>
        private Dictionary<Controls, KeyCode> InitializeKeyCodes()
        {
            Dictionary<Controls, KeyCode> response = new Dictionary<Input.Controls, KeyCode>();

            #region Keyboard Keys

            response.Add(Input.Controls.LEFT, UnityEngine.KeyCode.LeftArrow);
            response.Add(Input.Controls.RIGHT, UnityEngine.KeyCode.RightArrow);
            response.Add(Input.Controls.UP, UnityEngine.KeyCode.UpArrow);
            response.Add(Input.Controls.DOWN, UnityEngine.KeyCode.DownArrow);

            response.Add(Input.Controls.KB_W, UnityEngine.KeyCode.W);
            response.Add(Input.Controls.KB_A, UnityEngine.KeyCode.A);
            response.Add(Input.Controls.KB_S, UnityEngine.KeyCode.S);
            response.Add(Input.Controls.KB_D, UnityEngine.KeyCode.D);

            response.Add(Input.Controls.KB_ESCAPE, UnityEngine.KeyCode.Escape);
            response.Add(Input.Controls.KB_PAUSE, UnityEngine.KeyCode.Pause);
            response.Add(Input.Controls.KB_SPACE, UnityEngine.KeyCode.Space);
            response.Add(Input.Controls.KB_BACKSPACE, UnityEngine.KeyCode.Backspace);
            response.Add(Input.Controls.KB_LSHIFT, UnityEngine.KeyCode.LeftShift);

            #endregion

            #region Mouse Buttons

            response.Add(Input.Controls.LMB, UnityEngine.KeyCode.Mouse0);
            response.Add(Input.Controls.RMB, UnityEngine.KeyCode.Mouse1);

            #endregion

            return response;
        }

        /// <summary>
        /// Initialize a collection of control input commands.
        /// </summary>
        /// <returns>Returns collection of input commands.</returns>
        private Dictionary<Controls, string> InitializeCommands()
        {
            Dictionary<Controls, string> response = new Dictionary<Input.Controls, string>();

            #region Mouse Axis

            response.Add(Input.Controls.MX, "MX");
            response.Add(Input.Controls.MY, "MY");

            #endregion

            #region Joystick Buttons

            response.Add(Input.Controls.A, "A");
            response.Add(Input.Controls.X, "X");
            response.Add(Input.Controls.B, "B");
            response.Add(Input.Controls.Y, "Y");

            response.Add(Input.Controls.BACK, "Back");
            response.Add(Input.Controls.SELECT, "Select");
            response.Add(Input.Controls.OPTIONS, "Options");
            response.Add(Input.Controls.START, "Start");

            response.Add(Input.Controls.LB, "LB");
            response.Add(Input.Controls.RB, "RB");
            response.Add(Input.Controls.LS, "LS");
            response.Add(Input.Controls.RS, "RS");

            #endregion

            #region Joystick Axis

            response.Add(Input.Controls.LTRIG, "LTRIG");
            response.Add(Input.Controls.RTRIG, "RTRIG");
            response.Add(Input.Controls.TRIGGERS, "TRIGGERS");

            response.Add(Input.Controls.LS_X, "LS_X");
            response.Add(Input.Controls.LS_Y, "LS_Y");
            response.Add(Input.Controls.RS_X, "RS_X");
            response.Add(Input.Controls.RS_Y, "RS_Y");
            response.Add(Input.Controls.DPAD_X, "DPAD_X");
            response.Add(Input.Controls.DPAD_Y, "DPAD_Y");

            #endregion            
                        
            return response;
        }

        #endregion

    }

    #endregion

    #region Struct: Trigger

    /// <summary>
    /// A trigger has a control and a response mode.
    /// </summary>
    public struct Trigger
    {

        #region Data Members

        #region Properties.

        /// <summary>
        /// Get the manager instance.
        /// </summary>
        public InputManager Manager
        {
            get { return InputManager.GetInstance(); }
        }

        /// <summary>
        /// Reference to this trigger's control.
        /// </summary>
        public Controls Control
        {
            get { return this.m_control; }
        }

        /// <summary>
        /// Reference to this trigger's response mode.
        /// </summary>
        public ControlResponse Mode
        {
            get { return this.m_response; }
        }

        /// <summary>
        /// Reference to the previous state.
        /// </summary>
        public bool LastState
        {
            get { return this.m_previousState; }
            private set { this.m_previousState = value; }
        }

        /// <summary>
        /// Reference to the last value.
        /// </summary>
        public float LastValue
        {
            get { return this.m_previousValue; }
            private set { this.m_previousValue = value; }
        }

        /// <summary>
        /// Reference to the current state.
        /// </summary>
        public bool CurrentState
        {
            get { return this.m_currentState; }
            private set { this.m_currentState = value; }
        }

        /// <summary>
        /// Reference to the current value.
        /// </summary>
        public float CurrentValue
        {
            get { return this.m_currentValue; }
            private set { this.m_currentValue = value; }
        }

        /// <summary>
        /// Reference to the final state for this frame.
        /// </summary>
        private bool State
        {
            get { return this.m_state; }
            set { this.m_state = value; }
        }

        /// <summary>
        /// Reference to the final value for this frame.
        /// </summary>
        private float Value
        {
            get { return this.m_value; }
            set { this.m_value = value; }
        }

        /// <summary>
        /// Reference to flag on whether trigger should track previous values or not.
        /// </summary>
        public bool Track
        {
            get { return this.m_track; }
            private set { this.m_track = value; }
        }

        #endregion

        #region Fields.

        /// <summary>
        /// The control this trigger is associated with.
        /// </summary>
        private Controls m_control;

        /// <summary>
        /// The response type associated with this trigger.
        /// </summary>
        private ControlResponse m_response;

        /// <summary>
        /// Response state on the last call.
        /// </summary>
        private bool m_previousState;

        /// <summary>
        /// Response value of the last call.
        /// </summary>
        private float m_previousValue;

        /// <summary>
        /// Response state of the current call.
        /// </summary>
        private bool m_currentState;

        /// <summary>
        /// Response value of the current call.
        /// </summary>
        private float m_currentValue;

        /// <summary>
        /// Storage for the trigger state.
        /// </summary>
        private bool m_state;

        /// <summary>
        /// Storage for the trigger value.
        /// </summary>
        private float m_value;

        /// <summary>
        /// Set trigger to track change between states.
        /// </summary>
        private bool m_track;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Control-only constructor.
        /// </summary>
        /// <param name="_control">Control to assign.</param>
        public Trigger(Controls _control)
        {
            this.m_control = _control;
            this.m_response = ControlResponse.NONE; // Default.
            this.m_track = false;
            this.m_previousState = false;
            this.m_previousValue = 0.0f;
            this.m_currentState = false;
            this.m_currentValue = 0.0f;
            this.m_state = false;
            this.m_value = 0.0f;

            if (InputManager.GetInstance().IsAKeyOrButton(_control))
            {
                this.m_response = ControlResponse.HELD;
            }
            else if (InputManager.GetInstance().IsAxis(_control))
            {
                this.m_response = ControlResponse.AXIS;
            }
        }

        /// <summary>
        /// Explicit constructor.
        /// </summary>
        /// <param name="_control">Control to assign.</param>
        /// <param name="_response">Mode to set.</param>
        public Trigger(Controls _control, ControlResponse _response)
        {
            this.m_control = _control;
            this.m_response = _response;
            this.m_track = false;
            this.m_previousState = false;
            this.m_previousValue = 0.0f;
            this.m_currentState = false;
            this.m_currentValue = 0.0f;
            this.m_state = false;
            this.m_value = 0.0f;

            if (InputManager.GetInstance().IsAKeyOrButton(_control))
            {
                if (_response != ControlResponse.HELD
                    && _response != ControlResponse.PRESSED
                    && _response != ControlResponse.RELEASED
                    && _response != ControlResponse.UP)
                {
                    this.m_response = ControlResponse.NONE;
                }
            }
            else if (InputManager.GetInstance().IsAxis(_control))
            {
                if (_response != ControlResponse.AXIS
                    && _response != ControlResponse.AXIS_RAW
                    && _response != ControlResponse.DELTA
                    && _response != ControlResponse.ONCHANGE)
                {
                    this.m_response = ControlResponse.NONE;
                }
            }

            if (_response == ControlResponse.DELTA
                || _response == ControlResponse.ONCHANGE)
            {
                // Track for purposes of axis value calculations.
                this.m_track = true;
            }
        }

        #endregion

        #region Methods.

        /// <summary>
        /// When the update method is called, update last and current state/value members.
        /// </summary>
        public void Update()
        {
            // Track will only be true, if this is an axis with OnChange or Delta.
            if (this.Track)
            {
                // Store the previous members.
                this.LastState = this.CurrentState;
                this.LastValue = this.CurrentValue;

                // Calculate new current members.
                this.CurrentState = GetCurrentState();
                this.CurrentValue = GetCurrentAxis();
            }
        }

        /// <summary>
        /// Returns the trigger state.
        /// </summary>
        /// <returns>Returns true if trigger is triggered.</returns>
        public bool GetState()
        {
            if (this.Mode == ControlResponse.NONE)
            {
                this.State = false;
            }
            else if (this.Mode == ControlResponse.ONCHANGE)
            {
                this.State = (this.CurrentValue != this.LastValue);
            }
            else if (this.Mode == ControlResponse.DELTA)
            {
                this.State = (GetDelta() > 0.0f);
            }
            else
            {
                this.State = this.Manager.IsTriggered(this.Control, this.Mode);
            }

            return this.State;
        }

        /// <summary>
        /// Returns the trigger value as a float.
        /// </summary>
        /// <param name="weight">Value to multiply the output axis value by. Value is not scaled by default.</param>
        /// <returns>Returns a float (that can be multiplied by a supplied weight).</returns>
        public float GetAxis(float weight = 1.0f)
        {
            if (this.Mode == ControlResponse.NONE)
            {
                this.Value = 0.0f;
            }
            else if (this.Mode == ControlResponse.DELTA)
            {
                this.Value = GetDelta();
            }
            else
            {
                float value = 0.0f;
                this.Manager.IsTriggeredValue(this.Control, this.Mode, out value);
                this.Value = value;
            }

            return this.Value;
        }


        /// <summary>
        /// Get the change in value between the current and last frames.
        /// </summary>
        /// <returns></returns>
        private float GetDelta()
        {
            if (this.Mode == ControlResponse.DELTA)
            {
                return this.CurrentValue - this.LastValue;
            }

            // If not delta, return a zero change.
            return 0.0f;
        }

        /// <summary>
        /// Return the trigger's axis value.
        /// </summary>
        /// <returns>Return the axis value.</returns>
        private float GetCurrentAxis()
        {
            ControlResponse axisMode = this.Mode;

            if (this.Mode == ControlResponse.DELTA)
            {
                axisMode = ControlResponse.AXIS;
            }

            if (this.Mode == ControlResponse.ONCHANGE)
            {
                axisMode = ControlResponse.AXIS_RAW;
            }

            float value = 0.0f;
            if (Manager.IsTriggeredValue(this.Control, axisMode, out value))
            {
                return value;
            }
            return 0.0f;
        }

        /// <summary>
        /// Return the current state.
        /// </summary>
        /// <returns></returns>
        private bool GetCurrentState()
        {
            return Manager.IsTriggered(this.Control, this.Mode);
        }

        #endregion

    }

    #endregion

    #region Enum: Controls

    /// <summary>
    /// A list of common controls that may be requested.
    /// </summary>
    public enum Controls
    {

        #region Keyboard Keys

        #region Arrow Keys

        /// <summary>
        /// The left arrow key.
        /// </summary>
        LEFT,

        /// <summary>
        /// The right arrow key.
        /// </summary>
        RIGHT,

        /// <summary>
        /// The up arrow key.
        /// </summary>
        UP,

        /// <summary>
        /// The down arrow key.
        /// </summary>
        DOWN,

        #endregion

        #region WASD Keys

        /// <summary>
        /// The 'W' key.
        /// </summary>
        KB_W,

        /// <summary>
        /// The 'A' key.
        /// </summary>
        KB_A,

        /// <summary>
        /// The 'S' key.
        /// </summary>
        KB_S,

        /// <summary>
        /// The 'D' key.
        /// </summary>
        KB_D,

        #endregion

        #region Misc. Keys.

        /// <summary>
        /// The 'ESC' key.
        /// </summary>
        KB_ESCAPE,

        /// <summary>
        /// The 'PAUSE' key.
        /// </summary>
        KB_PAUSE,

        /// <summary>
        /// The 'SPACE' key.
        /// </summary>
        KB_SPACE,

        /// <summary>
        /// The 'BACKSPACE' key.
        /// </summary>
        KB_BACKSPACE,

        /// <summary>
        /// The left shift key.
        /// </summary>
        KB_LSHIFT,

        #endregion

        #endregion

        #region Mouse Buttons / Axes

        /// <summary>
        /// The left mouse button.
        /// </summary>
        LMB,

        /// <summary>
        /// The right mouse button.
        /// </summary>
        RMB,

        /// <summary>
        /// The mouse x-axis.
        /// </summary>
        MX,

        /// <summary>
        /// The mouse y-axis.
        /// </summary>
        MY,

        #endregion

        #region XInput Buttons / Axes

        /// <summary>
        /// The A button.
        /// </summary>
        A,

        /// <summary>
        /// The X button.
        /// </summary>
        X,

        /// <summary>
        /// The B button.
        /// </summary>
        B,

        /// <summary>
        /// The Y button.
        /// </summary>
        Y,


        /// <summary>
        /// The back/select button.
        /// </summary>
        BACK,

        /// <summary>
        /// The back/select button.
        /// </summary>
        SELECT,

        /// <summary>
        /// The options/start button.
        /// </summary>
        OPTIONS,

        /// <summary>
        /// The options/start button.
        /// </summary>
        START,

        /// <summary>
        /// The left bumper.
        /// </summary>
        LB,

        /// <summary>
        /// The right bumper.
        /// </summary>
        RB,

        /// <summary>
        /// The left trigger axis.
        /// </summary>
        LTRIG,

        /// <summary>
        /// The right trigger axis.
        /// </summary>
        RTRIG,

        /// <summary>
        /// The shared triggers axis.
        /// </summary>
        TRIGGERS,


        /// <summary>
        /// The left stick button.
        /// </summary>
        LS,

        /// <summary>
        /// The right stick button.
        /// </summary>
        RS,


        /// <summary>
        /// The x-axis (DPAD).
        /// </summary>
        DPAD_X,

        /// <summary>
        /// The y-axis (DPAD).
        /// </summary>
        DPAD_Y,

        /// <summary>
        /// The x-axis (Left Stick).
        /// </summary>
        LS_X,

        /// <summary>
        /// The y-axis (Left Stick).
        /// </summary>
        LS_Y,

        /// <summary>
        /// The x-axis (Right Stick).
        /// </summary>
        RS_X,

        /// <summary>
        /// The y-axis (Right Stick).
        /// </summary>
        RS_Y,

        #endregion
        
    }

    #endregion

    #region Enum: ControlResponse

    /// <summary>
    /// A list of common control response types.
    /// </summary>
    public enum ControlResponse
    {

        /// <summary>
        /// None will never generate a response. (Always returns false).
        /// </summary>
        NONE,

        #region Button/Key Responses

        /// <summary>
        /// Return true whenever the key isn't being pressed.
        /// </summary>
        UP,

        /// <summary>
        /// Return true whenever the key is being pressed down.
        /// </summary>
        HELD,

        /// <summary>
        /// Return true when the key was released this frame.
        /// </summary>
        RELEASED,

        /// <summary>
        /// Return true when the key was pressed this frame.
        /// </summary>
        PRESSED,

        #endregion

        #region Axis Response

        /// <summary>
        /// Return a float value for the axis.
        /// </summary>
        AXIS,

        /// <summary>
        /// Return a raw float value for the axis.
        /// </summary>
        AXIS_RAW,

        /// <summary>
        /// Return the change in x since last frame.
        /// </summary>
        DELTA,

        /// <summary>
        /// Return a true/false value when axis is changed.
        /// </summary>
        ONCHANGE

        #endregion

    }

    #endregion

    #region Enum: ControlType

    /// <summary>
    /// Type of control a control will be associated with.
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// Null control type.
        /// </summary>
        NULL,

        /// <summary>
        /// Keyboard buttons have the Keyboard type.
        /// </summary>
        Keyboard,

        /// <summary>
        /// Mouse buttons have the MouseButton type.
        /// </summary>
        MouseButton,

        /// <summary>
        /// Mouse axes have the MouseAxis type.
        /// </summary>
        MouseAxis,

        /// <summary>
        /// Joystick buttons have the JoystickButton type.
        /// </summary>
        JoystickButton,

        /// <summary>
        /// Joystick axes have the JoystickAxis type.
        /// </summary>
        JoystickAxis
    }

    #endregion

    }