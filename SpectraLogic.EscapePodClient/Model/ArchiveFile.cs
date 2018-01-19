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
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ArchiveFile
    {
        /// <summary>
        /// The name
        /// </summary>
        [DataMember(Order = 1)] public string Name;
        /// <summary>
        /// The URI
        /// </summary>
        [DataMember(Order = 2)] public string Uri;
        /// <summary>
        /// The size
        /// </summary>
        [DataMember(Order = 3)] public long Size;
        /// <summary>
        /// The metadata
        /// </summary>
        [DataMember(Order = 4)] public IDictionary<string, string> Metadata;
        /// <summary>
        /// The links
        /// </summary>
        [DataMember(Order = 5)] public IEnumerable<string> Links;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveFile"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="size">The size.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="links">The links.</param>
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
