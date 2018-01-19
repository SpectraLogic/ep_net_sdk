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

using System.Linq;

namespace SpectraLogic.EscapePodClient.Calls
{
    public class CancelRequest : RestRequest
    {
        public CancelRequest(string escapePodJobId)
        {
            AddQueryParam("id", escapePodJobId);
        }

        public CancelRequest(long escapePodJobId)
        {
            AddQueryParam("id", escapePodJobId.ToString());
        }

        internal override HttpVerb Verb => HttpVerb.PUT;
        internal override string Path => "api/cancel"; //TODO use the right path
        public override string ToString()
        {
            return $"{Path}?{string.Join(";", QueryParams.Select(q => q.Key + "=" + q.Value))}\n{Verb}";
        }
    }
}
