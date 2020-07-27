/*
 * ******************************************************************************
 *   Copyright 2014-2020 Spectra Logic Corporation. All Rights Reserved.
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

using Newtonsoft.Json;
using SpectraLogic.SpectraRioBrokerClient.Model;
using System;

namespace SpectraLogic.SpectraRioBrokerClient.Utils.JsonConverters
{
    internal class TimeCodeRangeJsonConverter : JsonConverter<TimeCodeRange>
    {
        #region Methods

        public override TimeCodeRange ReadJson(JsonReader reader, Type objectType, TimeCodeRange existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, TimeCodeRange value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        #endregion Methods
    }
}