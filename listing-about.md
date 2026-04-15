### Meta

- **Dataset name**: _Economic Surveys_
- **Vendor name**: _Bureau of Labor Statistics (BLS)_
- **Vendor Website**: *https://www.bls.gov*

### Introduction

The _Bureau of Labor Statistics_ tracks key U.S. macroeconomic indicators across four major surveys: Consumer Price Index (CPI), Current Employment Statistics (CES), Producer Price Index (PPI), and Job Openings and Labor Turnover Survey (JOLTS). The data covers 50 non-seasonally-adjusted series, starting in January 2000, and is delivered on a daily frequency (monthly releases with exact publication timestamps). This dataset is created by scraping BLS release schedules for accurate publication dates and fetching series data from the BLS API v2.

### About the Provider

The Bureau of Labor Statistics (BLS) was established in 1884 as a division of the Department of the Interior, with the goal of collecting and disseminating essential economic data about the U.S. labor market and price changes. BLS provides access to employment, inflation, compensation, and productivity data for researchers, policymakers, and financial professionals.

### Getting Started

Python:

```
self.add_data(BLSEconomicSurveysCpi, "CPI", Resolution.DAILY)
self.add_data(BLSEconomicSurveysCes, "CES", Resolution.DAILY)
self.add_data(BLSEconomicSurveysPpi, "PPI", Resolution.DAILY)
self.add_data(BLSEconomicSurveysJolts, "JOLTS", Resolution.DAILY)
```

C#:

```
AddData<BLSEconomicSurveysCpi>("CPI", Resolution.Daily);
AddData<BLSEconomicSurveysCes>("CES", Resolution.Daily);
AddData<BLSEconomicSurveysPpi>("PPI", Resolution.Daily);
AddData<BLSEconomicSurveysJolts>("JOLTS", Resolution.Daily);
```

### Data Summary

- **Start Date**: January 2000 (CPI/CES/PPI), November 2007 (JOLTS)
- **Asset Coverage**: 50 non-seasonally-adjusted series across 4 surveys
- **Resolution**: Daily
- **Data Density**: Monthly (one data point per survey per month)
- **Timezone**: Eastern Time (America/New_York)

### Example Applications

The BLS dataset enables researchers to accurately design strategies harnessing inflation, employment, and labor market data with precise point-in-time release timestamps. Examples include:

- Inflation surprise trading: compare CPI release values to consensus estimates and trade equity/bond positions based on deviations
- Employment momentum strategies: track month-over-month changes in nonfarm payrolls and sector employment to identify economic regime shifts
- Labor market tightness signals: use JOLTS job openings-to-quits ratio as a leading indicator of wage growth and Fed policy direction
- Cross-survey confirmation: combine CPI, PPI, and CES data to build composite economic health indicators

### Data Point Attributes

- _BLSEconomicSurveysCpi_ - Consumer Price Index (15 series): AllItems, CoreCpi, Food, FoodAtHome, FoodAwayFromHome, Energy, Shelter, RentOfPrimaryResidence, Gasoline, MedicalCare, Apparel, EducationAndCommunication, NewVehicles, UsedCarsAndTrucks, CollegeTuitionAndFees
- _BLSEconomicSurveysCes_ - Employment Situation (16 series): TotalNonfarm, TotalPrivate, AverageHourlyEarnings, AverageWeeklyHours, AverageWeeklyEarnings, ProductionHourlyEarnings, ProductionEmployees, Manufacturing, GoodsProducing, PrivateServiceProviding, Construction, RetailTrade, FinancialActivities, EducationAndHealthServices, LeisureAndHospitality, MiningAndLogging
- _BLSEconomicSurveysPpi_ - Producer Price Index (11 series): FinalDemand, CorePpi, FinalDemandLessFoodEnergyTrade, FinalDemandGoods, FinalDemandServices, FinalDemandConstruction, AllCommodities, FarmProducts, ProcessedFoodsAndFeeds, CrudePetroleum, FinalDemandGoodsLessFoods
- _BLSEconomicSurveysJolts_ - Job Openings and Labor Turnover (8 series): JobOpenings, JobOpeningsRate, Hires, HiresRate, Quits, QuitsRate, TotalSeparations, LayoffsAndDischarges

### Supported Surveys

| Ticker | Survey                          | Schedule Name        | Series Count | Release Time (ET) | Start Date |
| ------ | ------------------------------- | -------------------- | ------------ | ----------------- | ---------- |
| CPI    | Consumer Price Index            | Consumer Price Index | 15           | 08:30 AM          | Jan 2000   |
| CES    | Current Employment Statistics   | Employment Situation | 16           | 08:30 AM          | Jan 2000   |
| PPI    | Producer Price Index            | Producer Price Index | 11           | 08:30 AM          | Jan 2000   |
| JOLTS  | Job Openings and Labor Turnover | JOLTS                | 8            | 10:00 AM          | Nov 2007   |
