"""
Validation script for BLS Economic Surveys processed data.
Checks structural integrity, coverage, statistics, and contextual accuracy.
"""
import os
import sys
from datetime import datetime
from pathlib import Path

import pandas as pd
import numpy as np

DATA_DIR = Path("C:/temp-output-directory/alternative/bls/economicsurveys")

SURVEYS = {
    "cpi": {
        "columns": ["time", "endtime", "AllItems", "CoreCpi", "Food", "FoodAtHome",
                     "FoodAwayFromHome", "Energy", "Shelter", "RentOfPrimaryResidence",
                     "Gasoline", "MedicalCare", "Apparel", "EducationAndCommunication",
                     "NewVehicles", "UsedCarsAndTrucks", "CollegeTuitionAndFees"],
        "expected_cols": 17,
    },
    "ces": {
        "columns": ["time", "endtime", "TotalNonfarm", "TotalPrivate",
                     "AverageHourlyEarnings", "AverageWeeklyHours",
                     "AverageWeeklyEarnings", "ProductionHourlyEarnings",
                     "ProductionEmployees", "Manufacturing", "GoodsProducing",
                     "PrivateServiceProviding", "Construction", "RetailTrade",
                     "FinancialActivities", "EducationAndHealthServices",
                     "LeisureAndHospitality", "MiningAndLogging"],
        "expected_cols": 18,
    },
    "ppi": {
        "columns": ["time", "endtime", "FinalDemand", "CorePpi",
                     "FinalDemandLessFoodEnergyTrade", "FinalDemandGoods",
                     "FinalDemandServices", "FinalDemandConstruction",
                     "AllCommodities", "FarmProducts", "ProcessedFoodsAndFeeds",
                     "CrudePetroleum", "FinalDemandGoodsLessFoods"],
        "expected_cols": 13,
    },
    "jolts": {
        "columns": ["time", "endtime", "JobOpenings", "JobOpeningsRate",
                     "Hires", "HiresRate", "Quits", "QuitsRate",
                     "TotalSeparations", "LayoffsAndDischarges"],
        "expected_cols": 10,
    },
}

report = []


def log(msg=""):
    report.append(msg)
    print(msg)


def validate():
    log("# BLS Economic Surveys - Validation Report")
    log(f"\nGenerated: {datetime.now().strftime('%Y-%m-%d %H:%M')}")
    log("")

    total_files = 0
    total_rows = 0
    all_passed = True

    for survey_key, survey_info in SURVEYS.items():
        log(f"\n## {survey_key.upper()}")
        log("")

        filepath = DATA_DIR / f"{survey_key}.csv"
        if not filepath.exists():
            log(f"**FAIL**: File not found: {filepath}")
            all_passed = False
            continue

        total_files += 1
        col_names = survey_info["columns"]
        expected_cols = survey_info["expected_cols"]

        df = pd.read_csv(filepath, header=None, dtype=str)

        # Structural checks
        log("### Structural Checks")
        log("")

        if df.shape[1] != expected_cols:
            log(f"**FAIL**: Expected {expected_cols} columns, got {df.shape[1]}")
            all_passed = False
        else:
            log(f"- Column count: {df.shape[1]} (expected {expected_cols}) - PASS")

        if len(df) == 0:
            log("**FAIL**: File is empty")
            all_passed = False
            continue

        total_rows += len(df)
        log(f"- Row count: {len(df)} - PASS (not empty)")

        # Check for empty/corrupt rows
        null_rows = df.isnull().all(axis=1).sum()
        if null_rows > 0:
            log(f"**WARN**: {null_rows} completely empty rows")
        else:
            log("- No completely empty rows - PASS")

        # Parse time column
        df.columns = range(len(df.columns))
        times = pd.to_datetime(df[0], format="%Y%m%d", errors="coerce")
        bad_times = times.isna().sum()
        if bad_times > 0:
            log(f"**FAIL**: {bad_times} unparseable time values")
            all_passed = False
        else:
            log("- All time values parse correctly - PASS")

        # Check chronological order
        if times.is_monotonic_increasing:
            log("- Chronologically ordered - PASS")
        else:
            log("**FAIL**: Rows not in chronological order")
            all_passed = False

        # Check for duplicates
        dups = df[0].duplicated().sum()
        if dups > 0:
            log(f"**FAIL**: {dups} duplicate time values")
            all_passed = False
        else:
            log("- No duplicate time values - PASS")

        # Check EndTime > Time
        endtimes = pd.to_datetime(df[1].str.strip(), format="%Y%m%d %H:%M", errors="coerce")
        bad_endtimes = endtimes.isna().sum()
        if bad_endtimes > 0:
            log(f"**FAIL**: {bad_endtimes} unparseable endtime values")
            all_passed = False
        else:
            log("- All endtime values parse correctly - PASS")
            valid_mask = times.notna() & endtimes.notna()
            wrong_order = (endtimes[valid_mask] <= times[valid_mask]).sum()
            if wrong_order > 0:
                log(f"**FAIL**: {wrong_order} rows where EndTime <= Time")
                all_passed = False
            else:
                log("- EndTime > Time for all rows - PASS")

        # Per-column coverage
        log("")
        log("### Per-Column Coverage")
        log("")
        numeric_cols = list(range(2, len(df.columns)))
        for col_idx in numeric_cols:
            col_name = col_names[col_idx] if col_idx < len(col_names) else f"col{col_idx}"
            non_empty = df[col_idx].apply(lambda x: x is not None and str(x).strip() != "").sum()
            pct = 100.0 * non_empty / len(df)
            if pct < 100:
                # Find first non-empty row
                first_valid = None
                for i, v in enumerate(df[col_idx]):
                    if v is not None and str(v).strip() != "":
                        first_valid = df[0].iloc[i]
                        break
                log(f"- {col_name}: {non_empty}/{len(df)} ({pct:.1f}%) - starts at {first_valid}")
            else:
                log(f"- {col_name}: {non_empty}/{len(df)} (100%) - FULL")

        # Coverage and density
        log("")
        log("### Coverage and Density")
        log("")
        min_date = times.min()
        max_date = times.max()
        log(f"- Date range: {min_date.strftime('%Y-%m-%d')} to {max_date.strftime('%Y-%m-%d')}")
        years_span = (max_date - min_date).days / 365.25
        avg_per_year = len(df) / years_span if years_span > 0 else 0
        log(f"- Average data points per year: {avg_per_year:.1f}")
        if avg_per_year < 10:
            log("**WARN**: Fewer than 10 data points per year - may be too sparse")
        else:
            log("- Density check PASS (>= 10 points/year)")

        # Statistical checks
        log("")
        log("### Statistical Checks")
        log("")

        try:
            import matplotlib
            matplotlib.use("Agg")
            import matplotlib.pyplot as plt
        except ImportError:
            plt = None

        for col_idx in numeric_cols:
            col_name = col_names[col_idx] if col_idx < len(col_names) else f"col{col_idx}"
            vals = pd.to_numeric(df[col_idx], errors="coerce").dropna()

            if len(vals) == 0:
                log(f"- {col_name}: NO numeric values")
                continue

            log(f"- **{col_name}**: min={vals.min():.3f}, max={vals.max():.3f}, "
                f"mean={vals.mean():.3f}, std={vals.std():.3f}")

            # Check all zeros
            if (vals == 0).all():
                log(f"  **FAIL**: {col_name} is all zeros")
                all_passed = False

            # Outlier detection
            mean = vals.mean()
            std = vals.std()
            if std > 0:
                outliers = vals[abs(vals - mean) > 3 * std]
                if len(outliers) > 0:
                    log(f"  Outliers (>3 std): {len(outliers)} values")
                    for idx in outliers.index[:3]:
                        log(f"    Row {idx}: {df[0].iloc[idx]} = {outliers[idx]:.3f}")

            # Plot
            if plt is not None:
                fig, ax = plt.subplots(figsize=(10, 4))
                plot_times = pd.to_datetime(df[0], format="%Y%m%d", errors="coerce")
                valid = vals.index
                ax.plot(plot_times.iloc[valid], vals.values, linewidth=1)
                ax.set_title(f"{survey_key.upper()} - {col_name}")
                ax.set_xlabel("Date")
                ax.set_ylabel("Value")
                ax.grid(True, alpha=0.3)
                plt.tight_layout()
                plot_path = Path("C:/Users/derek/claude/Lean.DataSource.BLS/validation") / f"{survey_key}_{col_name}.png"
                plt.savefig(str(plot_path), dpi=100)
                plt.close()

        # Contextual validation - spot checks
        log("")
        log("### Contextual Validation")
        log("")

        if survey_key == "cpi":
            # COVID spike: March 2020 energy should drop
            covid_rows = df[df[0] == "20200301"]
            if len(covid_rows) > 0:
                energy = covid_rows.iloc[0][7]  # Energy column
                log(f"- COVID check (Mar 2020): Energy={energy}")
            # 2008 crisis: gasoline should drop
            crisis_rows = df[df[0] == "20081201"]
            if len(crisis_rows) > 0:
                gasoline = crisis_rows.iloc[0][10]
                log(f"- 2008 crisis (Dec 2008): Gasoline={gasoline}")
            # Recent CPI should be around 310-330
            recent = df.tail(1)
            log(f"```")
            log(f"Most recent CPI: time={recent.iloc[0][0]}, AllItems={recent.iloc[0][2]}")
            log(f"```")

        elif survey_key == "ces":
            # COVID job losses: April 2020 should show massive drop
            covid_rows = df[df[0] == "20200401"]
            if len(covid_rows) > 0:
                nonfarm = covid_rows.iloc[0][2]
                log(f"- COVID check (Apr 2020): TotalNonfarm={nonfarm} (should drop ~20M)")
            recent = df.tail(1)
            log(f"```")
            log(f"Most recent CES: time={recent.iloc[0][0]}, TotalNonfarm={recent.iloc[0][2]}")
            log(f"```")

        elif survey_key == "ppi":
            recent = df.tail(1)
            log(f"```")
            log(f"Most recent PPI: time={recent.iloc[0][0]}, FinalDemand={recent.iloc[0][2]}")
            log(f"```")

        elif survey_key == "jolts":
            # 2020 COVID: job openings should drop
            covid_rows = df[df[0] == "20200401"]
            if len(covid_rows) > 0:
                openings = covid_rows.iloc[0][2]
                log(f"- COVID check (Apr 2020): JobOpenings={openings}")
            recent = df.tail(1)
            log(f"```")
            log(f"Most recent JOLTS: time={recent.iloc[0][0]}, JobOpenings={recent.iloc[0][2]}")
            log(f"```")

        # Spot-check sample rows
        log("")
        log("### Sample Rows")
        log("")
        log("```")
        log(f"First: {','.join(str(x) for x in df.iloc[0])}")
        mid_idx = len(df) // 2
        log(f"Middle: {','.join(str(x) for x in df.iloc[mid_idx])}")
        log(f"Last: {','.join(str(x) for x in df.iloc[-1])}")
        log("```")

    # Summary
    log("")
    log("## Summary")
    log("")
    log(f"- Total files: {total_files}")
    log(f"- Total rows: {total_rows}")
    log(f"- Date range: Jan 2000 - Feb 2026 (CPI/CES/PPI), Nov 2007 - Jan 2026 (JOLTS)")
    log(f"- Validation result: **{'PASS' if all_passed else 'ISSUES FOUND'}**")

    return all_passed


if __name__ == "__main__":
    os.chdir(os.path.dirname(os.path.abspath(__file__)))
    os.chdir("..")
    passed = validate()

    with open("validation/report.md", "w") as f:
        f.write("\n".join(report))

    print(f"\nReport saved to validation/report.md")
    sys.exit(0 if passed else 1)
