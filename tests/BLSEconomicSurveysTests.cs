/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using QuantConnect.Data;
using QuantConnect.DataSource;

namespace QuantConnect.DataLibrary.Tests
{
    [TestFixture]
    public class BLSEconomicSurveysTests
    {
        private readonly string _cpiSamplePath = Path.Combine("output", "alternative", "bls", "economicsurveys", "cpi.csv");
        private readonly string _cesSamplePath = Path.Combine("output", "alternative", "bls", "economicsurveys", "ces.csv");
        private readonly string _ppiSamplePath = Path.Combine("output", "alternative", "bls", "economicsurveys", "ppi.csv");
        private readonly string _joltsSamplePath = Path.Combine("output", "alternative", "bls", "economicsurveys", "jolts.csv");

        private SubscriptionDataConfig _cpiConfig;
        private SubscriptionDataConfig _cesConfig;
        private SubscriptionDataConfig _ppiConfig;
        private SubscriptionDataConfig _joltsConfig;

        [SetUp]
        public void SetUp()
        {
            _cpiConfig = CreateConfig<BLSEconomicSurveysCpi>("CPI");
            _cesConfig = CreateConfig<BLSEconomicSurveysCes>("CES");
            _ppiConfig = CreateConfig<BLSEconomicSurveysPpi>("PPI");
            _joltsConfig = CreateConfig<BLSEconomicSurveysJolts>("JOLTS");
        }

        private static SubscriptionDataConfig CreateConfig<T>(string ticker) where T : BaseData
        {
            return new SubscriptionDataConfig(
                typeof(T),
                Symbol.Create(ticker, SecurityType.Base, Market.USA),
                Resolution.Daily,
                TimeZones.NewYork,
                TimeZones.NewYork,
                false,
                false,
                false
            );
        }

        // ==================== CPI Tests ====================

        [Test]
        public void CpiSampleDataFileExists()
        {
            Assert.IsTrue(File.Exists(_cpiSamplePath), $"Missing: {_cpiSamplePath}");
        }

        [Test]
        public void CpiReaderParsesAllSampleDataLines()
        {
            Assert.IsTrue(File.Exists(_cpiSamplePath));
            var lines = File.ReadAllLines(_cpiSamplePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            Assert.IsNotEmpty(lines);

            var instance = new BLSEconomicSurveysCpi();
            foreach (var line in lines)
            {
                var result = instance.Reader(_cpiConfig, line, DateTime.UtcNow, false) as BLSEconomicSurveysCpi;
                Assert.IsNotNull(result, $"Reader returned null for line: {line}");
                Assert.AreEqual(_cpiConfig.Symbol, result.Symbol);
                Assert.AreNotEqual(default(DateTime), result.Time);
                Assert.AreNotEqual(default(DateTime), result.EndTime);
                Assert.Greater(result.EndTime, result.Time, "EndTime must be after Time");
            }
        }

        [Test]
        public void CpiCloneCopiesAllProperties()
        {
            var firstLine = File.ReadLines(_cpiSamplePath).First();
            var instance = new BLSEconomicSurveysCpi();
            var original = instance.Reader(_cpiConfig, firstLine, DateTime.UtcNow, false) as BLSEconomicSurveysCpi;
            Assert.IsNotNull(original);

            var clone = original.Clone() as BLSEconomicSurveysCpi;
            Assert.IsNotNull(clone);
            AssertAreEqual(original, clone);
        }

        [Test]
        public void CpiJsonRoundTrip()
        {
            var firstLine = File.ReadLines(_cpiSamplePath).First();
            var instance = new BLSEconomicSurveysCpi();
            var original = instance.Reader(_cpiConfig, firstLine, DateTime.UtcNow, false) as BLSEconomicSurveysCpi;
            Assert.IsNotNull(original);

            var json = JsonConvert.SerializeObject(original);
            var deserialized = JsonConvert.DeserializeObject<BLSEconomicSurveysCpi>(json);
            Assert.IsNotNull(deserialized);
        }

        // ==================== CES Tests ====================

        [Test]
        public void CesSampleDataFileExists()
        {
            Assert.IsTrue(File.Exists(_cesSamplePath), $"Missing: {_cesSamplePath}");
        }

        [Test]
        public void CesReaderParsesAllSampleDataLines()
        {
            var lines = File.ReadAllLines(_cesSamplePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            Assert.IsNotEmpty(lines);

            var instance = new BLSEconomicSurveysCes();
            foreach (var line in lines)
            {
                var result = instance.Reader(_cesConfig, line, DateTime.UtcNow, false) as BLSEconomicSurveysCes;
                Assert.IsNotNull(result);
                Assert.AreEqual(_cesConfig.Symbol, result.Symbol);
                Assert.AreNotEqual(default(DateTime), result.Time);
                Assert.Greater(result.EndTime, result.Time);
            }
        }

        [Test]
        public void CesCloneCopiesAllProperties()
        {
            var firstLine = File.ReadLines(_cesSamplePath).First();
            var instance = new BLSEconomicSurveysCes();
            var original = instance.Reader(_cesConfig, firstLine, DateTime.UtcNow, false) as BLSEconomicSurveysCes;
            var clone = original.Clone() as BLSEconomicSurveysCes;
            AssertAreEqual(original, clone);
        }

        // ==================== PPI Tests ====================

        [Test]
        public void PpiSampleDataFileExists()
        {
            Assert.IsTrue(File.Exists(_ppiSamplePath), $"Missing: {_ppiSamplePath}");
        }

        [Test]
        public void PpiReaderParsesAllSampleDataLines()
        {
            var lines = File.ReadAllLines(_ppiSamplePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            Assert.IsNotEmpty(lines);

            var instance = new BLSEconomicSurveysPpi();
            foreach (var line in lines)
            {
                var result = instance.Reader(_ppiConfig, line, DateTime.UtcNow, false) as BLSEconomicSurveysPpi;
                Assert.IsNotNull(result);
                Assert.AreEqual(_ppiConfig.Symbol, result.Symbol);
                Assert.AreNotEqual(default(DateTime), result.Time);
                Assert.Greater(result.EndTime, result.Time);
            }
        }

        [Test]
        public void PpiCloneCopiesAllProperties()
        {
            var firstLine = File.ReadLines(_ppiSamplePath).First();
            var instance = new BLSEconomicSurveysPpi();
            var original = instance.Reader(_ppiConfig, firstLine, DateTime.UtcNow, false) as BLSEconomicSurveysPpi;
            var clone = original.Clone() as BLSEconomicSurveysPpi;
            AssertAreEqual(original, clone);
        }

        // ==================== JOLTS Tests ====================

        [Test]
        public void JoltsSampleDataFileExists()
        {
            Assert.IsTrue(File.Exists(_joltsSamplePath), $"Missing: {_joltsSamplePath}");
        }

        [Test]
        public void JoltsReaderParsesAllSampleDataLines()
        {
            var lines = File.ReadAllLines(_joltsSamplePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            Assert.IsNotEmpty(lines);

            var instance = new BLSEconomicSurveysJolts();
            foreach (var line in lines)
            {
                var result = instance.Reader(_joltsConfig, line, DateTime.UtcNow, false) as BLSEconomicSurveysJolts;
                Assert.IsNotNull(result);
                Assert.AreEqual(_joltsConfig.Symbol, result.Symbol);
                Assert.AreNotEqual(default(DateTime), result.Time);
                Assert.Greater(result.EndTime, result.Time);
            }
        }

        [Test]
        public void JoltsCloneCopiesAllProperties()
        {
            var firstLine = File.ReadLines(_joltsSamplePath).First();
            var instance = new BLSEconomicSurveysJolts();
            var original = instance.Reader(_joltsConfig, firstLine, DateTime.UtcNow, false) as BLSEconomicSurveysJolts;
            var clone = original.Clone() as BLSEconomicSurveysJolts;
            AssertAreEqual(original, clone);
        }

        // ==================== Common Tests ====================

        [Test]
        public void ToStringReturnsNonEmpty()
        {
            var line = File.ReadLines(_cpiSamplePath).First();
            var instance = new BLSEconomicSurveysCpi();
            var result = instance.Reader(_cpiConfig, line, DateTime.UtcNow, false) as BLSEconomicSurveysCpi;
            Assert.IsNotNull(result.ToString());
            Assert.IsNotEmpty(result.ToString());
        }

        [Test]
        public void GetSourceReturnsLocalFile()
        {
            var instance = new BLSEconomicSurveysCpi();
            var source = instance.GetSource(_cpiConfig, DateTime.UtcNow, false);
            Assert.IsNotNull(source);
            Assert.AreEqual(SubscriptionTransportMedium.LocalFile, source.TransportMedium);
        }

        [Test]
        public void DefaultResolutionIsDaily()
        {
            Assert.AreEqual(Resolution.Daily, new BLSEconomicSurveysCpi().DefaultResolution());
            Assert.AreEqual(Resolution.Daily, new BLSEconomicSurveysCes().DefaultResolution());
            Assert.AreEqual(Resolution.Daily, new BLSEconomicSurveysPpi().DefaultResolution());
            Assert.AreEqual(Resolution.Daily, new BLSEconomicSurveysJolts().DefaultResolution());
        }

        [Test]
        public void RequiresMappingIsFalse()
        {
            Assert.IsFalse(new BLSEconomicSurveysCpi().RequiresMapping());
            Assert.IsFalse(new BLSEconomicSurveysCes().RequiresMapping());
            Assert.IsFalse(new BLSEconomicSurveysPpi().RequiresMapping());
            Assert.IsFalse(new BLSEconomicSurveysJolts().RequiresMapping());
        }

        [Test]
        public void IsSparseDataIsTrue()
        {
            Assert.IsTrue(new BLSEconomicSurveysCpi().IsSparseData());
            Assert.IsTrue(new BLSEconomicSurveysCes().IsSparseData());
            Assert.IsTrue(new BLSEconomicSurveysPpi().IsSparseData());
            Assert.IsTrue(new BLSEconomicSurveysJolts().IsSparseData());
        }

        /// <summary>
        /// Reflection-based property comparison for Clone verification.
        /// </summary>
        private static void AssertAreEqual(object expected, object actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.GetType(), actual.GetType());

            foreach (var prop in expected.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead) continue;
                var expectedValue = prop.GetValue(expected);
                var actualValue = prop.GetValue(actual);
                Assert.AreEqual(expectedValue, actualValue, $"Property {prop.Name} mismatch");
            }
        }
    }
}
