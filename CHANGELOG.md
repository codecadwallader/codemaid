# Changelog

## vNext (TBD)

These changes have not been released to the official Visual Studio extension gallery, but (if checked) are available in preview within the [CI build](http://vsixgallery.com/extension/4c82e17d-927e-42d2-8460-b473ac7df316/).

- [ ] TBD

## Previous Releases

These are the changes to each version that has been released to the official Visual Studio extension gallery.

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
