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

using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Model;

namespace SpectraLogic.EscapePodClient
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEscapePodClient
    {
        /// <summary>
        /// Gets the archive.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ArchiveNotFoundException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapoPodServerCredentialsException" />
        /// <returns></returns>
        IEscapePodArchive GetArchive(GetArchiveRequest request);

        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEscapePodJob GetJob(GetEscapePodJob request);
        
        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEscapePodJob Delete(DeleteRequest request);
        
        /// <summary>
        /// Restores the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEscapePodJob Restore(RestoreRequest request);
        
        /// <summary>
        /// Archives the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEscapePodJob Archive(ArchiveRequest request);
        
        /// <summary>
        /// Cancels the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEscapePodJob Cancel(CancelRequest request);
        
        /// <summary>
        /// Creates the archive.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEscapePodArchive CreateArchive(CreateArchiveRequest request);
    }
}
