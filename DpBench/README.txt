﻿dpqBench 
http://sourceforge.net/projects/dpbench
Oliver Springauf, 2013


dpqBench is a tool for comparing details of digital images. It is written in C# and requires
the Microsoft .NET Framework 3.5.

dpqBench is Open Source Software, licensed under the conditions of the Apache License 2.0
(http://www.apache.org/licenses/LICENSE-2.0.html)


Version History

v0.6
compatible with Mono (tested on Mono 2.10.8.1/Ubuntu) with some restrictions:
- drag and drop might not work
- dock layout does not work, controls are opened in separate windows
- ObjectListView does not work, project window uses simple table

v0.5
fixed IndexOutOfRangeException when started with one file name on command line
treated "OverflowException: Der Wert für einen UInt32 war zu groß oder zu klein." during EXIF extraction

v0.4
internal refactoring
command line interface supports multiple image file names to be opened in a new project
(for integration with other software)

v0.3 - 03 Feb 2013
improved details editor, move/resize detail areas by dragging with the mouse

v0.2 - 02 Feb 2013
initial public release

Acknowledgements

The following components were used to create dpqBench:

ExifMetadata
http://www.codeproject.com/Articles/27242/ExifTagCollection-An-EXIF-metadata-extraction-libr
CPOL license: http://www.codeproject.com/info/cpol10.aspx

ObjectListView
http://objectlistview.sourceforge.net/cs/index.html
GPL license

Weifen Luo Dock Panel Suite
http://github.com/dockpanelsuite
MIT license

Iconic icon collection
https://github.com/somerandomdude/Iconic
Creative Commons license