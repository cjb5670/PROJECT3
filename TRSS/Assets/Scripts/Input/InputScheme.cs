/*************************************************
 * InputScheme.cs
 * 
 * This file contains:
 * - The InputScheme class.
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
    /// An input scheme stores a series of controls (and responses), keeping track of values each frame.
    /// </summary>
    public class InputScheme : MonoBehaviour
    {

        #region Data Members

        #region Fields

        /// <summary>
        /// Reference to the input manager that should be kept track of.
        /// </summary>
        private InputManager m_manager = null;

        /// <summary>
        /// Collection of triggers this input scheme will keep track of.
        /// </summary>
        private Dictionary<string, Trigger> m_triggers = null;

        /// <summary>
        /// Collection of keys.
        /// </summary>
        private List<string> m_keys = null;

        /// <summary>
        /// Initialization flag.
        /// </summary>
        private bool m_initialized = false;
        
        #endregion

        #region Properties

        /// <summary>
        /// Reference to the input manager.
        /// </summary>
        public InputManager Manager
        {
            get
            {
                if (m_manager == null)
                {
                    m_manager = InputManager.GetInstance();
                }
                return m_manager;
            }
        }

        /// <summary>
        /// Reference to collection of triggers.
        /// </summary>
        public Dictionary<string, Trigger> Triggers
        {
            get
            {
                if (m_triggers == null)
                {
                    m_triggers = new Dictionary<string, Trigger>();
                }
                return m_triggers;
            }
        }

        /// <summary>
        /// Reference to collection of keys.
        /// </summary>
        private List<string> Keys
        {
            get
            {
                if (m_keys == null)
                {
                    m_keys = new List<string>();
                }
                return m_keys;
            }
        }

        /// <summary>
        /// Initialization flag.
        /// </summary>
        public bool Initialized
        {
            get { return this.m_initialized; }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// Initialize the input scheme.
        /// </summary>
        public void Start()
        {
            this.Initialize();
        }

        /// <summary>
        /// Update will handle input values and update all triggers.
        /// </summary>
        public void Update()
        {
            // Initialize if it hasn't been done yet.
            this.Initialize();

            // Update triggers if it has any.
            if (this.HasTriggers())
            {
                foreach (Trigger t in GetTriggers())
                {
                    t.Update();
                }
            }
        }

        #endregion

        #region Initialization Methods.

        /// <summary>
        /// Initialization method.
        /// </summary>
        public void Initialize()
        {
            if (!this.Initialized)
            {
                this.m_manager = this.Manager;
                this.m_triggers = this.Triggers;
                this.m_initialized = true;
            }
        }

        #endregion

        #region Check Trigger Methods.

        /// <summary>
        /// Check if the key is valid.
        /// </summary>
        /// <param name="_key">Input key.</param>
        /// <returns>Returns true if length is greater than zero.</returns>
        public bool ValidKey(string _key)
        {
            // Check length.
            return (_key.Trim().Length != 0);
        }

        /// <summary>
        /// Formats a string value to match the anticipated key format.
        /// </summary>
        /// <param name="_key">String to turn into key.</param>
        /// <returns>Returns string.</returns>
        public string MakeKey(string _key)
        {
            return _key.Trim().ToUpper();
        }

        /// <summary>
        /// Returns true if the input key is valid and exists in the collection.
        /// </summary>
        /// <param name="_key">Input key to check.</param>
        /// <returns>Returns true if it is valid and is in the collection.</returns>
        public bool HasKey(string _key)
        {
            string key = MakeKey(_key);
            return (HasTriggers() && ValidKey(key) && this.Triggers.ContainsKey(key));
        }

        /// <summary>
        /// Checks to see if there are any triggers.
        /// </summary>
        /// <returns>Returns true if collection count is greater than zero.</returns>
        public bool HasTriggers()
        {
            return (this.Triggers.Count() > 0);
        }

        /// <summary>
        /// Returns the trigger associated with a given key.
        /// </summary>
        /// <param name="_key">Input key to find trigger for.</param>
        /// <returns>Returns a trigger struct if one exists.</returns>
        public Trigger GetTrigger(string _key)
        {
            // Null trigger.
            Trigger response = new Trigger(Controls.KB_ESCAPE, ControlResponse.NONE);
            string key = MakeKey(_key);

            if(HasKey(_key))
            {
                return this.Triggers[key];
            }
            
            // Null trigger.
            return response;
        }

        /// <summary>
        /// Returns true if a trigger with a response mode, different from NONE, is returned.
        /// </summary>
        /// <param name="_key">Key.</param>
        /// <param name="_response">Trigger that is returned via 'out'.</param>
        /// <returns>Returns a true if a non-None trigger exists.</returns>
        public bool TryGetTrigger(string _key, out Trigger _response)
        {
            _response = GetTrigger(_key);
            return (_response.Mode != ControlResponse.NONE);
        }
        
        /// <summary>
        /// Removes trigger from the collection, if key is valid and it exists.
        /// </summary>
        /// <param name="_key">Key assigned to trigger for removal.</param>
        /// <param name="_response">Trigger object returned, if one such trigger object exists.</param>
        /// <returns>Returns the trigger that was just removed.</returns>
        public bool RemoveTrigger(string _key, out Trigger _response)
        {
            string key = MakeKey(_key);

            // If we can get a trigger out, we can remove it.
            if (TryGetTrigger(key, out _response))
            {
                this.Triggers.Remove(key);
                this.Keys.Remove(key);
                return true;
            }

            // No trigger to remove.
            return false;
        }

        /// <summary>
        /// Returns a collection of triggers.
        /// </summary>
        /// <returns>Returns list.</returns>
        public List<Trigger> GetTriggers()
        {
            List<Trigger> response = new List<Trigger>();

            if (HasTriggers())
            {
                foreach (string key in this.Keys)
                {
                    response.Add(this.Triggers[key]);
                }
            }

            return response;
        }

        #endregion

        #region Mutator Trigger Methods.

        /// <summary>
        /// Adds a trigger to the collection, if key is valid.
        /// </summary>
        /// <param name="_key">Key to assign to trigger.</param>
        /// <param name="_control">Control to assign to key.</param>
        /// <param name="_response">Response mode to assign to key.</param>
        public void AddTrigger(string _key, Controls _control, ControlResponse _response)
        {
            this.AddTrigger(_key, new Trigger(_control, _response));
        }

        /// <summary>
        /// Adds a trigger to the collection, if key is valid.
        /// </summary>
        /// <param name="_key">Key to assign to trigger.</param>
        /// <param name="_trigger">Trigger to assign to key.</param>
        public void AddTrigger(string _key, Trigger _trigger)
        {
            string key = MakeKey(_key);

            if (ValidKey(key) && !HasKey(key))
            {
                this.Triggers.Add(key, _trigger);
                this.Keys.Add(key);
            }
        }

        #endregion

    }

}
