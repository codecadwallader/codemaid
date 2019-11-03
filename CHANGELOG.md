# Changelog

## vNext (11.2)

These changes have not been released to the Visual Studio marketplace, but (if checked) are available in preview within the [CI build](http://vsixgallery.com/extension/4c82e17d-927e-42d2-8460-b473ac7df316/).

- [ ] Features
- [ ] Fixes

## Previous Releases

These are the changes to each version that has been released to the Visual Studio marketplace.

## 11.1

**2019-11-03**

- [x] Features
  - [x] [#620](https://github.com/codecadwallader/codemaid/pull/620) - Formatting: Allow for individual tag formatting rules - thanks [willemduncan](https://github.com/willemduncan)!
  - [x] [#665](https://github.com/codecadwallader/codemaid/pull/665) - Use image monikors so icons show up again when tool windows are small - thanks [Diermeier](https://github.com/Diermeier)!

- [x] Fixes
  - [x] [#647](https://github.com/codecadwallader/codemaid/pull/647) - Formatting: Fix magically added slashes - thanks [willemduncan](https://github.com/willemduncan)!
  - [x] [#670](https://github.com/codecadwallader/codemaid/pull/670) - Options: Fix importing read-only config - thanks [Smartis2812](https://github.com/Smartis2812)!

## 11.0

**2019-03-23**

- [x] Features
  - [x] [#625](https://github.com/codecadwallader/codemaid/pull/625) - Use new AsyncPackage base class

- [x] Fixes
  - [x] [#613](https://github.com/codecadwallader/codemaid/pull/613) - Formatting: Avoid trailing comment line on self closing XML tag - thanks [bonimy](https://github.com/bonimy)!
  - [x] [#632](https://github.com/codecadwallader/codemaid/issues/632) - Options: Fix XML encoding issue from resource translations

## 10.6

**2018-12-09**

- [x] Features
  - [x] [#582](https://github.com/codecadwallader/codemaid/pull/582) - Digging: New option to show item types (e.g. method return or property type) - thanks [GammaWolf](https://github.com/GammaWolf)!
  - [x] [#593](https://github.com/codecadwallader/codemaid/pull/593) - Switching: Add .cshtml -> .cshtml.cs to defaults - thanks [derekmckinnon](https://github.com/derekmckinnon)!
  - [x] [#594](https://github.com/codecadwallader/codemaid/pull/594) - Cleaning: New option to add blank lines before/after single-line fields - thanks [jasonjtyler](https://github.com/jasonjtyler)!
  - [x] [#604](https://github.com/codecadwallader/codemaid/pull/604) - Turn on VS2019 support - thanks [digovc](https://github.com/digovc)!

## 10.5

**2018-06-09**

- [x] Features
  - [x] [#477](https://github.com/codecadwallader/codemaid/pull/477) - Digging: New radial progress bar - thanks [Jammer77](https://github.com/Jammer77)!
  - [x] [#506](https://github.com/codecadwallader/codemaid/pull/506) - Enable localization for Chinese - thanks [maikebing](https://github.com/maikebing)!
  - [x] [#519](https://github.com/codecadwallader/codemaid/pull/519) - Simplify the code by removing unnecessary guids - thanks [heku](https://github.com/heku)!
  - [x] [#525](https://github.com/codecadwallader/codemaid/pull/525) - Make all features switchable - thanks [heku](https://github.com/heku)!
  - [x] [#545](https://github.com/codecadwallader/codemaid/pull/545) - Ignore comment lines starting with certain prefixes - thanks [willemduncan](https://github.com/willemduncan)!
  
- [x] Fixes
  - [x] [#479](https://github.com/codecadwallader/codemaid/pull/479) - Update XAML Styler integration mappings - thanks [grochocki](https://github.com/grochocki)!
  - [x] [#496](https://github.com/codecadwallader/codemaid/pull/496) - Fix the .NET Framework minimum required version (which is v4.6)
  - [x] [#541](https://github.com/codecadwallader/codemaid/pull/541) - Project file (.csproj) cleanup - thanks [heku](https://github.com/heku)!
  - [x] [#546](https://github.com/codecadwallader/codemaid/pull/546) - Fix a setting that would leave a trailing white space when formatting comments - thanks [willemduncan](https://github.com/willemduncan)!
  - [x] [#556](https://github.com/codecadwallader/codemaid/issues/556) - Partial fix reducing how long we will block waiting for a code model to be built.

## 10.4

**2017-03-26**

- [x] Features
  - [x] [#444](https://github.com/codecadwallader/codemaid/pull/444) - Cleaning: VB now supports many of the same cleanups as C# - thanks [thehutman](https://github.com/thehutman)!
  - [x] [#449](https://github.com/codecadwallader/codemaid/pull/449) - Undo a previous pull request for hiding Spade during full screen mode (inconsistent with other extensions) - thanks [iouri-s](https://github.com/iouri-s)!

- [x] Fixes
  - [x] [#333](https://github.com/codecadwallader/codemaid/issues/333) - Reorganizing: VB now moves attributes - thanks [thehutman](https://github.com/thehutman)!
  - [x] [#440](https://github.com/codecadwallader/codemaid/issues/440) - Cleaning: Exclude *.min.css and *.min.js files by default

## 10.3

**2017-03-26**

- [x] Features
  - [x] [#359](https://github.com/codecadwallader/codemaid/pull/359) - Reorganizing: Add option to sort private->public vs. public->private - thanks [ahalassy](https://github.com/ahalassy)!
  - [x] [#394](https://github.com/codecadwallader/codemaid/pull/394) - Finding: Add ability to clear solution explorer search before finding - thanks [joeburdick](https://github.com/joeburdick)!
  - [x] [#420](https://github.com/codecadwallader/codemaid/pull/420) - Upgraded projects to .NET 4.6.1 and misc. fixes for VS2017 build support

- [x] Fixes
  - [x] [#419](https://github.com/codecadwallader/codemaid/pull/419) - Cleaning: Switched using statement cleanup command to workaround VS2017+ReSharper issue that prevented using statement cleanup from activating - thanks [jlbeard84](https://github.com/jlbeard84)!

## 10.2

**2017-01-01**

- [x] Features
  - [x] [#284](https://github.com/codecadwallader/codemaid/issues/284) - Performance improvements to compiling regular expressions - thanks [flagbug](https://github.com/flagbug)!
  - [x] [#298](https://github.com/codecadwallader/codemaid/issues/298) - First class support for VB regions (viewing, inserting and removing)
  - [x] [#337](https://github.com/codecadwallader/codemaid/issues/337) - Reorganizing: Add option to put explicit interface implementations after other members - thanks [samcragg](https://github.com/samcragg)!
  - [x] [#371](https://github.com/codecadwallader/codemaid/issues/371) - Support for VS2017 RC

- [x] Fixes
  - [x] [#290](https://github.com/codecadwallader/codemaid/issues/290) - Finding: When track active item is enabled an error can be displayed on invocation
  - [x] [#315](https://github.com/codecadwallader/codemaid/issues/315) - Reorganizing: Explicit interface implementations may take multiple passes to get in stable order - thanks [samcragg](https://github.com/samcragg)!
  - [x] [#326](https://github.com/codecadwallader/codemaid/issues/326) - Digging: VB comments were not visible
  - [x] [#342](https://github.com/codecadwallader/codemaid/issues/342) - Digging: VB regions were not visible - thanks [aeab13](https://github.com/aeab13)!

## 10.1

**2016-04-23**

- [x] Features
  - [x] [#241](https://github.com/codecadwallader/codemaid/issues/241) - Create a demo video
  - [x] [#245](https://github.com/codecadwallader/codemaid/issues/245) - Reorganizing: Support for VB
  - [x] [#248](https://github.com/codecadwallader/codemaid/issues/248) - Cleaning: Exclude files where auto-generated header is detected
  - [x] [#266](https://github.com/codecadwallader/codemaid/issues/266) - Remove solution explorer toolbar buttons
  - [x] [#268](https://github.com/codecadwallader/codemaid/issues/268) - Reorganizing: Add prompt to override safety checks in presence of preprocessor conditionals

- [x] Fixes
  - [x] [#227](https://github.com/codecadwallader/codemaid/issues/227) - VS2015 could freeze for 30s with Visual F# Power Tools installed and CodeMaid F# cleanup disabled
  - [x] [#255](https://github.com/codecadwallader/codemaid/issues/255) - Expected exception messages for Node.JS project item detection should be reduced to diagnostic level
  - [x] [#256](https://github.com/codecadwallader/codemaid/issues/256) - When VS creates a dummy solution, CodeMaid options were not accessible
  - [x] [#272](https://github.com/codecadwallader/codemaid/issues/272) - Reorganizing: Remove existing regions affects in-method regions as well
  - [x] [#275](https://github.com/codecadwallader/codemaid/issues/275) - Digging: In-method regions were being shown within Spade
  - [x] [#276](https://github.com/codecadwallader/codemaid/issues/276) - ReSharper 2016.1 changed the name of their cleanup command and needed updates within CodeMaid - thanks [jamiehumphries](https://github.com/jamiehumphries)!

## 10.0

**2016-04-02**

- [x] Features
  - [x] Add support for Visual Studio "15" Preview
  - [x] Add support for R language
  - [x] Automate deploys through AppVeyor to create a CI channel
  - [x] Consolidate support sites
  - [x] #235 - Extended support for CodeMaid integration into solution explorer context menus to cover scenarios like selecting items in separate projects

- [x] Fixes
  - [x] #231 - VB: Moving functions around in Spade wasn't moving XML comments
  - [x] #239 - Reorganize was not disabling in the presence of #pragma statements

## 0.9.1

**2016-03-16**

- [x] Cleaning
  - [x] Enforce/update copyright statement headers
  - [x] Support basic cleanup on any file type
  - [x] #198 - VisualGDB now supported (thanks adontz!)

- [x] Digging
  - [x] #199 - Make Spade disappear/reappear when entering/exiting new full screen mode (thanks iouri-s!)

- [x] Formatting
  - [x] Added support for XML based list tags (thanks Willem Duncan!)

- [x] Others
  - [x] #193 - Added undo transaction to insert region command (thanks Matthias Reitinger!)
  - [x] Remove multithread operations performance option that was root of problems like #212

### CHANGELOG started with v0.9.1, see [Blog](http://www.codemaid.net/news/) for deeper history
