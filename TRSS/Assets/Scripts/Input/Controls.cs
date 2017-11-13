/*************************************************
 * Control.cs
 * 
 * This file contains:
 * - The Control class.
 * - The ControlTrigger class.
 * - The ControlResponse class.
 * ***********************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TRSS.Input
{
    
    #region Enum: ResponseMode

    /// <summary>
    /// Response mode, indicating how a trigger will be activated.
    /// </summary>
    public enum ResponseMode
    {
        /// <summary>
        /// No response.
        /// </summary>
        None,

        /// <summary>
        /// Trigger activated when control is held.
        /// </summary>
        Held,

        /// <summary>
        /// Trigger activated when control is released.
        /// </summary>
        Released,

        /// <summary>
        /// Trigger activated when control is pressed.
        /// </summary>
        Pressed,

        /// <summary>
        /// Trigger activated when there is no button input.
        /// </summary>
        Up,
        
        /// <summary>
        /// Trigger activation will return a float axis value.
        /// </summary>
        Axis,

        /// <summary>
        /// Trigger activation will return a raw float axis value.
        /// </summary>
        AxisRaw
    }

    #endregion

    #region Enum: ControlType

    /// <summary>
    /// Enum regarding input mapping methods.
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// A Key, Joystick, or Mouse button.
        /// </summary>
        Key,

        /// <summary>
        /// A button accessed through the GetButton method.
        /// </summary>
        Button,

        /// <summary>
        /// A Mouse or Joystick axis.
        /// </summary>
        Axis,

        /// <summary>
        /// Refers to the left or right mouse buttons.
        /// </summary>
        MouseButton
    }

    #endregion

    #region Enum: MouseButton

    /// <summary>
    /// Enum representing the mouse button to request.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// Left mouse button.
        /// </summary>
        LMB = 0,

        /// <summary>
        /// Right mouse button.
        /// </summary>
        RMB = 1,

        /// <summary>
        /// Middle mouse button.
        /// </summary>
        MMB = 2,

        /// <summary>
        /// Null mouse button option.
        /// </summary>
        None = -1
    }

    #endregion
    
    #region Struct: Trigger struct.

    /// <summary>
    /// Maps controls to input response types.
    /// </summary>
    public struct Trigger
    {

        #region Data Members.

        #region Fields.

        /// <summary>
        /// The control associated with this trigger.
        /// </summary>
        private Control m_control;

        /// <summary>
        /// The trigger response mode.
        /// </summary>
        private ResponseMode m_triggerMode;

        #endregion

        #region Properties.

        /// <summary>
        /// Reference to the control.
        /// </summary>
        public Control Control
        {
            get { return this.m_control; }
        }

        /// <summary>
        /// Reference to response mode.
        /// </summary>
        public ResponseMode Mode
        {
            get { return this.m_triggerMode; }
        }

        /// <summary>
        /// Returns axis value depending on trigger mode, as well as 1.0f if button mode is true.
        /// </summary>
        /// <returns>Returns a value between -1.0f and 1.0f</returns>
        public float Value
        {
            get
            {
                // Can't be triggered if response mode is set to none.
                if (this.Mode == ResponseMode.None) { return 0.0f; }

                // If an axis.
                if (IsAxisTrigger() && IsAxisMode())
                {
                    switch (this.Mode)
                    {
                        case ResponseMode.Axis:
                            return this.Control.GetAxis();
                        case ResponseMode.AxisRaw:
                            return this.Control.GetAxisRaw();
                    }
                }

                // If a button.
                if (IsActivated()) { return 1.0f; }

                // Return 0.0f if nothing is valid.
                return 0.0f;
            }
        }

        #endregion

        #endregion

        #region Constructors.

        /// <summary>
        /// Create a trigger, setting the response mode based on the input control's ControlType.
        /// </summary>
        /// <param name="_control">Control to assign trigger to.</param>
        public Trigger(Control _control)
        {
            this.m_control = _control;
            this.m_triggerMode = ResponseMode.None;
            SetControl(_control);
        }

        /// <summary>
        /// Create a control with an explicitly specified response mode.
        /// </summary>
        /// <param name="_control">Control to assign trigger to.</param>
        /// <param name="_mode">Trigger mode.</param>
        public Trigger(Control _control, ResponseMode _mode)
        {
            this.m_control = _control;
            this.m_triggerMode = _mode;
            SetControl(_control);
            SetResponseMode(_mode);
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set the control and parse out the correct default response mode.
        /// </summary>
        /// <param name="_control">Control to assign trigger to.</param>
        public void SetControl(Control _control)
        {
            this.m_control = _control;

            if (_control.IsAxis())
            {
                // If the control is an axis, set the response mode to axis, by default.
                SetResponseMode(ResponseMode.Axis);
            }

            if (_control.IsButton())
            {
                // If the control is a button, set the response mode to held, by default.
                SetResponseMode(ResponseMode.Held);
            }
        }

        /// <summary>
        /// Set the control with an explicitly specified response mode.
        /// </summary>
        /// <param name="_control">Control to assign trigger to.</param>
        /// <param name="_mode">Mode to set trigger to.</param>
        public void SetControl(Control _control, ResponseMode _mode)
        {
            SetControl(_control);
            SetResponseMode(_mode);
        }

        /// <summary>
        /// Set the response mode.
        /// </summary>
        /// <param name="_mode">Mode to set trigger to.</param>
        public void SetResponseMode(ResponseMode _mode)
        {
            if (IsButtonTrigger())
            {
                if (IsButtonMode(_mode))
                {
                    this.m_triggerMode = _mode;
                    return;
                }
            }

            if (IsAxisTrigger())
            {
                if (IsAxisMode(_mode))
                {
                    this.m_triggerMode = _mode;
                    return;
                }
            }

          }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// If this is a trigger for a button control, return true.
        /// </summary>
        /// <returns>Returns true if control is a button.</returns>
        public bool IsButtonTrigger()
        {
            return (this.m_control.IsButton());
        }

        /// <summary>
        /// If this is a trigger for an axis control, return true.
        /// </summary>
        /// <returns>Returns true if control is an axis.</returns>
        public bool IsAxisTrigger()
        {
            return (this.m_control.IsAxis());
        }

        /// <summary>
        /// Returns true if the current response mode is for a button.
        /// </summary>
        /// <returns>Returns true based on response mode.</returns>
        public bool IsButtonMode()
        {
            return IsButtonMode(this.m_triggerMode);
        }

        /// <summary>
        /// Returns true if the input response mode is for a button.
        /// </summary>
        /// <returns>Returns true based on input response mode.</returns>
        public bool IsButtonMode(ResponseMode _mode)
        {
            return (_mode == ResponseMode.Held || _mode == ResponseMode.Pressed || _mode == ResponseMode.Released || _mode == ResponseMode.Up || _mode == ResponseMode.None);
        }

        /// <summary>
        /// Returns true if the current response mode is for an axis.
        /// </summary>
        /// <returns>Returns true based on response mode.</returns>
        public bool IsAxisMode()
        {
            return IsAxisMode(this.m_triggerMode);
        }

        /// <summary>
        /// Returns true if the input response mode is for an axis.
        /// </summary>
        /// <returns>Returns true based on input response mode.</returns>
        public bool IsAxisMode(ResponseMode _mode)
        {
            return (_mode == ResponseMode.Axis || _mode == ResponseMode.AxisRaw || _mode == ResponseMode.None);
        }

        /// <summary>
        /// Determines if the trigger has been activated.
        /// </summary>
        /// <returns>Returns true if activated.</returns>
        public bool IsActivated()
        {
            // Can't be triggered if response mode is set to none.
            if (this.Mode == ResponseMode.None) { return false; }

            // If a button.
            if (IsButtonTrigger() && IsButtonMode())
            {
                switch (this.Mode)
                {
                    case ResponseMode.Held:
                        return this.Control.IsHeld();
                    case ResponseMode.Pressed:
                        return this.Control.IsPressed();
                    case ResponseMode.Released:
                        return this.Control.IsReleased();
                    case ResponseMode.Up:
                        return !(this.Control.IsHeld() || this.Control.IsPressed());
                }
            }

            // If an axis.
            if (IsAxisTrigger() && IsAxisMode())
            {
                switch (this.Mode)
                {
                    case ResponseMode.Axis:
                        return (this.Control.GetAxis() != 0.0f);
                    case ResponseMode.AxisRaw:
                        return (this.Control.GetAxisRaw() != 0.0f);
                }
            }

            // Return false if nothing is valid.
            return false;
        }

        #endregion

    }

    #endregion
    
    #region Struct: Control struct.

    /// <summary>
    /// A control directly references a button, axis, or other type of input.
    /// It doesn't care about who calls it.
    /// </summary>
    public struct Control
    {

        #region Static Members.

        /// <summary>
        /// Create a key control.
        /// </summary>
        /// <param name="_key">Key name associated with control.</param>
        /// <returns>Returns Control object.</returns>
        public static Control CreateKey(KeyCode _key)
        {
            return new Control(_key);
        }

        /// <summary>
        /// Create a button control.
        /// </summary>
        /// <param name="_button">Button name associated with control.</param>
        /// <returns>Returns Control object.</returns>
        public static Control CreateButton(string _button)
        {
            return new Control(_button, ControlType.Button);
        }

        /// <summary>
        /// Create a mouse button control.
        /// </summary>
        /// <param name="_button">Mouse button to be associated with control.</param>
        /// <returns>Returns Control object.</returns>
        public static Control CreateMouseButton(MouseButton _button)
        {
            return new Control(_button);
        }

        /// <summary>
        /// Create an axis control.
        /// </summary>
        /// <param name="_button">Axis name associated with control.</param>
        /// <returns>Returns Control object.</returns>
        public static Control CreateAxis(string _axis)
        {
            return new Control(_axis, ControlType.Axis);
        }

        #endregion

        #region Common Controls

        /// <summary>
        /// Returns a control for the horizontal mouse axis movement.
        /// </summary>
        /// <returns>Returns a control object.</returns>
        public static Control MouseX()
        {
            return CreateAxis("Mouse X");
        }

        /// <summary>
        /// Returns a control for the vertical mouse axis movement.
        /// </summary>
        /// <returns>Returns a control object.</returns>
        public static Control MouseY()
        {
            return CreateAxis("Mouse Y");
        }

        /// <summary>
        /// Returns a control for the mouse scrollwheel movement.
        /// </summary>
        /// <returns>Returns a control object.</returns>
        public static Control ScrollWheel()
        {
            return CreateAxis("Mouse ScrollWheel");
        }

        /// <summary>
        /// Returns the A button status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control AButton()
        {
            return CreateKey(KeyCode.JoystickButton0);
        }

        /// <summary>
        /// Returns the B button status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control BButton()
        {
            return CreateKey(KeyCode.JoystickButton1);
        }

        /// <summary>
        /// Returns the X button status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control XButton()
        {
            return CreateKey(KeyCode.JoystickButton2);
        }

        /// <summary>
        /// Returns the Y button status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control YButton()
        {
            return CreateKey(KeyCode.JoystickButton3);
        }

        /// <summary>
        /// Returns the left bumper status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control LeftBumper()
        {
            return CreateKey(KeyCode.JoystickButton4);
        }

        /// <summary>
        /// Returns the right bumper status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control RightBumper()
        {
            return CreateKey(KeyCode.JoystickButton5);
            
        }

        /// <summary>
        /// Returns the back button status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control BackButton()
        {
            return CreateKey(KeyCode.JoystickButton6);        
        }

        /// <summary>
        /// Returns the start button status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control StartButton()
        {           
            return CreateKey(KeyCode.JoystickButton7);           
        }

        /// <summary>
        /// Returns the left stick button status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control LeftStickButton()
        {
            return CreateKey(KeyCode.JoystickButton8);
        }

        /// <summary>
        /// Returns the right stick button status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control RightStickButton()
        {
            return CreateKey(KeyCode.JoystickButton9);         
        }

        /// <summary>
        /// Returns the left stick x-axis status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control LeftStickHorizontal()
        {
            return CreateAxis("LSX");
        }

        /// <summary>
        /// Returns the left stick y-axis status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control LeftStickVertical()
        {
            return CreateAxis("LSY");
        }

        /// <summary>
        /// Returns the right stick x-axis status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control RightStickHorizontal()
        {
            return CreateAxis("RSX");
        }

        /// <summary>
        /// Returns the right stick y-axis status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control RightStickVertical()
        {
            return CreateAxis("RSY");
        }

        /// <summary>
        /// Returns the digital pad x-axis status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control DPadHorizontal()
        {
            return CreateAxis("DPADX");
        }

        /// <summary>
        /// Returns the digital pad y-axis status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control DPadVertical()
        {
             return CreateAxis("DPADY");
            
        }

        /// <summary>
        /// Returns the triggers axis status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control BothTriggers()
        {
             return CreateAxis("TRIGGERS");
        }

        /// <summary>
        /// Returns the left trigger axis status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control LeftTrigger()
        {
            return CreateAxis("LTRIG");
        }

        /// <summary>
        /// Returns the right trigger axis status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control RightTrigger()
        {
            return CreateAxis("RTRIG");            
        }

        /// <summary>
        /// Returns the logo button status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control LogoButton()
        {
            return CreateKey(KeyCode.JoystickButton12);            
        }

        /// <summary>
        /// Returns the touchpad button status.
        /// </summary>
        /// <returns>Control object.</returns>
        public static Control TouchpadButton()
        {
            return CreateKey(KeyCode.JoystickButton13);
        }

        #endregion
        
        #region Data Members

        #region Fields.

        /// <summary>
        /// Type of input to be associated with the control name.
        /// </summary>
        private ControlType m_controlType;

        /// <summary>
        /// KeyCode associated with the button or keyboard key to request.
        /// </summary>
        private KeyCode m_key;

        /// <summary>
        /// The axis or button-axis name to request from Unity. 
        /// </summary>
        private string m_axis;

        /// <summary>
        /// Name of the button to use.
        /// </summary>
        private string m_button;

        /// <summary>
        /// Button to use for mouse input.
        /// </summary>
        private MouseButton m_mouse;

        #endregion

        #region Properties.

        /// <summary>
        /// Return the type of control input.
        /// </summary>
        public ControlType ControlType
        {
            get { return this.m_controlType; }
        }

        /// <summary>
        /// Returns the button or key code used for the input.
        /// </summary>
        public KeyCode Key
        {
            get { return this.m_key; }
        }

        /// <summary>
        /// Returns the axis name used for the input.
        /// </summary>
        public string Axis
        {
            get { return this.m_axis; }
        }

        /// <summary>
        /// Returns the name of the button used for the input.
        /// </summary>
        public string Button
        {
            get { return this.m_button; }
        }

        /// <summary>
        /// Returns the enum value for the mouse button being pressed.
        /// </summary>
        public MouseButton MouseButton
        {
            get { return this.m_mouse; }
        }

        #endregion

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a control struct with given inputs. By defualt, creates an axis.
        /// </summary>
        /// <param name="_input">Name of the input to request values from.</param>
        public Control(string _input, ControlType _control = ControlType.Axis)
        {
            // Validate values.
            string input = _input.Trim();

            // Set defaults.
            this.m_controlType = _control;
            this.m_key = KeyCode.None;
            this.m_button = "";
            this.m_axis = "";
            this.m_mouse = MouseButton.None;

            switch (this.ControlType)
            {
                case ControlType.Button:
                    this.m_button = input;
                    break;
                case ControlType.Axis:
                    this.m_axis = input;
                    break;
            }
        }

        /// <summary>
        /// Creates a control struct associated with a specific keycode.
        /// </summary>
        /// <param name="_code">KeyCode to associate control with.</param>
        public Control(KeyCode _code)
        {
            this.m_controlType = ControlType.Key;
            this.m_key = _code;
            this.m_button = "";
            this.m_axis = "";
            this.m_mouse = MouseButton.None;
        }

        /// <summary>
        /// Creates a control struct associated with a specific mouse button.
        /// </summary>
        /// <param name="_mouseButton">MouseButton to associate control with.</param>
        public Control(MouseButton _mouseButton)
        {
            this.m_controlType = ControlType.MouseButton;
            this.m_key = KeyCode.None;
            this.m_button = "";
            this.m_axis = "";
            this.m_mouse = _mouseButton;

            switch (this.MouseButton)
            {
                case MouseButton.LMB:
                    this.m_key = KeyCode.Mouse0;
                    break;
                case MouseButton.RMB:
                    this.m_key = KeyCode.Mouse1;
                    break;
                case MouseButton.MMB:
                    this.m_key = KeyCode.Mouse2;
                    break;
            }
        }

        #endregion

        #region Accessor Methods

        /// <summary>
        /// Check if this is a button, keycode, or mouse button.
        /// </summary>
        /// <returns>Returns true if it has appropriate ControlType.</returns>
        public bool IsButton()
        {
            return this.ControlType == ControlType.Button || this.ControlType == ControlType.MouseButton || this.ControlType == ControlType.Key;
        }

        /// <summary>
        /// Check if this is an axis.
        /// </summary>
        /// <returns>Returns true if it has appropriate ControlType.</returns>
        public bool IsAxis()
        {
            return this.ControlType == ControlType.Axis;
        }

        /// <summary>
        /// Check if it has a KeyCode.
        /// </summary>
        /// <returns>Returns true if it has a KeyCode.</returns>
        public bool HasKeyCode()
        {
            return IsButton() && this.Key != KeyCode.None;
        }

        /// <summary>
        /// Check if it has a Button.
        /// </summary>
        /// <returns>Returns true if it has a button name.</returns>
        public bool HasButton()
        {
            return IsButton() && this.Button.Length > 0;
        }

        /// <summary>
        /// Check if it has a MouseButton
        /// </summary>
        /// <returns>Returns true if it has a mouse button.</returns>
        public bool HasMouseButton()
        {
            return IsButton() && this.MouseButton == MouseButton.None;
        }

        /// <summary>
        /// Check if it has an axis name.
        /// </summary>
        /// <returns>Returns true if it has an Axis name.</returns>
        public bool HasAxis()
        {
            return IsAxis() && this.Axis.Length > 0;
        }

        /// <summary>
        /// Returns a KeyCode.
        /// </summary>
        /// <returns>Returns KeyCode.None if not a button.</returns>
        public KeyCode GetKeyCode()
        {
            if (HasKeyCode()) { return this.Key; }
            return KeyCode.None;
        }

        /// <summary>
        /// Returns the name of the button.
        /// </summary>
        /// <returns>Returns empty string if no button name exists.</returns>
        public string GetButtonName()
        {
            if (HasButton()) { return this.Button; }
            return "";
        }

        /// <summary>
        /// Returns the mouse button as Unity recognized values.
        /// </summary>
        /// <returns>Returns MouseButton.None if no mouse button is there.</returns>
        public int GetMouseButton()
        {
            if (HasMouseButton()) { return (int)this.MouseButton; }
            return (int)MouseButton.None;
        }

        /// <summary>
        /// Returns the mouse button as its keycode version, if applicable.
        /// </summary>
        /// <returns>Returns a KeyCode corresponding to the button inputs.</returns>
        public KeyCode GetMouseCode()
        {
            if (HasMouseButton()) { return this.Key; }
            return KeyCode.None;
        }

        /// <summary>
        /// Return the axis name.
        /// </summary>
        /// <returns>Returns an empty string if there is no axis name.</returns>
        public string GetAxisName()
        {
            if (HasAxis()) { return this.Axis; }
            return "";
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Returns axis value, with smoothing.
        /// </summary>
        /// <returns>Value of input axis.</returns>
        public float GetAxis()
        {
            if (IsAxis())
            {
                return UnityEngine.Input.GetAxis(this.Axis);
            }

            return 0.0f;
        }

        /// <summary>
        /// Returns raw axis value.
        /// </summary>
        /// <returns>Value of input axis.</returns>
        public float GetAxisRaw()
        {
            if (IsAxis())
            {
                return UnityEngine.Input.GetAxisRaw(this.Axis);
            }

            return 0.0f;
        }

        /// <summary>
        /// Returns true if control is being held.
        /// </summary>
        /// <returns>Returns true based on input.</returns>
        public bool IsHeld()
        {
            if (IsButton())
            {
                if (HasKeyCode())
                {
                    return this.IsKeyHeld();
                }

                if (HasButton())
                {
                    return this.IsButtonHeld();
                }

                if (HasMouseButton())
                {
                    return this.IsMouseButtonHeld();
                }
            }

            // Return false if it is an axis.
            return false;
        }

        /// <summary>
        /// Check if key is being held.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsKeyHeld()
        {
            if (HasKeyCode())
            {
                return UnityEngine.Input.GetKey(this.Key);
            }

            return false;
        }

        /// <summary>
        /// Check if button is being held.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsButtonHeld()
        {
            if (HasButton())
            {
                return UnityEngine.Input.GetButton(this.Button);
            }

            return false;
        }

        /// <summary>
        /// Check if the mouse button is being held.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsMouseButtonHeld()
        {
            if (HasMouseButton())
            {
                return UnityEngine.Input.GetMouseButton(this.GetMouseButton());
            }

            return false;
        }

        /// <summary>
        /// Returns true if control was just pressed.
        /// </summary>
        /// <returns>Returns true based on input.</returns>
        public bool IsPressed()
        {
            if (IsButton())
            {
                if (HasKeyCode())
                {
                    return this.IsKeyPressed();
                }

                if (HasButton())
                {
                    return this.IsButtonPressed();
                }

                if (HasMouseButton())
                {
                    return this.IsMouseButtonPressed();
                }
            }

            // Return false if it is an axis.
            return false;
        }

        /// <summary>
        /// Check if key was just pressed.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsKeyPressed()
        {
            if (HasKeyCode())
            {
                return UnityEngine.Input.GetKeyDown(this.Key);
            }

            return false;
        }

        /// <summary>
        /// Check if button was just pressed.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsButtonPressed()
        {
            if (HasButton())
            {
                return UnityEngine.Input.GetButtonDown(this.Button);
            }

            return false;
        }

        /// <summary>
        /// Check if the mouse button was just pressed.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsMouseButtonPressed()
        {
            if (HasMouseButton())
            {
                return UnityEngine.Input.GetMouseButtonDown(this.GetMouseButton());
            }

            return false;
        }

        /// <summary>
        /// Returns true if control was just released.
        /// </summary>
        /// <returns>Returns true based on input.</returns>
        public bool IsReleased()
        {
            if (IsButton())
            {
                if (HasKeyCode())
                {
                    return this.IsKeyReleased();
                }

                if (HasButton())
                {
                    return this.IsButtonReleased();
                }

                if (HasMouseButton())
                {
                    return this.IsMouseButtonReleased();
                }
            }

            // Return false if it is an axis.
            return false;
        }

        /// <summary>
        /// Check if key was just released.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsKeyReleased()
        {
            if (HasKeyCode())
            {
                return UnityEngine.Input.GetKeyUp(this.Key);
            }

            return false;
        }

        /// <summary>
        /// Check if button was just released.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsButtonReleased()
        {
            if (HasButton())
            {
                return UnityEngine.Input.GetButtonUp(this.Button);
            }

            return false;
        }

        /// <summary>
        /// Check if the mouse button was just released.
        /// </summary>
        /// <returns>Returns true if applicable.</returns>
        private bool IsMouseButtonReleased()
        {
            if (HasMouseButton())
            {
                return UnityEngine.Input.GetMouseButtonUp(this.GetMouseButton());
            }

            return false;
        }

        /// <summary>
        /// Check if controls are the same.
        /// </summary>
        /// <param name="other">Other Control to compare.</param>
        /// <returns>Returns boolean.</returns>
        public bool Equals(Control other)
        {
            if (other.ControlType == this.ControlType)
            {
                switch (this.ControlType)
                {
                    case ControlType.Axis:
                        return this.Axis == other.Axis;
                    case ControlType.Key:
                        return this.Key == other.Key;
                    case ControlType.Button:
                        return this.Button == other.Button;
                    case ControlType.MouseButton:
                        return this.MouseButton == other.MouseButton;
                }
            }

            // Return false by default.
            return false;
        }

        /// <summary>
        /// Check if object is a control and is the same.
        /// </summary>
        /// <param name="obj">Other object to compare.</param>
        /// <returns>Returns boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Control))
            {
                return Equals((Control)obj);
            }

            return false;
        }

        /// <summary>
        /// Return the hash code.
        /// </summary>
        /// <returns>Returns integer as hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Operator Overload

        /// <summary>
        /// Checks controls for equality.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns true if equal.</returns>
        public static bool operator ==(Control a, Control b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Checks controls for inequality.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns true if unequal.</returns>
        public static bool operator !=(Control a, Control b)
        {
            return !(a.Equals(b));
        }


        #endregion

    }

    #endregion

    /// <summary>
    /// An action is a name, associated with a trigger and a control.
    /// </summary>
    public class Action
    {

        #region Static Members.

        /// <summary>
        /// Collection of unique action IDs.
        /// </summary>
        private List<string> s_ids = null;

        /// <summary>
        /// Reference to collection of unique action IDs.
        /// </summary>
        public List<string> UIDs
        {
            get
            {
                if (s_ids == null)
                {
                    s_ids = new List<string>();
                }
                return s_ids;
            }
        }

        /// <summary>
        /// Makes an ID value out of the input.
        /// </summary>
        /// <param name="_id">ID to convert.</param>
        /// <returns>Returns a trimmed, uppercase string.</returns>
        public string MakeID(string _id)
        {
            return _id.Trim().ToUpper();
        }

        /// <summary>
        /// Checks if the collection already contains the input ID.
        /// </summary>
        /// <param name="_id">ID to check for.</param>
        /// <returns>Returns true if the ID is already found.</returns>
        public bool HasID(string _id)
        {
            string key = MakeID(_id);
            if (UIDs.Count() > 0)
            {
                return UIDs.Contains(key);
            }
            return false;
        }

        /// <summary>
        /// Add input value as a new ID, if possible.
        /// </summary>
        /// <param name="_id">ID to add.</param>
        /// <returns>Returns the key form of the string.</returns>
        public string AddID(string _id)
        {
            string key = MakeID(_id);
            if (!HasID(_id))
            {
                UIDs.Add(key);
            }
            return key;
        }

        #endregion

        #region Data Members

        /// <summary>
        /// The unique name of the action.
        /// </summary>
        private string m_name = "";

        /// <summary>
        /// The control trigger for the action.
        /// </summary>
        private Trigger m_trigger;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an action, with a default "HELD" trigger.
        /// </summary>
        /// <param name="_name">Name of the action.</param>
        /// <param name="_control">Control to associate with the new trigger.</param>
        /// <param name="_mode">Response mode to assign.</param>
        public Action(string _name, Control _control, ResponseMode _mode = ResponseMode.Held)
        {
            m_name = AddID(_name);
            m_trigger = new Trigger(_control, _mode);
        }
        
        /// <summary>
        /// Creates an action, with a default "HELD" trigger.
        /// </summary>
        /// <param name="_name">Name of the action.</param>
        /// <param name="_trigger">Trigger with a control.</param>
        public Action(string _name, Trigger _trigger)
        {
            m_name = AddID(_name);
            m_trigger = _trigger;
        }

        #endregion

    }

    /// <summary>
    /// A control trigger pairs a control object with a response type, making it a trigger.
    /// </summary>
    public class ControlScheme
    {

        #region Data Members

        private List<string> actions;

        #endregion

    }


}
