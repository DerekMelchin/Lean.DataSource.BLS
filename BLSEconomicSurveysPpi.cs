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
    /// BLS Producer Price Index (PPI) data. Contains 11 non-seasonally-adjusted PPI series
    /// including the Final Demand framework (headline PPI), core PPI, and major commodity groups.
    /// PPI measures the average change over time in the selling prices received by domestic producers.
    /// It is a leading indicator of consumer inflation. Released monthly by the Bureau of Labor Statistics.
    /// </summary>
    public class BLSEconomicSurveysPpi : BaseData
    {
        /// <summary>
        /// End time of this data point — the actual release date/time when this PPI data was published.
        /// Overridden to decouple from Time (BaseData.EndTime setter overwrites Time).
        /// Release dates are scraped from BLS schedule pages at https://www.bls.gov/schedule/{year}/home.htm
        /// </summary>
        public override DateTime EndTime { get; set; }

        /// <summary>
        /// Final demand, NSA (WPUFD4). Headline PPI index.
        /// Base date: Nov 2009=100.
        /// </summary>
        public decimal? FinalDemand { get; set; }

        /// <summary>
        /// Final demand less foods and energy (Core PPI), NSA (WPUFD49104).
        /// Base date: Oct 2004=100.
        /// </summary>
        public decimal? CorePpi { get; set; }

        /// <summary>
        /// Final demand less foods, energy, and trade services, NSA (WPUFD49116).
        /// Base date: Aug 2013=100.
        /// </summary>
        public decimal? FinalDemandLessFoodEnergyTrade { get; set; }

        /// <summary>
        /// Final demand goods, NSA (WPUFD41).
        /// Base date: Nov 2009=100.
        /// </summary>
        public decimal? FinalDemandGoods { get; set; }

        /// <summary>
        /// Final demand services, NSA (WPUFD42).
        /// Base date: Nov 2009=100.
        /// </summary>
        public decimal? FinalDemandServices { get; set; }

        /// <summary>
        /// Final demand construction, NSA (WPUFD43).
        /// Base date: Nov 2009=100.
        /// </summary>
        public decimal? FinalDemandConstruction { get; set; }

        /// <summary>
        /// All commodities, NSA (WPU00000000).
        /// Base date: 1982=100.
        /// </summary>
        public decimal? AllCommodities { get; set; }

        /// <summary>
        /// Farm products, NSA (WPU01).
        /// Base date: 1982=100.
        /// </summary>
        public decimal? FarmProducts { get; set; }

        /// <summary>
        /// Processed foods and feeds, NSA (WPU02).
        /// Base date: 1982=100.
        /// </summary>
        public decimal? ProcessedFoodsAndFeeds { get; set; }

        /// <summary>
        /// Crude petroleum (domestic), NSA (WPU0571).
        /// Base date: 1982=100.
        /// </summary>
        public decimal? CrudePetroleum { get; set; }

        /// <summary>
        /// Final demand goods less foods, NSA (WPUFD49112).
        /// </summary>
        public decimal? FinalDemandGoodsLessFoods { get; set; }

        /// <summary>
        /// Default constructor required for LEAN deserialization.
        /// </summary>
        public BLSEconomicSurveysPpi()
        {
        }

        /// <summary>
        /// Constructor that parses a CSV line into the data model.
        /// </summary>
        /// <param name="line">CSV line to parse</param>
        public BLSEconomicSurveysPpi(string line)
        {
            var csv = line.Split(',');
            if (csv.Length < 3) return;

            Time = DateTime.ParseExact(csv[0], "yyyyMMdd", CultureInfo.InvariantCulture);
            EndTime = DateTime.ParseExact(csv[1].Trim(), "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);

            FinalDemand = csv[2].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture));
            CorePpi = csv.Length > 3 ? csv[3].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            FinalDemandLessFoodEnergyTrade = csv.Length > 4 ? csv[4].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            FinalDemandGoods = csv.Length > 5 ? csv[5].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            FinalDemandServices = csv.Length > 6 ? csv[6].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            FinalDemandConstruction = csv.Length > 7 ? csv[7].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            AllCommodities = csv.Length > 8 ? csv[8].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            FarmProducts = csv.Length > 9 ? csv[9].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            ProcessedFoodsAndFeeds = csv.Length > 10 ? csv[10].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            CrudePetroleum = csv.Length > 11 ? csv[11].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            FinalDemandGoodsLessFoods = csv.Length > 12 ? csv[12].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;

            Value = FinalDemand ?? 0m;
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
            return new BLSEconomicSurveysPpi(line) { Symbol = config.Symbol };
        }

        /// <summary>
        /// BLS releases are in Eastern Time.
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
        /// PPI data is sparse — released monthly, so most days have no data.
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
            return new BLSEconomicSurveysPpi
            {
                Symbol = Symbol,
                Time = Time,
                EndTime = EndTime,
                FinalDemand = FinalDemand,
                CorePpi = CorePpi,
                FinalDemandLessFoodEnergyTrade = FinalDemandLessFoodEnergyTrade,
                FinalDemandGoods = FinalDemandGoods,
                FinalDemandServices = FinalDemandServices,
                FinalDemandConstruction = FinalDemandConstruction,
                AllCommodities = AllCommodities,
                FarmProducts = FarmProducts,
                ProcessedFoodsAndFeeds = ProcessedFoodsAndFeeds,
                CrudePetroleum = CrudePetroleum,
                FinalDemandGoodsLessFoods = FinalDemandGoodsLessFoods,
                Value = Value
            };
        }

        /// <summary>
        /// Returns a string representation for debugging.
        /// </summary>
        public override string ToString()
        {
            return $"{Symbol} - Time: {Time:yyyy-MM-dd} EndTime: {EndTime:yyyy-MM-dd HH:mm} FinalDemand: {FinalDemand} CorePpi: {CorePpi}";
        }
    }
}
