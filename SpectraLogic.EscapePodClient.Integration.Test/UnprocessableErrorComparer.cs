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

using log4net;
using SpectraLogic.EscapePodClient.Model;
using System.Collections.Generic;

namespace SpectraLogic.EscapePodClient.Integration.Test
{
    public class UnprocessableErrorComparer : Comparer<UnprocessableError>
    {
        #region Fields

        private static ILog LOG = LogManager.GetLogger("UnprocessableErrorComparer");

        #endregion Fields

        #region Methods

        public override int Compare(UnprocessableError x, UnprocessableError y)
        {
            if (!x.ErrorType.Equals(y.ErrorType))
            {
                LOG.Error($"expected ErrorType to be {x.ErrorType} but was {y.ErrorType}");
                return 1;
            }

            if (!x.FieldName.Equals(y.FieldName))
            {
                LOG.Error($"expected FieldName to be {x.FieldName} but was {y.FieldName}");
                return 1;
            }

            if (!x.FieldType.Equals(y.FieldType))
            {
                LOG.Error($"expected FieldType to be {x.FieldType} but was {y.FieldType}");
                return 1;
            }

            return 0;
        }

        #endregion Methods
    }
}