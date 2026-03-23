# BLSEconomicSurveys - LEAN Data Source Contribution

## Overview

**Bureau of Labor Statistics Economic Surveys** - U.S. macroeconomic indicators across CPI, CES (Employment Situation), PPI, and JOLTS surveys with exact publication timestamps.

| Property | Value |
|---|---|
| **Type** | Unlinked dataset |
| **Requires Mapping** | No |
| **Has Universe Data** | No |
| **Resolution** | Daily |
| **Timezone** | America/New_York |
| **Sparse** | Yes |
| **Streaming** | No |
| **Data Process Time** | ~30s (full history, cached release dates) |
| **Data Process Duration** | ~30s |
| **Update Process Duration** | ~5s |

## Publication Lag

Each data point's `EndTime` is set to the **exact release date and time** scraped from BLS schedule pages at `https://www.bls.gov/schedule/{year}/home.htm`. CPI, CES, and PPI are released at 08:30 AM ET; JOLTS at 10:00 AM ET. Release dates are only available from the year 2000 onward (when BLS began publishing online schedules), so data prior to January 2000 is not included. The processing script caches scraped release dates to `release_dates_cache.json` to avoid repeated scraping.

## Partial Coverage Notes

Some series within a survey category were introduced after the dataset's start date:
- **PPI Final Demand series** (FinalDemand, CorePpi, FinalDemandGoods, etc.) were introduced in November 2009. These columns will be empty for data points before that date. The Reader handles empty values by returning `null` for those properties.
- **CES AverageHourlyEarnings, AverageWeeklyHours, AverageWeeklyEarnings** series (CEU0500000003, CEU0500000002, CEU0500000011) may have limited early data.
- **JOLTS** data begins November 2007 (earliest release date found on BLS schedule pages in new format).

Algorithms should use null-checking (`HasValue` in C#, `is not None` in Python) when accessing properties that may be absent for older data points.

## Generated Files

| File | Purpose |
|---|---|
| `BLSEconomicSurveysCpi.cs` | CPI data model class (15 series) |
| `BLSEconomicSurveysCes.cs` | CES data model class (16 series) |
| `BLSEconomicSurveysPpi.cs` | PPI data model class (11 series) |
| `BLSEconomicSurveysJolts.cs` | JOLTS data model class (8 series) |
| `BLSEconomicSurveysAlgorithm.cs` | C# demonstration algorithm |
| `BLSEconomicSurveysAlgorithm.py` | Python demonstration algorithm |
| `DataProcessing/process.ipynb` | Jupyter notebook for data processing |
| `DataProcessing/config.json` | Processing configuration |
| `tests/BLSEconomicSurveysTests.cs` | Unit tests |
| `listing-about.md` | Marketplace listing description |
| `listing-documentation.md` | Full documentation |
| `output/` | Sample data for demos and tests |

## Getting Started

### 1. Clone

```bash
git clone https://github.com/<username>/Lean.DataSource.BLS.git
cd Lean.DataSource.BLS
```

### 2. Configure

Edit `DataProcessing/config.json` and set your BLS API key:
```json
{
  "bls-api-key": "YOUR_BLS_API_KEY_HERE",
  "temp-output-directory": "C:/temp-output-directory",
  "data-folder": "/path/to/Lean/Data/"
}
```

Register for a free API key at https://data.bls.gov/registrationEngine/

### 3. Compile

```bash
dotnet build QuantConnect.DataSource.csproj
dotnet build tests/Tests.csproj
```

### 4. Process Data

```bash
python -m venv nb_venv
source nb_venv/bin/activate  # or nb_venv\Scripts\activate on Windows
pip install requests nbconvert ipykernel pandas
cd DataProcessing
jupyter nbconvert --to notebook --execute process.ipynb
```

### 5. Test

```bash
dotnet test tests/Tests.csproj
```

### 6. Submit Pull Request

Before submitting:
- [ ] All unit tests pass
- [ ] Demo algorithms run without error in C# and Python
- [ ] Minimal sample data included in `output/`
- [ ] Processing script reads existing data and merges incrementally
- [ ] Processing script reads deployment date from `QC_DATAFLEET_DEPLOYMENT_DATE`
- [ ] Processing script writes to temp output directory (not repo `output/`)
- [ ] Code follows LEAN conventions (XML docs, decimal types, Apache 2.0 header)

## CSV Format

```
time,endtime,series1,series2,...,seriesN
20240101,20240213 08:30,325.252,331.950,...
```

- `time`: Start of observation period (1st of month, yyyyMMdd)
- `endtime`: Exact release date and time (yyyyMMdd HH:mm)
- Remaining columns: Series values (decimal, empty if not available)

## Reference

- [Dataset SDK Documentation](https://www.quantconnect.com/docs/v2/lean-engine/contributions/datasets/key-concepts)
- [BLS API v2 Documentation](https://www.bls.gov/developers/api_signature_v2.htm)
- [BLS Release Schedule](https://www.bls.gov/schedule/2026/home.htm)
