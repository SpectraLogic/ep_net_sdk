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

using NUnit.Framework;
using SpectraLogic.SpectraRioBrokerClient.Exceptions;
using SpectraLogic.SpectraRioBrokerClient.Model;
using System.Threading.Tasks;

namespace SpectraLogic.SpectraRioBrokerClient.Test
{
    [TestFixture]
    internal class TimeCodeRangeTests
    {
        #region Methods

        [Test]
        public void DropTimeCodeRangeTest()
        {
            var timeCodeRange = new TimeCodeRange(new TimeCode(01, 08, 59, 28, true), new TimeCode(01, 08, 59, 29, true));
            Assert.AreEqual("01:08:59;28-01:08:59;29", timeCodeRange.ToString());
        }

        [Test]
        public void DropTimeCodeTest()
        {
            var dropTimeCode = new TimeCode(01, 08, 59, 28, true);
            Assert.AreEqual("01:08:59;28", dropTimeCode.ToString());
        }

        [Test]
        public void MixTimeCodeRangeTest()
        {
            Assert.ThrowsAsync<MixTimeCodeException>(() => Task.FromResult(new TimeCodeRange(new TimeCode(01, 08, 59, 28, false), new TimeCode(01, 08, 59, 29, true))));
            Assert.ThrowsAsync<MixTimeCodeException>(() => Task.FromResult(new TimeCodeRange(new TimeCode(01, 08, 59, 28, true), new TimeCode(01, 08, 59, 29, false))));
        }

        [Test]
        public void NonDropTimeCodeRangeTest()
        {
            var timeCodeRange = new TimeCodeRange(new TimeCode(01, 08, 59, 28, false), new TimeCode(01, 08, 59, 29, false));
            Assert.AreEqual("01:08:59:28-01:08:59:29", timeCodeRange.ToString());
        }

        [Test]
        public void NonDropTimeCodeTest()
        {
            var nonDropTimeCode = new TimeCode(01, 08, 59, 28, false);
            Assert.AreEqual("01:08:59:28", nonDropTimeCode.ToString());
        }

        #endregion Methods
    }
}