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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    /// 
    /// </summary>
    public enum JobsSortByEnum
    {
        /// <summary>
        /// Sort by creation date
        /// </summary>
        CREATION_DATE,
        
        /// <summary>
        /// Sort by last updated
        /// </summary>
        LAST_UPDATED,
        
        /// <summary>
        /// Sort by type
        /// </summary>
        TYPE,
        
        /// <summary>
        /// Sort by job id
        /// </summary>
        JOB_ID,
        
        /// <summary>
        /// Sort by status
        /// </summary>
        STATUS,
        
        /// <summary>
        /// Sort by number of files
        /// </summary>
        NUMBER_OF_FILES,
        
        /// <summary>
        /// Sort by file transferred
        /// </summary>
        FILES_TRANSFERRED,
        
        /// <summary>
        /// Sort by total size in bytes
        /// </summary>
        TOTAL_SIZE_IN_BYTES,
        
        /// <summary>
        /// Sort by progress
        /// </summary>
        PROGRESS
    }
}