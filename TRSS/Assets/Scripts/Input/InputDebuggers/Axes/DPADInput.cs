/*************************************************
 * DPADInput.cs
 * 
 * This file contains:
 * - The DPADInput class.
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
    /// The DPAD input, based off of the controller input component.
    /// </summary>
    public class DPADInput : ControllerInput
    {

        #region Trigger Keys.

        const string DPAD_X = "Digital Pad X";
        const string DPAD_Y = "Digital Pad Y";

        #endregion

        #region Trigger Initialization.

        /// <summary>
        /// Create input scheme triggers here.
        /// </summary>
        public override void InitializeTriggers()
        {
            this.Scheme.AddTrigger(DPAD_X, Controls.DPAD_X, ControlResponse.AXIS);
            this.Scheme.AddTrigger(DPAD_Y, Controls.DPAD_Y, ControlResponse.AXIS);
        }

        #endregion

        #region Handle Input.

        /// <summary>
        /// Handle input scheme triggers here.
        /// </summary>
        public override void HandleInput()
        {
            Trigger x = this.Scheme.GetTrigger(DPAD_X);
            Trigger y = this.Scheme.GetTrigger(DPAD_Y);

            Vector3 offset = new Vector3(x.GetAxis(), y.GetAxis(), 0.0f);

            this.Display.UpdateDisplay(Vector3.Distance(this.Display.transform.position, this.Display.transform.position + offset));
            this.Display.UpdatePosition(offset, 1.0f);

            if (offset != Vector3.zero)
            {
                this.AddMessage("DPAD Offset: " + ((Vector2)(offset)).ToString());
            }
        }

        #endregion

    }
}
