/*************************************************
 * ControllerInput.cs
 * 
 * This file contains:
 * - The ControllerInput class.
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
    /// <summary>
    /// Uses an input scheme to handle input.
    /// </summary>
    public class ControllerInput : MonoBehaviour
    {

        #region Data Members

        #region Fields

        /// <summary>
        /// The input scheme that will handle the component's control updates.
        /// </summary>
        private InputScheme m_scheme = null;

        /// <summary>
        /// Input display component that will reflect input activity.
        /// </summary>
        private InputDisplay m_display = null;

        /// <summary>
        /// Default active color for input display.
        /// </summary>
        public Color m_inputColor = Color.yellow;

        /// <summary>
        /// The initialization flag.
        /// </summary>
        private bool m_initialized = false;

        /// <summary>
        /// Reference to the debug component.
        /// </summary>
        public InputDebug m_debug;

        #endregion

        #region Properties

        /// <summary>
        /// Reference to the input scheme.
        /// </summary>
        public InputScheme Scheme
        {
            get
            {
                if (this.m_scheme == null)
                {
                    this.m_scheme = Services.CreateComponent<InputScheme>(this.gameObject);
                }
                return this.m_scheme;
            }
        }

        /// <summary>
        /// Reference to the input display.
        /// </summary>
        public InputDisplay Display
        {
            get
            {
                if (this.m_display == null)
                {
                    this.m_display = Services.CreateComponent<InputDisplay>(this.gameObject);
                    this.m_display.activeColor = this.m_inputColor;
                }
                return this.m_display;
            }
        }
        
        /// <summary>
        /// Input debug component.
        /// </summary>
        public InputDebug Debug
        {
            get { return this.m_debug; }
        }

        /// <summary>
        /// Initialization flag reference.
        /// </summary>
        public bool Initialized
        {
            get { return this.m_initialized; }
        }
        
        #endregion

        #endregion

        #region UnityEngine Methods

        /// <summary>
        /// Start method for the component.
        /// </summary>
        public void Start()
        {
            this.Initialize();
        }

        /// <summary>
        /// Update method called every cycle.
        /// </summary>
        public void Update()
        {
            HandleInput();
        }

        /// <summary>
        /// Handle input scheme triggers here.
        /// </summary>
        public virtual void HandleInput()
        {
            // For overriding by children components.
        }

        /// <summary>
        /// Add message to debug module, if it exists.
        /// </summary>
        protected void AddMessage(string _message)
        {
            if (_message.Length > 0 && this.m_debug != null)
            {
                string message = "[" + gameObject.name + "]: " + _message;
                this.m_debug.AddMessage(message);
            }
        }
            
        #endregion

        #region Initialization Methods

        /// <summary>
        /// Initialize the component.
        /// </summary>
        public void Initialize()
        {
            if (!this.Initialized)
            {
                this.m_scheme = this.Scheme;
                this.InitializeTriggers();
                this.m_initialized = true;
            }
        }

        /// <summary>
        /// Create input scheme triggers here.
        /// </summary>
        public virtual void InitializeTriggers()
        {
            // For overriding by children components.
        }

        #endregion

    }
}
