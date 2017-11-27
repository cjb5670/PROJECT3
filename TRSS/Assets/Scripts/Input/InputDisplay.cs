/*************************************************
 * InputDisplay.cs
 * 
 * This file contains:
 * - The InputDisplay class.
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
    /// An input display object displays whether or not a particular input has been pressed down,
    /// by changing the material color on an object.
    /// </summary>
    public class InputDisplay : MonoBehaviour
    {

        #region Fields

        /// <summary>
        /// Starting color of the input material.
        /// </summary>
        private Color m_startColor;

        /// <summary>
        /// Ending color of the input material.
        /// </summary>
        public Color activeColor;

        /// <summary>
        /// Interpolation value for the material color.
        /// </summary>
        private float m_value;

        /// <summary>
        /// Reference to material of the object.
        /// </summary>
        private Material m_material;

        /// <summary>
        /// Current color.
        /// </summary>
        private Color m_currentColor;

        /// <summary>
        /// The base position in which things are based off of.
        /// </summary>
        private Vector3 m_basePosition;

        /// <summary>
        /// The target position that the display will lerp to.
        /// </summary>
        private Vector3 m_targetPosition;

        /// <summary>
        /// The offset applied to the base position.
        /// </summary>
        private Vector3 m_offset;

        /// <summary>
        /// Initialization flag.
        /// </summary>
        private bool m_initialized = false;

        #endregion

        #region Properties

        /// <summary>
        /// Reference to the material property.
        /// </summary>
        public Material Material
        {
            get
            {
                if (m_material == null)
                {
                    m_material = this.gameObject.GetComponent<MeshRenderer>().material;
                    m_startColor = this.m_material.color;
                }
                return m_material;
            }
        }

        /// <summary>
        /// Return the current, actual position.
        /// </summary>
        public Vector3 Position
        {
            get { return this.gameObject.transform.position; }
            private set { this.gameObject.transform.position = value; }
        }

        /// <summary>
        /// Return the target position.
        /// </summary>
        public Vector3 TargetPosition
        {
            get { return this.m_targetPosition; }
            private set { this.m_targetPosition = value; }
        }

        /// <summary>
        /// Reference to initialization flag.
        /// </summary>
        public bool Initialized
        {
            get { return this.m_initialized; }
        }

        #endregion

        #region UnityEngine Methods

        /// <summary>
        /// Get the start color and material references.
        /// </summary>
        public void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Lerp to the target position.
        /// </summary>
        public void Update()
        {
            Vector3 LerpPosition = Vector3.Lerp(this.Position, this.TargetPosition, Time.deltaTime * 10.0f);

            if (Vector3.Distance(this.Position, LerpPosition) < 0.005f)
            {
                this.Position = this.TargetPosition;
            }
            else
            {
                this.Position = LerpPosition;
            }
        }

        #endregion

        #region Service Methods

        /// <summary>
        /// Initialization method.
        /// </summary>
        public void Initialize()
        {
            if (!this.Initialized)
            {
                // Get the start color and material reference.
                m_material = this.gameObject.GetComponent<MeshRenderer>().material;
                m_startColor = this.Material.color;
                m_basePosition = this.Position;
                m_targetPosition = this.Position;
                m_offset = Vector3.zero;
                ResetDisplay();
                this.m_initialized = true;
            }
        }

        /// <summary>
        /// Reset the color settings.
        /// </summary>
        public void ResetDisplay()
        {
            this.SetValue(0.0f);
            m_currentColor = m_startColor;
        }

        /// <summary>
        /// Calculate the current color based off of the interpolation value.
        /// </summary>
        private void CalculateColor()
        {
            m_currentColor = Color.Lerp(m_startColor, activeColor, m_value);
        }

        /// <summary>
        /// Set the value, clamped between 0 and 1f.
        /// </summary>
        /// <param name="_value">Value to set.</param>
        private void SetValue(float _value)
        {
            m_value = Mathf.Clamp(Math.Abs(_value), 0.0f, 1.0f);
        }

        /// <summary>
        /// Updating the display will assign a new value and change the material color.
        /// </summary>
        /// <param name="_value">Value to assign.</param>
        public void UpdateDisplay(float _value)
        {
            Initialize();
            // Set the interpolation value.
            SetValue(_value);

            // Calculate the new current color.
            CalculateColor();

            // Set the material color accordingly.
            this.Material.color = m_currentColor;

        }

        /// <summary>
        /// Set the offset as a clamped vector.
        /// </summary>
        /// <param name="_offset">Offset to be added.</param>
        private void SetOffset(Vector3 _offset)
        {
            m_offset = Vector3.ClampMagnitude(_offset, 1.0f);
        }

        /// <summary>
        /// Changes the position based on the input offset, it's applied magnitude and it's base position.
        /// </summary>
        /// <param name="_offset">Offset to apply.</param>
        /// <param name="_magnitude">Magnitude of the offset to apply.</param>
        public void UpdatePosition(Vector3 _offset, float _magnitude)
        {
            Initialize();

            // Set the offset.
            SetOffset(_offset);

            // Calculate the new position.
            Vector3 position = m_basePosition + (m_offset * _magnitude);

            if (this.TargetPosition != position)
            {
                // Set the new position.
                this.TargetPosition = position;
            }
        }

        #endregion

    }
}
