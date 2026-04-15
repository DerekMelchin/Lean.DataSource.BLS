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

using QuantConnect.Data;
using QuantConnect.DataSource;

namespace QuantConnect.Algorithm.CSharp
{
    /// <summary>
    /// Demonstration algorithm showing how to use BLS Economic Surveys custom datasets
    /// (CPI, CES, PPI, JOLTS) for signal generation and trading.
    /// </summary>
    public class BLSEconomicSurveysAlgorithm : QCAlgorithm
    {
        private Symbol _cpiSymbol;
        private Symbol _cesSymbol;
        private Symbol _ppiSymbol;
        private Symbol _spySymbol;

        /// <summary>
        /// Initializes the algorithm with custom data subscriptions.
        /// </summary>
        public override void Initialize()
        {
            SetStartDate(2000, 1, 1);
            SetEndDate(2002, 6, 1);
            SetCash(100000);

            // Add a tradeable equity for order execution
            _spySymbol = AddEquity("SPY", Resolution.Daily).Symbol;

            // Subscribe to BLS economic surveys (unlinked custom data)
            _cpiSymbol = AddData<BLSEconomicSurveysCpi>("CPI", Resolution.Daily).Symbol;
            _cesSymbol = AddData<BLSEconomicSurveysCes>("CES", Resolution.Daily).Symbol;
            _ppiSymbol = AddData<BLSEconomicSurveysPpi>("PPI", Resolution.Daily).Symbol;
        }

        /// <summary>
        /// Handles data events. Uses CPI and employment data to generate trading signals.
        /// </summary>
        /// <param name="slice">The current data slice</param>
        public override void OnData(Slice slice)
        {
            // Check for CPI data
            if (slice.ContainsKey(_cpiSymbol))
            {
                var cpi = slice.Get<BLSEconomicSurveysCpi>(_cpiSymbol);
                Log($"{Time} - CPI AllItems: {cpi.AllItems}, CoreCpi: {cpi.CoreCpi}, Energy: {cpi.Energy}");

                // Simple signal: if energy CPI is rising faster than core, reduce equity exposure
                if (cpi.Energy.HasValue && cpi.CoreCpi.HasValue && cpi.Energy > cpi.CoreCpi * 1.5m)
                {
                    if (Portfolio[_spySymbol].Invested)
                    {
                        SetHoldings(_spySymbol, 0.5);
                    }
                }
            }

            // Check for employment data
            if (slice.ContainsKey(_cesSymbol))
            {
                var ces = slice.Get<BLSEconomicSurveysCes>(_cesSymbol);
                Log($"{Time} - CES TotalNonfarm: {ces.TotalNonfarm}, AvgHourlyEarnings: {ces.AverageHourlyEarnings}");

                // Simple signal: go long when nonfarm payrolls are strong
                if (ces.TotalNonfarm.HasValue && ces.TotalNonfarm > 130000m && !Portfolio[_spySymbol].Invested)
                {
                    SetHoldings(_spySymbol, 1);
                }
            }

            // Check for PPI data
            if (slice.ContainsKey(_ppiSymbol))
            {
                var ppi = slice.Get<BLSEconomicSurveysPpi>(_ppiSymbol);
                Log($"{Time} - PPI AllCommodities: {ppi.AllCommodities}, FarmProducts: {ppi.FarmProducts}");
            }
        }
    }
}
