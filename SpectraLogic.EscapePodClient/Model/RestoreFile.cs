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

using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class RestoreFile
    {
        [DataMember(Order = 1)] private string Name;
        [DataMember(Order = 2)] private string Destination;
        [DataMember(Order = 3)] private bool RestoreFileAttributes;
        [DataMember(Order = 4)] private ByteRange ByteRange;
        [DataMember(Order = 5)] private TimecodeRange TimeCodeRange;

        public RestoreFile(string name, string destination, bool restoreFileAttributes)
        {
            Name = name;
            Destination = destination;
            RestoreFileAttributes = restoreFileAttributes;
        }

        public RestoreFile(string name, string destination, ByteRange byteRange)
        {
            Name = name;
            Destination = destination;
            ByteRange = byteRange;
        }

        public RestoreFile(string name, string destination, TimecodeRange timeCodeRange)
        {
            Name = name;
            Destination = destination;
            TimeCodeRange = timeCodeRange;
        }
    }
}
