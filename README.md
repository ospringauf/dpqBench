# dpqBench

**TODO** migrate project description & other files from https://sourceforge.net/projects/dpbench/

dpqBench lets you create side-by-side comparisons of digital images. Evaluate the performance of camera sensors and lenses by comparing the image quality at different aperture, ISO setting, or focal length.

dpqBench was written by and for photography enthusiasts who are interested in the quality of their photography gear (pronounce "pixel peepers"). It lets you compare details of images taken with different equipment or at different settings.

dpqBench is Open Source Software, licensed under the conditions of the Apache License 2.0
(http://www.apache.org/licenses/LICENSE-2.0.html)

## System Requirements

Microsoft Windows 32bit or 64bit
Microsoft .NET Framework 3.5 (might change to 4.0 later)

Installation: Extract the ZIP file into a directory of your choice and start the EXE from there. No special installation required.



## Getting started with dpqBench

This section explains the steps for creating benchmark charts with dpqBench. Some examples of benchmark charts can be found in the [Gallery].

### Prepare images

Take a series of test images with the photographic equipment / settings that you want to evaluate. These images should show the same scene, but with different parameters (eg. lenses, aperture settings, ISO, focal lengths, ...).
Save these images as JPG files, but without any image processing (like sharpening). Adding EXIF data will help, but you can also add these data later.

### Create a dpqBench project

dpqBench will start with an empty project window. Add the prepared JPGs by using the Project/Add File(s) menu or by dragging the files into the project. dpqBench will try to read the images parameters from the file's EXIF data. You can add any missing parameters in the project table or in the Properties view.

**TODO** image


### Define detail areas for comparison

Use the Details Editor to define at least one detail area. Select a rectangular image detail with the mouse, and give it a name. You can then fine-tune the size of the area by adjusting the width and height values.
The detail areas are shown on the image preview. You can save the preview together with the detail areas by right-clicking on the preview.

### Create a comparison chart

Finally! Select "Create Benchmark Chart" from the Project menu. This will open the Group filter/Render editor.

**TODO** image

This editor lets you define the layout of the benchmark chart. You can use multiple grouping/filtering levels. The last filter defines the columns - eg. the different aperture values. The other filter levels define rows or row groups - eg. detail area, lens, camera.
On each level, you can select a parameter and the parameter values. You can also arrange the values in any order you like.

Click "Render" to see what your benchmark chart will look like. If the renderer can not choose a unique images for each combination of parameters, it will display a warning - you can then go back and adjust your filter settings or add more filters.

Here's an example with three filter levels (1st level: detail "mid", 2nd level: the lenses, 3rd level: different aperture settings).

To save your benchmark chart, right-click on the preview image.

**TODO** image


## Gallery (examples)

**TODO** images




## Version History

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

## Acknowledgements

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
