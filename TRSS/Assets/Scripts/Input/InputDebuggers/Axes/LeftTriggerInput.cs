/*************************************************
 * LeftTriggerInput.cs
 * 
 * This file contains:
 * - The LeftTriggerInput class.
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
    /// The left trigger input, based off of the controller input component.
    /// </summary>
    public class LeftTriggerInput : ControllerInput
    {

        #region Trigger Keys.

        const string LTRIG = "Left Trigger";
        
        #endregion

        #region Trigger Initialization.

        /// <summary>
        /// Create input scheme triggers here.
        /// </summary>
        public override void InitializeTriggers()
        {
            this.Scheme.AddTrigger(LTRIG, Controls.LTRIG, ControlResponse.AXIS);
        }

        #endregion

        #region Handle Input.

        /// <summary>
        /// Handle input scheme triggers here.
        /// </summary>
        public override void HandleInput()
        {
            Trigger leftTrigger = this.Scheme.GetTrigger(LTRIG);
            this.Display.UpdateDisplay(leftTrigger.GetAxis());

            if (leftTrigger.GetAxis() != 0.0f)
            {
                this.AddMessage("Left Trigger Value: " + leftTrigger.GetAxis());
            }
        }

        #endregion

    }
}
