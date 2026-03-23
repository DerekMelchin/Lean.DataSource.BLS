# region imports
from AlgorithmImports import *
from QuantConnect.DataSource import *
# endregion


class BLSEconomicSurveysAlgorithm(QCAlgorithm):
    """
    Demonstration algorithm showing how to use BLS Economic Surveys custom datasets
    (CPI, CES, PPI, JOLTS) for signal generation and trading.
    """

    def initialize(self):
        self.set_start_date(2000, 1, 1)
        self.set_end_date(2002, 6, 1)
        self.set_cash(100000)

        # Add a tradeable equity for order execution
        self._spy_symbol = self.add_equity("SPY", Resolution.DAILY).symbol

        # Subscribe to BLS economic surveys (unlinked custom data)
        self._cpi_symbol = self.add_data(BLSEconomicSurveysCpi, "CPI", Resolution.DAILY).symbol
        self._ces_symbol = self.add_data(BLSEconomicSurveysCes, "CES", Resolution.DAILY).symbol
        self._ppi_symbol = self.add_data(BLSEconomicSurveysPpi, "PPI", Resolution.DAILY).symbol

    def on_data(self, slice: Slice):
        # Check for CPI data
        if slice.contains_key(self._cpi_symbol):
            cpi = slice.get(BLSEconomicSurveysCpi, self._cpi_symbol)
            self.log(f"{self.time} - CPI AllItems: {cpi.all_items}, CoreCpi: {cpi.core_cpi}, Energy: {cpi.energy}")

            # Simple signal: if energy CPI is rising faster than core, reduce equity exposure
            if cpi.energy is not None and cpi.core_cpi is not None and cpi.energy > cpi.core_cpi * 1.5:
                if self.portfolio[self._spy_symbol].invested:
                    self.set_holdings(self._spy_symbol, 0.5)

        # Check for employment data
        if slice.contains_key(self._ces_symbol):
            ces = slice.get(BLSEconomicSurveysCes, self._ces_symbol)
            self.log(f"{self.time} - CES TotalNonfarm: {ces.total_nonfarm}, AvgHourlyEarnings: {ces.average_hourly_earnings}")

            # Simple signal: go long when nonfarm payrolls are strong
            if ces.total_nonfarm is not None and ces.total_nonfarm > 130000 and not self.portfolio[self._spy_symbol].invested:
                self.set_holdings(self._spy_symbol, 1)

        # Check for PPI data
        if slice.contains_key(self._ppi_symbol):
            ppi = slice.get(BLSEconomicSurveysPpi, self._ppi_symbol)
            self.log(f"{self.time} - PPI AllCommodities: {ppi.all_commodities}, FarmProducts: {ppi.farm_products}")
