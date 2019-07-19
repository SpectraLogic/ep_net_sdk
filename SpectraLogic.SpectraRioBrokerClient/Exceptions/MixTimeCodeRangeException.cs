﻿/*
 * ******************************************************************************
 *   Copyright 2014-2019 Spectra Logic Corporation. All Rights Reserved.
 *   Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *   this file except in compliance with the License. A copy of the License is located at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 *   or in the "license" file accompanying this file.
 *   This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *   CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *   specific language governing permissions and limitations under the License.
 * ****************************************************************************
 */

using System;

namespace SpectraLogic.SpectraRioBrokerClient.Exceptions
{
    /// <summary>
    /// This exception will be thrown when a user uses drop and non-drop frames when creating a TimeCodeRange
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class MixTimeCodeException : Exception
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="MixTimeCodeException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public MixTimeCodeException(string message) : base(message)
        {
        }

        #endregion Constructors
    }
}