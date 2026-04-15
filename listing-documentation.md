### Requesting Data

To add BLS data to your algorithm, call the **AddData** method for each survey category.
Save a reference to the dataset **Symbol** so you can access the data later in your algorithm.

```python
class BLSDataAlgorithm(QCAlgorithm):

    def initialize(self) -> None:
        self.set_start_date(2020, 1, 1)
        self.set_end_date(2024, 1, 1)
        self.set_cash(100000)
        self._cpi_symbol = self.add_data(BLSEconomicSurveysCpi, "CPI", Resolution.DAILY).symbol
        self._ces_symbol = self.add_data(BLSEconomicSurveysCes, "CES", Resolution.DAILY).symbol
        self._ppi_symbol = self.add_data(BLSEconomicSurveysPpi, "PPI", Resolution.DAILY).symbol
        self._jolts_symbol = self.add_data(BLSEconomicSurveysJolts, "JOLTS", Resolution.DAILY).symbol
```

```csharp
public class BLSDataAlgorithm : QCAlgorithm
{
    private Symbol _cpiSymbol, _cesSymbol, _ppiSymbol, _joltsSymbol;

    public override void Initialize()
    {
        SetStartDate(2020, 1, 1);
        SetEndDate(2024, 1, 1);
        SetCash(100000);
        _cpiSymbol = AddData<BLSEconomicSurveysCpi>("CPI", Resolution.Daily).Symbol;
        _cesSymbol = AddData<BLSEconomicSurveysCes>("CES", Resolution.Daily).Symbol;
        _ppiSymbol = AddData<BLSEconomicSurveysPpi>("PPI", Resolution.Daily).Symbol;
        _joltsSymbol = AddData<BLSEconomicSurveysJolts>("JOLTS", Resolution.Daily).Symbol;
    }
}
```

### Accessing Data

To get the current BLS data, index the current [Slice](https://www.quantconnect.com/docs/v2/writing-algorithms/key-concepts/time-modeling/timeslices) with the dataset **Symbol**.
**Slice** objects deliver unique events to your algorithm as they happen, but the **Slice** may not contain data for your dataset at every time step.
To avoid issues, check if the **Slice** contains the data you want before you index it.

```python
def on_data(self, data: Slice) -> None:
    if self._cpi_symbol in data:
        cpi = data[self._cpi_symbol]
        all_items = cpi.all_items
        core = cpi.core_cpi
        energy = cpi.energy
```

```csharp
public override void OnData(Slice data)
{
    if (data.ContainsKey(_cpiSymbol))
    {
        var cpi = data.Get<BLSEconomicSurveysCpi>(_cpiSymbol);
        var allItems = cpi.AllItems;
        var core = cpi.CoreCpi;
        var energy = cpi.Energy;
    }
}
```

### Historical Data

To get historical BLS data, call the **History** method with the dataset **Symbol**.
If there is no data in the period you request, the history result is empty.

```python
# DataFrame
history_df = self.history(self._cpi_symbol, 100, Resolution.DAILY)

# Dataset objects
history_bars = self.history[BLSEconomicSurveysCpi](self._cpi_symbol, 100, Resolution.DAILY)
```

```csharp
var history = History<BLSEconomicSurveysCpi>(_cpiSymbol, 100, Resolution.Daily);
```

For more information about historical data, see [History Requests](https://www.quantconnect.com/docs/v2/writing-algorithms/historical-data/history-requests).

### Remove Subscriptions

To remove a subscription to the BLS data, call the **RemoveSecurity** method.

```python
self.remove_security(self._cpi_symbol)
```

```csharp
RemoveSecurity(_cpiSymbol);
```
