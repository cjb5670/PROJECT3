/*************************************************
 * LeftStickInput.cs
 * 
 * This file contains:
 * - The LeftStickInput class.
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
    /// The left stick input, based off of the controller input component.
    /// </summary>
    public class LeftStickInput : ControllerInput
    {

        #region Trigger Keys.

        const string LS_X = "Left Stick X";
        const string LS_Y = "Left Stick Y";

        #endregion
        
        #region Trigger Initialization.

        /// <summary>
        /// Create input scheme triggers here.
        /// </summary>
        public override void InitializeTriggers()
        {
            this.Scheme.AddTrigger(LS_X, Controls.LS_X, ControlResponse.AXIS);
            this.Scheme.AddTrigger(LS_Y, Controls.LS_Y, ControlResponse.AXIS);
        }

        #endregion

        #region Handle Input.

        /// <summary>
        /// Handle input scheme triggers here.
        /// </summary>
        public override void HandleInput()
        {
            Trigger x = this.Scheme.GetTrigger(LS_X);
            Trigger y = this.Scheme.GetTrigger(LS_Y);
                        
            Vector3 offset = new Vector3(x.GetAxis(), -y.GetAxis(), 0.0f);

            this.Display.UpdateDisplay(Vector3.Distance(this.Display.transform.position, this.Display.transform.position + offset));
            this.Display.UpdatePosition(offset, 1.0f);

            if (offset != Vector3.zero)
            {
                this.AddMessage("Left Stick Offset: " + ((Vector2)(offset)).ToString());
            }
        }

        #endregion

    }
}
