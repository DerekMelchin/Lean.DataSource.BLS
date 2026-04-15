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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NodaTime;
using QuantConnect.Data;
using QuantConnect.Util;

namespace QuantConnect.DataSource
{
    /// <summary>
    /// BLS Job Openings and Labor Turnover Survey (JOLTS) data. Contains 8 non-seasonally-adjusted
    /// series covering job openings, hires, quits, total separations, and layoffs for total nonfarm.
    /// JOLTS is closely watched by the Federal Reserve as a gauge of labor market tightness.
    /// The quit rate is considered a leading indicator of wage growth. Released monthly
    /// (typically with a 2-month lag) by the Bureau of Labor Statistics.
    /// </summary>
    public class BLSEconomicSurveysJolts : BaseData
    {
        /// <summary>
        /// End time of this data point — the actual release date/time when this JOLTS data was published.
        /// Overridden to decouple from Time (BaseData.EndTime setter overwrites Time).
        /// Release dates are scraped from BLS schedule pages at https://www.bls.gov/schedule/{year}/home.htm
        /// </summary>
        public override DateTime EndTime { get; set; }

        /// <summary>
        /// Total nonfarm job openings level, NSA (JTU000000000000000JOL). Thousands.
        /// </summary>
        public decimal? JobOpenings { get; set; }

        /// <summary>
        /// Total nonfarm job openings rate, NSA (JTU000000000000000JOR). Percent.
        /// </summary>
        public decimal? JobOpeningsRate { get; set; }

        /// <summary>
        /// Total nonfarm hires level, NSA (JTU000000000000000HIL). Thousands.
        /// </summary>
        public decimal? Hires { get; set; }

        /// <summary>
        /// Total nonfarm hires rate, NSA (JTU000000000000000HIR). Percent.
        /// </summary>
        public decimal? HiresRate { get; set; }

        /// <summary>
        /// Total nonfarm quits level, NSA (JTU000000000000000QUL). Thousands.
        /// </summary>
        public decimal? Quits { get; set; }

        /// <summary>
        /// Total nonfarm quits rate, NSA (JTU000000000000000QUR). Percent.
        /// </summary>
        public decimal? QuitsRate { get; set; }

        /// <summary>
        /// Total nonfarm total separations level, NSA (JTU000000000000000TSL). Thousands.
        /// </summary>
        public decimal? TotalSeparations { get; set; }

        /// <summary>
        /// Total nonfarm layoffs and discharges level, NSA (JTU000000000000000LDL). Thousands.
        /// </summary>
        public decimal? LayoffsAndDischarges { get; set; }

        /// <summary>
        /// Default constructor required for LEAN deserialization.
        /// </summary>
        public BLSEconomicSurveysJolts()
        {
        }

        /// <summary>
        /// Constructor that parses a CSV line into the data model.
        /// </summary>
        /// <param name="line">CSV line to parse</param>
        public BLSEconomicSurveysJolts(string line)
        {
            var csv = line.Split(',');
            if (csv.Length < 3) return;

            Time = DateTime.ParseExact(csv[0], "yyyyMMdd", CultureInfo.InvariantCulture);
            EndTime = DateTime.ParseExact(csv[1].Trim(), "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);

            JobOpenings = csv[2].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture));
            JobOpeningsRate = csv.Length > 3 ? csv[3].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            Hires = csv.Length > 4 ? csv[4].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            HiresRate = csv.Length > 5 ? csv[5].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            Quits = csv.Length > 6 ? csv[6].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            QuitsRate = csv.Length > 7 ? csv[7].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            TotalSeparations = csv.Length > 8 ? csv[8].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            LayoffsAndDischarges = csv.Length > 9 ? csv[9].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;

            Value = JobOpenings ?? 0m;
        }

        /// <summary>
        /// Return the URL/path string source of the file.
        /// </summary>
        public override SubscriptionDataSource GetSource(
            SubscriptionDataConfig config,
            DateTime date,
            bool isLiveMode)
        {
            return new SubscriptionDataSource(
                Path.Combine(
                    Globals.DataFolder,
                    "alternative",
                    "bls",
                    "economicsurveys",
                    $"{config.Symbol.Value.ToLowerInvariant()}.csv"
                ),
                SubscriptionTransportMedium.LocalFile,
                FileFormat.Csv
            );
        }

        /// <summary>
        /// Parses the data from the line provided and loads it into LEAN.
        /// </summary>
        public override BaseData Reader(
            SubscriptionDataConfig config,
            string line,
            DateTime date,
            bool isLiveMode)
        {
            return new BLSEconomicSurveysJolts(line) { Symbol = config.Symbol };
        }

        /// <summary>
        /// BLS releases are in Eastern Time. JOLTS is released at 10:00 AM ET.
        /// </summary>
        public override DateTimeZone DataTimeZone()
        {
            return TimeZones.NewYork;
        }

        /// <summary>
        /// Gets the supported resolutions for this data type.
        /// </summary>
        public override List<Resolution> SupportedResolutions()
        {
            return new List<Resolution> { Resolution.Daily };
        }

        /// <summary>
        /// Gets the default resolution for this data type.
        /// </summary>
        public override Resolution DefaultResolution()
        {
            return Resolution.Daily;
        }

        /// <summary>
        /// JOLTS data is sparse — released monthly, so most days have no data.
        /// </summary>
        public override bool IsSparseData()
        {
            return true;
        }

        /// <summary>
        /// This is unlinked macro data, not tied to specific securities.
        /// </summary>
        public override bool RequiresMapping()
        {
            return false;
        }

        /// <summary>
        /// Creates a copy of the instance.
        /// </summary>
        public override BaseData Clone()
        {
            return new BLSEconomicSurveysJolts
            {
                Symbol = Symbol,
                Time = Time,
                EndTime = EndTime,
                JobOpenings = JobOpenings,
                JobOpeningsRate = JobOpeningsRate,
                Hires = Hires,
                HiresRate = HiresRate,
                Quits = Quits,
                QuitsRate = QuitsRate,
                TotalSeparations = TotalSeparations,
                LayoffsAndDischarges = LayoffsAndDischarges,
                Value = Value
            };
        }

        /// <summary>
        /// Returns a string representation for debugging.
        /// </summary>
        public override string ToString()
        {
            return $"{Symbol} - Time: {Time:yyyy-MM-dd} EndTime: {EndTime:yyyy-MM-dd HH:mm} JobOpenings: {JobOpenings} Quits: {Quits}";
        }
    }
}
