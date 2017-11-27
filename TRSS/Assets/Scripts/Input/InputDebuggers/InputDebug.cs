using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TRSS.Input
{
    public class InputDebug : MonoBehaviour
    {

        #region Data Members

        #region Fields

        /// <summary>
        /// Messages list.
        /// </summary>
        private List<string> m_messages = null;

        /// <summary>
        /// Text box.
        /// </summary>
        public Text text = null;

        /// <summary>
        /// Debug mode flag for this input debug module.
        /// </summary>
        public bool m_displayDebugMessages = true;

        #endregion

        #region Properties

        /// <summary>
        /// Determines if any messages should be displayed based on internal debug flag and Services debug flag.
        /// </summary>
        public bool Debug
        {
            get { return Debugger.DEBUG_MODE && this.m_displayDebugMessages; }
        }

        /// <summary>
        /// Reference to the messages list.
        /// </summary>
        private List<string> Messages
        {
            get
            {
                if (this.m_messages == null)
                {
                    this.m_messages = new List<string>();
                }
                return this.m_messages;
            }
        }

        /// <summary>
        /// Return reference to the Text UI component.
        /// </summary>
        public Text TextBox
        {
            get
            {
                if (text == null)
                {
                    text = gameObject.GetComponent<Text>();
                }
                return text;
            }
        }

        #endregion

        #endregion

        #region UnityEngine Methods.

        /// <summary>
        /// On start, get the text component and the messages list.
        /// </summary>
        public void Start()
        {
            ClearMessages();
        }

        /// <summary>
        /// Update the debug messages.
        /// </summary>
        public void Update()
        {
            string toPrint = "";

            if (this.Debug)
            {
                toPrint = "Debug message: \n";

                // Check if there are any messages to print.
                if (this.Messages.Count > 0)
                {
                    toPrint = "Debug message: \n";

                    foreach (string msg in this.Messages)
                    {
                        toPrint += msg + "\n";
                    }

                    // Clear message queue.
                    ClearMessages();
                }
                else
                {
                    toPrint = "There are no debug messages.";
                }
            }

            this.TextBox.text = toPrint;
        }

        #endregion

        #region Service Methods.
        
        /// <summary>
        /// Add a message to the message queue.
        /// </summary>
        /// <param name="_message">Add a message, as long as it isn't empty.</param>
        public void AddMessage(string _message)
        {
            if (this.Debug && _message.Length > 0)
            {
                this.Messages.Add(_message);
            }
        }

        /// <summary>
        /// Clear the messages list and set it to null.
        /// </summary>
        public void ClearMessages()
        {
            if (this.m_messages != null && this.m_messages.Count > 0)
            {
                this.m_messages.Clear();
                this.m_messages = null;
            }
        }

        /// <summary>
        /// Toggle the debug mode.
        /// </summary>
        public void ToggleDisplay()
        {
            this.m_displayDebugMessages = !this.m_displayDebugMessages;
        }

        #endregion
        
    }
}
