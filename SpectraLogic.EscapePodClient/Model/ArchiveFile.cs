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

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class ArchiveFile
    {
        [DataMember(Order = 1)] public string Name;
        [DataMember(Order = 2)] public string Uri;
        [DataMember(Order = 3)] public long Size;
        [DataMember(Order = 4)] public IDictionary<string, string> Metadata;
        [DataMember(Order = 5)] public IEnumerable<string> Links;

        public ArchiveFile(string name, string uri, long size, IDictionary<string, string> metadata, IEnumerable<string> links)
        {
            Name = name;
            Uri = uri;
            Size = size;
            Metadata = metadata;
            Links = links;
        }
    }
}
