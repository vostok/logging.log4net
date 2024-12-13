## 1.0.8 (13-12-2024): 

Bump NuGet deps versions

## 1.0.7 (06-12-2021):

Added `net6.0` target.

## 1.0.5 (06.09.2021):

Fixed context rendering with `UseVostokTemplate` setting ([pull request](https://github.com/Tinkturianec/logging.log4net/pull/2)).

## 1.0.3 (28.02.2020):

Added `UseVostokTemplate` setting.

## 1.0.2 (18.10.2019):

* Fixed lowerCamelCase `WellKnownProperties`.

## 1.0.1 (03.05.2019)

Fixed a bug where Log4netLog wouldn't preserve its configured LoggerNameFactory after ForContext() calls.

## 1.0.0 (11.03.2019):

Breaking change: ForContext() is now hierarchical. Log4netLog now exposes a LoggerNameFactory to customize creation of logger names from source context. 

## 0.1.0 (06-09-2018): 

Initial prerelease.
