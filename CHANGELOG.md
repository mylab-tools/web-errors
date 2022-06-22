# Changelog

All notable changes to this project will be documented in this file

Формат лога изменений базируется на [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [1.1.5] - 2022-06-22

### Changed

* сhoosing a binding based on inheritance

## [1.1.4] - 2022-04-06

### Changed

* exception filters deep refactoring without breaking changes

### Fixed

* log binded exceptions if unsuccessful response code

## [1.1.3] - 2021-06-22

### Added

* log fact `http-trace-id` when unhandled exception cought
* update reference to `MyLab.Log.Dsl`

## [1.0.3] - 2020-05-12

### Changed

- move to Net Core 3.1
- string trace identifier insted exeption guid.

## [1.0.2] - 2020-04-27

### Added

- add changelog.

### Fixed

* fix multiple restriction for `ErrorToResponseAttribute`.