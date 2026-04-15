# BLS Economic Surveys - Validation Report

Generated: 2026-03-23 11:14


## CPI

### Structural Checks

- Column count: 17 (expected 17) - PASS
- Row count: 313 - PASS (not empty)
- No completely empty rows - PASS
- All time values parse correctly - PASS
- Chronologically ordered - PASS
- No duplicate time values - PASS
- All endtime values parse correctly - PASS
- EndTime > Time for all rows - PASS

### Per-Column Coverage

- AllItems: 313/313 (100%) - FULL
- CoreCpi: 313/313 (100%) - FULL
- Food: 313/313 (100%) - FULL
- FoodAtHome: 313/313 (100%) - FULL
- FoodAwayFromHome: 313/313 (100%) - FULL
- Energy: 313/313 (100%) - FULL
- Shelter: 313/313 (100%) - FULL
- RentOfPrimaryResidence: 313/313 (100%) - FULL
- Gasoline: 313/313 (100%) - FULL
- MedicalCare: 313/313 (100%) - FULL
- Apparel: 313/313 (100%) - FULL
- EducationAndCommunication: 313/313 (100%) - FULL
- NewVehicles: 313/313 (100%) - FULL
- UsedCarsAndTrucks: 313/313 (100%) - FULL
- CollegeTuitionAndFees: 313/313 (100%) - FULL

### Coverage and Density

- Date range: 2000-01-01 to 2026-02-01
- Average data points per year: 12.0
- Density check PASS (>= 10 points/year)

### Statistical Checks

- **AllItems**: min=168.800, max=326.785, mean=233.771, std=41.706
- **CoreCpi**: min=178.800, max=333.242, mean=238.981, std=41.067
- **Food**: min=166.100, max=346.564, mean=237.497, std=48.446
- **FoodAtHome**: min=166.300, max=318.898, mean=229.707, std=40.825
- **FoodAwayFromHome**: min=167.200, max=391.706, mean=250.561, std=60.571
- **Energy**: min=111.000, max=340.917, mean=210.663, std=50.340
- **Shelter**: min=190.100, max=422.776, mean=278.155, std=61.080
- **RentOfPrimaryResidence**: min=181.100, max=442.157, mean=282.285, std=70.610
- **Gasoline**: min=95.400, max=430.142, mean=231.420, std=67.559
- **MedicalCare**: min=255.500, max=592.593, mean=419.552, std=95.909
- **Apparel**: min=113.500, max=136.132, mean=124.219, std=4.984
- **EducationAndCommunication**: min=101.500, max=147.928, mean=129.280, std=13.683
- **NewVehicles**: min=132.264, max=179.750, mean=147.933, std=13.586
- **UsedCarsAndTrucks**: min=121.061, max=213.683, mean=152.198, std=20.398
  Outliers (>3 std): 1 values
    Row 270: 20220701 = 213.683
- **CollegeTuitionAndFees**: min=326.000, max=968.168, mean=681.919, std=198.929

### Contextual Validation

- COVID check (Mar 2020): Energy=199.573
- 2008 crisis (Dec 2008): Gasoline=146.102
```
Most recent CPI: time=20260201, AllItems=326.785
```

### Sample Rows

```
First: 20000101,20000218 08:30,168.8,178.8,166.1,166.3,167.2,112.5,190.1,181.1,111.9,255.5,126.8,102.7,143.3,153.9,326.0
Middle: 20130101,20130221 08:30,230.280,231.612,236.341,234.240,240.713,234.624,260.039,264.700,286.417,420.687,124.687,135.225,145.871,145.260,722.477
Last: 20260201,20260311 08:30,326.785,333.242,346.564,318.898,391.706,277.179,422.776,442.157,263.378,592.593,136.132,146.533,178.841,175.559,967.536
```

## CES

### Structural Checks

- Column count: 18 (expected 18) - PASS
- Row count: 313 - PASS (not empty)
- No completely empty rows - PASS
- All time values parse correctly - PASS
- Chronologically ordered - PASS
- No duplicate time values - PASS
- All endtime values parse correctly - PASS
- EndTime > Time for all rows - PASS

### Per-Column Coverage

- TotalNonfarm: 313/313 (100%) - FULL
- TotalPrivate: 313/313 (100%) - FULL
- AverageHourlyEarnings: 313/313 (100%) - FULL
- AverageWeeklyHours: 313/313 (100%) - FULL
- AverageWeeklyEarnings: 313/313 (100%) - FULL
- ProductionHourlyEarnings: 313/313 (100%) - FULL
- ProductionEmployees: 313/313 (100%) - FULL
- Manufacturing: 313/313 (100%) - FULL
- GoodsProducing: 313/313 (100%) - FULL
- PrivateServiceProviding: 313/313 (100%) - FULL
- Construction: 313/313 (100%) - FULL
- RetailTrade: 313/313 (100%) - FULL
- FinancialActivities: 313/313 (100%) - FULL
- EducationAndHealthServices: 313/313 (100%) - FULL
- LeisureAndHospitality: 313/313 (100%) - FULL
- MiningAndLogging: 313/313 (100%) - FULL

### Coverage and Density

- Date range: 2000-01-01 to 2026-02-01
- Average data points per year: 12.0
- Density check PASS (>= 10 points/year)

### Statistical Checks

- **TotalNonfarm**: min=127804.000, max=159571.000, mean=140472.089, std=9191.926
- **TotalPrivate**: min=105428.000, max=135901.000, mean=118337.808, std=8773.359
- **AverageHourlyEarnings**: min=20.040, max=37.590, mean=26.697, std=4.782
- **AverageWeeklyHours**: min=33.500, max=35.200, mean=34.390, std=0.307
- **AverageWeeklyEarnings**: min=682.380, max=1296.860, mean=918.347, std=165.622
- **ProductionHourlyEarnings**: min=13.820, max=32.190, mean=20.799, std=4.886
- **ProductionEmployees**: min=86440.000, max=110876.000, mean=96926.390, std=7173.029
- **Manufacturing**: min=11326.000, max=17403.000, mean=13171.879, std=1439.390
- **GoodsProducing**: min=17074.000, max=25091.000, mean=20798.422, std=1766.710
- **PrivateServiceProviding**: min=84422.000, max=114311.000, mean=97539.387, std=8788.487
- **Construction**: min=5046.000, max=8476.000, mean=6942.626, std=829.249
- **RetailTrade**: min=13074.700, max=16338.200, mean=15224.997, std=465.368
  Outliers (>3 std): 2 values
    Row 243: 20200401 = 13074.700
    Row 244: 20200501 = 13547.800
- **FinancialActivities**: min=7629.000, max=9278.000, mean=8314.617, std=478.092
- **EducationAndHealthServices**: min=14954.000, max=27830.000, mean=20968.524, std=3333.186
- **LeisureAndHospitality**: min=8601.000, max=17599.000, mean=14182.556, std=1770.807
  Outliers (>3 std): 1 values
    Row 243: 20200401 = 8601.000
- **MiningAndLogging**: min=531.000, max=914.000, mean=683.917, std=96.601

### Contextual Validation

- COVID check (Apr 2020): TotalNonfarm=130252 (should drop ~20M)
```
Most recent CES: time=20260201, TotalNonfarm=157286
```

### Sample Rows

```
First: 20000101,20000204 08:30,128993,108502,nan,nan,nan,13.83,88090,17179,24080,84422,6322,15102.3,7719,14995,11056,579
Middle: 20130101,20130201 08:30,133062,111330,23.87,34.0,811.58,20.04,91757,11859,18050,93280,5353,14811.2,7782,20798,13324,838
Last: 20260201,20260306 08:30,157286,133705,37.59,34.5,1296.86,32.19,108849,12526,21131,112574,8011,15273.2,9117,27820,16328,594
```

## PPI

### Structural Checks

- Column count: 13 (expected 13) - PASS
- Row count: 313 - PASS (not empty)
- No completely empty rows - PASS
- All time values parse correctly - PASS
- Chronologically ordered - PASS
- No duplicate time values - PASS
- All endtime values parse correctly - PASS
- EndTime > Time for all rows - PASS

### Per-Column Coverage

- FinalDemand: 313/313 (100%) - FULL
- CorePpi: 313/313 (100%) - FULL
- FinalDemandLessFoodEnergyTrade: 313/313 (100%) - FULL
- FinalDemandGoods: 313/313 (100%) - FULL
- FinalDemandServices: 313/313 (100%) - FULL
- FinalDemandConstruction: 313/313 (100%) - FULL
- AllCommodities: 313/313 (100%) - FULL
- FarmProducts: 313/313 (100%) - FULL
- ProcessedFoodsAndFeeds: 313/313 (100%) - FULL
- CrudePetroleum: 313/313 (100%) - FULL
- FinalDemandGoodsLessFoods: 313/313 (100%) - FULL

### Coverage and Density

- Date range: 2000-01-01 to 2026-02-01
- Average data points per year: 12.0
- Density check PASS (>= 10 points/year)

### Statistical Checks

- **FinalDemand**: min=99.900, max=153.186, mean=120.124, std=15.074
- **CorePpi**: min=100.000, max=152.168, mean=118.964, std=14.899
- **FinalDemandLessFoodEnergyTrade**: min=99.900, max=140.165, mean=114.760, std=12.162
- **FinalDemandGoods**: min=100.000, max=149.694, mean=120.278, std=14.788
- **FinalDemandServices**: min=99.800, max=153.651, mean=119.540, std=15.061
- **FinalDemandConstruction**: min=99.900, max=183.732, mean=129.913, std=27.473
- **AllCommodities**: min=128.100, max=280.251, mean=191.201, std=38.770
- **FarmProducts**: min=94.300, max=263.817, mean=163.533, std=43.607
- **ProcessedFoodsAndFeeds**: min=131.000, max=279.089, mean=195.292, std=40.770
- **CrudePetroleum**: min=59.200, max=414.562, mean=198.549, std=72.305
- **FinalDemandGoodsLessFoods**: min=100.000, max=145.152, mean=116.877, std=14.019

### Contextual Validation

```
Most recent PPI: time=20260201, FinalDemand=153.186
```

### Sample Rows

```
First: 20000101,20000217 08:30,nan,nan,nan,nan,nan,nan,128.3,95.9,131.0,76.3,nan
Middle: 20130101,20130220 08:30,108.3,105.5,nan,111.9,106.2,106.4,202.5,202.7,207.5,275.1,108.1
Last: 20260201,20260318 08:30,153.186,152.168,140.165,149.694,153.651,183.732,267.848,241.653,274.240,200.303,144.407
```

## JOLTS

### Structural Checks

- Column count: 10 (expected 10) - PASS
- Row count: 218 - PASS (not empty)
- No completely empty rows - PASS
- All time values parse correctly - PASS
- Chronologically ordered - PASS
- No duplicate time values - PASS
- All endtime values parse correctly - PASS
- EndTime > Time for all rows - PASS

### Per-Column Coverage

- JobOpenings: 218/218 (100%) - FULL
- JobOpeningsRate: 218/218 (100%) - FULL
- Hires: 218/218 (100%) - FULL
- HiresRate: 218/218 (100%) - FULL
- Quits: 218/218 (100%) - FULL
- QuitsRate: 218/218 (100%) - FULL
- TotalSeparations: 218/218 (100%) - FULL
- LayoffsAndDischarges: 218/218 (100%) - FULL

### Coverage and Density

- Date range: 2007-11-01 to 2026-01-01
- Average data points per year: 12.0
- Density check PASS (>= 10 points/year)

### Statistical Checks

- **JobOpenings**: min=2157.000, max=12616.000, mean=6022.784, std=2464.198
- **JobOpeningsRate**: min=1.600, max=7.700, mean=3.937, std=1.384
- **Hires**: min=2822.000, max=8716.000, mean=5201.858, std=1034.078
  Outliers (>3 std): 2 values
    Row 150: 20200501 = 8716.000
    Row 151: 20200601 = 8389.000
- **HiresRate**: min=2.200, max=6.500, mean=3.613, std=0.639
  Outliers (>3 std): 2 values
    Row 150: 20200501 = 6.500
    Row 151: 20200601 = 6.100
- **Quits**: min=1410.000, max=5204.000, mean=2889.642, std=818.204
- **QuitsRate**: min=1.100, max=3.500, mean=1.989, std=0.487
  Outliers (>3 std): 1 values
    Row 165: 20210801 = 3.500
- **TotalSeparations**: min=3168.000, max=15625.000, mean=5110.972, std=1159.974
  Outliers (>3 std): 2 values
    Row 148: 20200301 = 15625.000
    Row 149: 20200401 = 11452.000
- **LayoffsAndDischarges**: min=1101.000, max=12689.000, mean=1885.321, std=940.493
  Outliers (>3 std): 2 values
    Row 148: 20200301 = 12689.000
    Row 149: 20200401 = 9026.000

### Contextual Validation

- COVID check (Apr 2020): JobOpenings=5166
```
Most recent JOLTS: time=20260101, JobOpenings=7110
```

### Sample Rows

```
First: 20071101,20080110 10:00,4317,3.0,4694,3.4,2325,1.7,4591,1968
Middle: 20161201,20170207 10:00,5287,3.5,3988,2.7,2528,1.7,4852,1951
Last: 20260101,20260313 10:00,7110,4.3,5186,3.3,3038,1.9,5556,2132
```

## Summary

- Total files: 4
- Total rows: 1157
- Date range: Jan 2000 - Feb 2026 (CPI/CES/PPI), Nov 2007 - Jan 2026 (JOLTS)
- Validation result: **PASS**