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
    /// BLS Consumer Price Index (CPI) data. Contains 15 non-seasonally-adjusted CPI series
    /// including headline CPI, core CPI, and major expenditure categories. CPI measures the
    /// average change over time in the prices paid by urban consumers for a market basket of
    /// consumer goods and services. Released monthly by the Bureau of Labor Statistics.
    /// </summary>
    public class BLSEconomicSurveysCpi : BaseData
    {
        /// <summary>
        /// End time of this data point — the actual release date/time when this CPI data was published.
        /// Overridden to decouple from Time (BaseData.EndTime setter overwrites Time).
        /// Release dates are scraped from BLS schedule pages at https://www.bls.gov/schedule/{year}/home.htm
        /// </summary>
        public override DateTime EndTime { get; set; }

        /// <summary>
        /// All items, U.S. city average, all urban consumers, NSA (CUUR0000SA0). Headline CPI index.
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? AllItems { get; set; }

        /// <summary>
        /// All items less food and energy (Core CPI), NSA (CUUR0000SA0L1E).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? CoreCpi { get; set; }

        /// <summary>
        /// Food, NSA (CUUR0000SAF1).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? Food { get; set; }

        /// <summary>
        /// Food at home, NSA (CUUR0000SAF11).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? FoodAtHome { get; set; }

        /// <summary>
        /// Food away from home, NSA (CUUR0000SEFV).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? FoodAwayFromHome { get; set; }

        /// <summary>
        /// Energy, NSA (CUUR0000SA0E).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? Energy { get; set; }

        /// <summary>
        /// Shelter, NSA (CUUR0000SAH1).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? Shelter { get; set; }

        /// <summary>
        /// Rent of primary residence, NSA (CUUR0000SEHA).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? RentOfPrimaryResidence { get; set; }

        /// <summary>
        /// Gasoline (all types), NSA (CUUR0000SETB01).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? Gasoline { get; set; }

        /// <summary>
        /// Medical care, NSA (CUUR0000SAM).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? MedicalCare { get; set; }

        /// <summary>
        /// Apparel, NSA (CUUR0000SAA).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? Apparel { get; set; }

        /// <summary>
        /// Education and communication, NSA (CUUR0000SAE).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? EducationAndCommunication { get; set; }

        /// <summary>
        /// New vehicles, NSA (CUUR0000SETA01).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? NewVehicles { get; set; }

        /// <summary>
        /// Used cars and trucks, NSA (CUUR0000SETA02).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? UsedCarsAndTrucks { get; set; }

        /// <summary>
        /// College tuition and fees, NSA (CUUR0000SEEB01).
        /// Base period: 1982-84=100.
        /// </summary>
        public decimal? CollegeTuitionAndFees { get; set; }

        /// <summary>
        /// Default constructor required for LEAN deserialization.
        /// </summary>
        public BLSEconomicSurveysCpi()
        {
        }

        /// <summary>
        /// Constructor that parses a CSV line into the data model.
        /// CSV format: time,endtime,allitems,corecpi,food,foodathome,foodawayfromhome,energy,shelter,rentofprimaryresidence,gasoline,medicalcare,apparel,educationandcommunication,newvehicles,usedcarsandtrucks,collegetuitionandfees
        /// </summary>
        /// <param name="line">CSV line to parse</param>
        public BLSEconomicSurveysCpi(string line)
        {
            var csv = line.Split(',');
            if (csv.Length < 3) return;

            Time = DateTime.ParseExact(csv[0], "yyyyMMdd", CultureInfo.InvariantCulture);
            EndTime = DateTime.ParseExact(csv[1].Trim(), "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);

            AllItems = csv[2].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture));
            CoreCpi = csv.Length > 3 ? csv[3].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            Food = csv.Length > 4 ? csv[4].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            FoodAtHome = csv.Length > 5 ? csv[5].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            FoodAwayFromHome = csv.Length > 6 ? csv[6].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            Energy = csv.Length > 7 ? csv[7].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            Shelter = csv.Length > 8 ? csv[8].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            RentOfPrimaryResidence = csv.Length > 9 ? csv[9].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            Gasoline = csv.Length > 10 ? csv[10].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            MedicalCare = csv.Length > 11 ? csv[11].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            Apparel = csv.Length > 12 ? csv[12].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            EducationAndCommunication = csv.Length > 13 ? csv[13].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            NewVehicles = csv.Length > 14 ? csv[14].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            UsedCarsAndTrucks = csv.Length > 15 ? csv[15].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;
            CollegeTuitionAndFees = csv.Length > 16 ? csv[16].IfNotNullOrEmpty<decimal?>(s => decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture)) : null;

            Value = AllItems ?? 0m;
        }

        /// <summary>
        /// Return the URL/path string source of the file. This will be converted to a stream.
        /// </summary>
        /// <param name="config">Configuration object</param>
        /// <param name="date">Date of this source file</param>
        /// <param name="isLiveMode">true if we're in live mode, false for backtesting</param>
        /// <returns>Subscription data source object pointing to the data location</returns>
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
        /// <param name="config">Subscription data config setup object</param>
        /// <param name="line">Line of the source document</param>
        /// <param name="date">Date of the requested data</param>
        /// <param name="isLiveMode">true if we're in live mode, false for backtesting</param>
        /// <returns>Instance of the class with parsed data</returns>
        public override BaseData Reader(
            SubscriptionDataConfig config,
            string line,
            DateTime date,
            bool isLiveMode)
        {
            return new BLSEconomicSurveysCpi(line) { Symbol = config.Symbol };
        }

        /// <summary>
        /// Indicates the data time zone for this data source. BLS releases are in Eastern Time.
        /// </summary>
        /// <returns>The DateTimeZone of this data source</returns>
        public override DateTimeZone DataTimeZone()
        {
            return TimeZones.NewYork;
        }

        /// <summary>
        /// Gets the supported resolutions for this data type.
        /// </summary>
        /// <returns>List of supported resolutions</returns>
        public override List<Resolution> SupportedResolutions()
        {
            return new List<Resolution> { Resolution.Daily };
        }

        /// <summary>
        /// Gets the default resolution for this data type.
        /// </summary>
        /// <returns>The default resolution</returns>
        public override Resolution DefaultResolution()
        {
            return Resolution.Daily;
        }

        /// <summary>
        /// CPI data is sparse — released monthly, so most days have no data.
        /// </summary>
        /// <returns>true because CPI is monthly data</returns>
        public override bool IsSparseData()
        {
            return true;
        }

        /// <summary>
        /// This is unlinked macro data, not tied to specific securities.
        /// </summary>
        /// <returns>false</returns>
        public override bool RequiresMapping()
        {
            return false;
        }

        /// <summary>
        /// Creates a copy of the instance.
        /// </summary>
        /// <returns>Clone of the instance</returns>
        public override BaseData Clone()
        {
            return new BLSEconomicSurveysCpi
            {
                Symbol = Symbol,
                Time = Time,
                EndTime = EndTime,
                AllItems = AllItems,
                CoreCpi = CoreCpi,
                Food = Food,
                FoodAtHome = FoodAtHome,
                FoodAwayFromHome = FoodAwayFromHome,
                Energy = Energy,
                Shelter = Shelter,
                RentOfPrimaryResidence = RentOfPrimaryResidence,
                Gasoline = Gasoline,
                MedicalCare = MedicalCare,
                Apparel = Apparel,
                EducationAndCommunication = EducationAndCommunication,
                NewVehicles = NewVehicles,
                UsedCarsAndTrucks = UsedCarsAndTrucks,
                CollegeTuitionAndFees = CollegeTuitionAndFees,
                Value = Value
            };
        }

        /// <summary>
        /// Returns a string representation for debugging.
        /// </summary>
        /// <returns>String representation of the data point</returns>
        public override string ToString()
        {
            return $"{Symbol} - Time: {Time:yyyy-MM-dd} EndTime: {EndTime:yyyy-MM-dd HH:mm} AllItems: {AllItems} CoreCpi: {CoreCpi}";
        }
    }
}
