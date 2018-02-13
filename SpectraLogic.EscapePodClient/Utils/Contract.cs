/*
 * ******************************************************************************
 *   Copyright 2014-2018 Spectra Logic Corporation. All Rights Reserved.
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

namespace SpectraLogic.EscapePodClient.Utils
{
    /// <summary>
    ///
    /// </summary>
    public class Contract
    {
        #region Methods

        /// <summary>
        /// Requireses the specified predicate.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">if set to <c>true</c> [predicate].</param>
        /// <param name="message">The message.</param>
        public static void Requires<TException>(bool predicate, string message)
        where TException : Exception
        {
            if (!predicate)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        #endregion Methods
    }
}