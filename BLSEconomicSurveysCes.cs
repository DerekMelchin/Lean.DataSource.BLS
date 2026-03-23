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
    /// BLS Current Employment Statistics (CES) data from the Employment Situation report.
    /// Contains 16 non-seasonally-adjusted series covering nonfarm payrolls, average hourly
    /// earnings, average weekly hours, and employment across major industry sectors.
    /// The Employment Situation report is the single most market-moving economic release.
    /// Released monthly (typically first Friday) by the Bureau of Labor Statistics.
    /// </summary>
    public class BLSEconomicSurveysCes : BaseData
    {
        /// <summary>
        /// End time of this data point — the actual release date/time when this employment data was published.
        /// Overridden to decouple from Time (BaseData.EndTime setter overwrites Time).
        /// Release dates are scraped from BLS schedule pages at https://www.bls.gov/schedule/{year}/home.htm
        /// </summary>
        public override DateTime EndTime { get; set; }

        /// <summary>
        /// Total nonfarm, all employees, NSA (CEU0000000001). Thousands.
        /// </summary>
        public decimal? TotalNonfarm { get; set; }

        /// <summary>
        /// Total private, all employees, NSA (CEU0500000001). Thousands.
        /// </summary>
        public decimal? TotalPrivate { get; set; }

        /// <summary>
        /// Average hourly earnings, all employees, total private, NSA (CEU0500000003). Dollars.
        /// </summary>
        public decimal? AverageHourlyEarnings { get; set; }

        /// <summary>
        /// Average weekly hours, all employees, total private, NSA (CEU0500000002). Hours.
        /// </summary>
        public decimal? AverageWeeklyHours { get; set; }

        /// <summary>
        /// Average weekly earnings, all employees, total private, NSA (CEU0500000011). Dollars.
        /// </summary>
        public decimal? AverageWeeklyEarnings { get; set; }

        /// <summary>
        /// Average hourly earnings, production and nonsupervisory, total private, NSA (CEU0500000008). Dollars.
        /// </summary>
        public decimal? ProductionHourlyEarnings { get; set; }

        /// <summary>
        /// Production and nonsupervisory employees, total private, NSA (CEU0500000006). Thousands.
        /// </summary>
        public decimal? ProductionEmployees { get; set; }

        /// <summary>
        /// Manufacturing, all employees, NSA (CEU3000000001). Thousands.
        /// </summary>
        public decimal? Manufacturing { get; set; }

        /// <summary>
        /// Goods-producing, all employees, NSA (CEU0600000001). Thousands.
        /// </summary>
        public decimal? GoodsProducing { get; set; }

        /// <summary>
        /// Private service-providing, all employees, NSA (CEU0800000001). Thousands.
        /// </summary>
        public decimal? PrivateServiceProviding { get; set; }

        /// <summary>
        /// Construction, all employees, NSA (CEU2000000001). Thousands.
        /// </summary>
        public decimal? Construction { get; set; }

        /// <summary>
        /// Retail trade, all employees, NSA (CEU4200000001). Thousands.
        /// </summary>
        public decimal? RetailTrade { get; set; }

        /// <summary>
        /// Financial activities, all employees, NSA (CEU5500000001). Thousands.
        /// </summary>
        public decimal? FinancialActivities { get; set; }

        /// <summary>
        /// Education and health services, all employees, NSA (CEU6500000001). Thousands.
        /// </summary>
        public decimal? EducationAndHealthServices { get; set; }

        /// <summary>
        /// Leisure and hospitality, all employees, NSA (CEU7000000001). Thousands.
        /// </summary>
        public decimal? LeisureAndHospitality { get; set; }

        /// <summary>
        /// Mining and logging, all employees, NSA (CEU1000000001). Thousands.
        /// </summary>
        public decimal? MiningAndLogging { get; set; }

        /// <summary>
        /// Default constructor required for LEAN deserialization.
        /// </summary>
        public BLSEconomicSurveysCes()
        {
        }

        /// <summary>
        /// Constructor that parses a CSV line into the data model.
        /// CSV format: time,endtime,totalnonfarm,totalprivate,avghourlyrearnings,...,miningandlogging
        /// </summary>
        /// <param name="line">CSV line to parse</param>
        public BLSEconomicSurveysCes(string line)
        {
            var csv = line.Split(',');
            if (csv.Length < 3) return;

            Time = DateTime.ParseExact(csv[0], "yyyyMMdd", CultureInfo.InvariantCulture);
            EndTime = DateTime.ParseExact(csv[1].Trim(), "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);

            TotalNonfarm = csv[2].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture));
            TotalPrivate = csv.Length > 3 ? csv[3].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            AverageHourlyEarnings = csv.Length > 4 ? csv[4].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            AverageWeeklyHours = csv.Length > 5 ? csv[5].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            AverageWeeklyEarnings = csv.Length > 6 ? csv[6].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            ProductionHourlyEarnings = csv.Length > 7 ? csv[7].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            ProductionEmployees = csv.Length > 8 ? csv[8].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            Manufacturing = csv.Length > 9 ? csv[9].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            GoodsProducing = csv.Length > 10 ? csv[10].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            PrivateServiceProviding = csv.Length > 11 ? csv[11].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            Construction = csv.Length > 12 ? csv[12].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            RetailTrade = csv.Length > 13 ? csv[13].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            FinancialActivities = csv.Length > 14 ? csv[14].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            EducationAndHealthServices = csv.Length > 15 ? csv[15].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            LeisureAndHospitality = csv.Length > 16 ? csv[16].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            MiningAndLogging = csv.Length > 17 ? csv[17].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;

            Value = TotalNonfarm ?? 0m;
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
            return new BLSEconomicSurveysCes(line) { Symbol = config.Symbol };
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
        /// CES data is sparse — released monthly, so most days have no data.
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
            return new BLSEconomicSurveysCes
            {
                Symbol = Symbol,
                Time = Time,
                EndTime = EndTime,
                TotalNonfarm = TotalNonfarm,
                TotalPrivate = TotalPrivate,
                AverageHourlyEarnings = AverageHourlyEarnings,
                AverageWeeklyHours = AverageWeeklyHours,
                AverageWeeklyEarnings = AverageWeeklyEarnings,
                ProductionHourlyEarnings = ProductionHourlyEarnings,
                ProductionEmployees = ProductionEmployees,
                Manufacturing = Manufacturing,
                GoodsProducing = GoodsProducing,
                PrivateServiceProviding = PrivateServiceProviding,
                Construction = Construction,
                RetailTrade = RetailTrade,
                FinancialActivities = FinancialActivities,
                EducationAndHealthServices = EducationAndHealthServices,
                LeisureAndHospitality = LeisureAndHospitality,
                MiningAndLogging = MiningAndLogging,
                Value = Value
            };
        }

        /// <summary>
        /// Returns a string representation for debugging.
        /// </summary>
        public override string ToString()
        {
            return $"{Symbol} - Time: {Time:yyyy-MM-dd} EndTime: {EndTime:yyyy-MM-dd HH:mm} TotalNonfarm: {TotalNonfarm} AverageHourlyEarnings: {AverageHourlyEarnings}";
        }
    }
}
