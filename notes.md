# Army Arrow Counter

## Todo List

- Optionally support ModLib or OptionScreen or whatever to provide in-game settings.
- Add toggle hotkey.
- Fix the one dude's crash.
- Count only solders under your command.
- Fix bug: Fleeing allies in hideouts don't subtract arrows when removed.

## Customization

### Counter Type

#### EXACT_FRACTION

- e.g. 1258 / 1721

#### NEAREST_100_FRACTION

- e.g. ~1300 / ~1700

#### EXACT_PERCENT

- e.g. 73%

#### NEAREST_10_PERCENT

- e.g. ~70%

#### NEAREST_20_PERCENT

- e.g. ~80%

#### NEAREST_25_PERCENT

- e.g. ~75%

#### NEAREST_WRITTEN

- "Your army has %s ammunition remaining.
- Replace with %s with:
  - all - [100%, 100%]
  - almost all - [87.5%, 100%)
  - about three quarters - [71%, 87.5%)
  - about two thirds - [58.5%, 71%)
  - about half - [41.5%, 58.5%)
  - about one third - [29%, 41.5%)
  - about one quarter - [12.5%, 29%)
  - almost no - (0%, 12.5%)
  - no - [0%, 0%]
