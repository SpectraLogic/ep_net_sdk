/*
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

using log4net;
using SpectraLogic.SpectraRioBrokerClient.Model;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Integration.Test
{
    public class ValidationErrorComparer : Comparer<ValidationError>
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger("ValidationErrorComparer");

        #endregion Fields

        #region Methods

        public override int Compare(ValidationError x, ValidationError y)
        {
            var ret = 0;

            if (!x.ErrorType.Equals(y.ErrorType))
            {
                Log.Error($"expected ErrorType to be '{x.ErrorType}' but was '{y.ErrorType}'");
                ret = 1;
            }

            if (!x.FieldName.Equals(y.FieldName))
            {
                Log.Error($"expected FieldName to be '{x.FieldName}' but was '{y.FieldName}'");
                ret = 1;
            }

            if (!x.FieldType.Equals(y.FieldType))
            {
                Log.Error($"expected FieldType to be '{x.FieldType}' but was '{y.FieldType}'");
                ret = 1;
            }

            if (x.Value == null && y.Value != null ||
                x.Value != null && y.Value == null ||
                x.Value != null && y.Value != null && !x.Value.Equals(y.Value))
            {
                Log.Error($"expected Value to be '{x.Value}' but was '{y.Value}'");
                ret = 1;
            }

            if (x.Reason == null && y.Reason != null ||
                x.Reason != null && y.Reason == null ||
                x.Reason != null && y.Reason != null && !x.Reason.Equals(y.Reason))
            {
                Log.Error($"expected Reason to be '{x.Reason}' but was '{y.Reason}'");
                ret = 1;
            }

            return ret;
        }

        #endregion Methods
    }
}