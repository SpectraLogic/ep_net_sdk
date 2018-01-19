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
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.Utils;

namespace SpectraLogic.EscapePodClient.Calls
{
    [DataContract]
    public class ArchiveRequest : RestRequest
    {
        [DataMember] public IEnumerable<ArchiveFile> Files;

        public ArchiveRequest() { }

        public ArchiveRequest(IEnumerable<ArchiveFile> files)
        {
            Files = files;
        }

        internal override HttpVerb Verb => HttpVerb.POST;
        internal override string Path => "api/archive"; //TODO use the right path
        public override string ToString()
        {
            return $"{Path}\n{Verb}\n{GetBody()}";
        }

        internal override string GetBody()
        {
            return HttpUtils<ArchiveRequest>.ObjectToJson(this);
        }
    }
}
