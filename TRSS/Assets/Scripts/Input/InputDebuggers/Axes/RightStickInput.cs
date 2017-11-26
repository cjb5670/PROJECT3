/*************************************************
 * RightStickInput.cs
 * 
 * This file contains:
 * - The RightStickInput class.
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
    /// The right stick input, based off of the controller input component.
    /// </summary>
    public class RightStickInput : ControllerInput
    {

        #region Trigger Keys.

        const string RS_X = "Right Stick X";
        const string RS_Y = "Right Stick Y";

        #endregion

        #region Trigger Initialization.

        /// <summary>
        /// Create input scheme triggers here.
        /// </summary>
        public override void InitializeTriggers()
        {
            this.Scheme.AddTrigger(RS_X, Controls.RS_X, ControlResponse.AXIS);
            this.Scheme.AddTrigger(RS_Y, Controls.RS_Y, ControlResponse.AXIS);
        }

        #endregion

        #region Handle Input.

        /// <summary>
        /// Handle input scheme triggers here.
        /// </summary>
        public override void HandleInput()
        {
            Trigger x = this.Scheme.GetTrigger(RS_X);
            Trigger y = this.Scheme.GetTrigger(RS_Y);

            Vector3 offset = new Vector3(x.GetAxis(), -y.GetAxis(), 0.0f);

            this.Display.UpdateDisplay(Vector3.Distance(this.Display.transform.position, this.Display.transform.position + offset));
            this.Display.UpdatePosition(offset, 1.0f);

            if (offset != Vector3.zero)
            {
                this.AddMessage("Right Stick Offset: " + ((Vector2)(offset)).ToString());
            }
        }

        #endregion

    }
}
