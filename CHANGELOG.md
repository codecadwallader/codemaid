# Roadmap

- [x] Create a demo video

Features that have a checkmark are complete and available for download in the [CI build](http://vsixgallery.com/extension/4c82e17d-927e-42d2-8460-b473ac7df316/).

# Changelog

These are the changes to each version that has been released on the official Visual Studio extension gallery.

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