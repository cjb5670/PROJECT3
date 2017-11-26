/*************************************************
 * RightTriggerInput.cs
 * 
 * This file contains:
 * - The RightTriggerInput class.
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
    /// The right trigger input, based off of the controller input component.
    /// </summary>
    public class RightTriggerInput : ControllerInput
    {

        #region Trigger Keys.

        const string RTRIG = "Right Trigger";

        #endregion

        #region Trigger Initialization.

        /// <summary>
        /// Create input scheme triggers here.
        /// </summary>
        public override void InitializeTriggers()
        {
            this.Scheme.AddTrigger(RTRIG, Controls.RTRIG, ControlResponse.AXIS);
        }

        #endregion

        #region Handle Input.

        /// <summary>
        /// Handle input scheme triggers here.
        /// </summary>
        public override void HandleInput()
        {
            Trigger rightTrigger = this.Scheme.GetTrigger(RTRIG);
            this.Display.UpdateDisplay(rightTrigger.GetAxis());

            if (rightTrigger.GetAxis() != 0.0f)
            {
                this.AddMessage("Right Trigger Value: " + rightTrigger.GetAxis());
            }
        }

        #endregion

    }
}
