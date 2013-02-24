// -----------------------------------------------------------------------------------------
// DpBench - Project.cs
// http://sourceforge.net/projects/dpbench/
// -----------------------------------------------------------------------------------------
// Copyright 2013 Oliver Springauf
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// -----------------------------------------------------------------------------------------

namespace Paguru.DpBench.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Xml.Serialization;

    using DeepZoomPublisher;

    using LevDan.Exif;

    /// <summary>
    /// The dpqb project definition.
    /// A collection of photos (images with metadata), detail areas and rendering filters.
    /// </summary>
    public class Project
    {
        #region Constructors and Destructors

        public Project()
        {
            DetailLoadSaveList = new List<DetailArea>();
            PhotoLoadSaveList = new List<Photo>();
            DetailAreas = new BindingList<DetailArea>();
            Photos = new BindingList<Photo>();
            Filters = new List<GroupFilter>();
        }

        #endregion

        #region Public Properties

        [XmlIgnore]
        public BindingList<DetailArea> DetailAreas { get; set; }

        /// <summary>
        /// Gets or sets the details load save list - only for serialization -.
        /// </summary>
        [XmlElement(ElementName = "Details")]
        public List<DetailArea> DetailLoadSaveList { get; set; }

        [XmlIgnore]
        public string Name { get; set; }

        [XmlIgnore]
        public string ProjectFile { get; set; }
        
        [XmlIgnore]
        public string ProjectDir
        {
            get
            {
                return string.IsNullOrEmpty(ProjectFile) ? null : Path.GetDirectoryName(ProjectFile);
            }
        }

        /// <summary>
        /// Gets or sets the photo load save list - only for serialization -.
        /// </summary>
        [XmlElement(ElementName = "Photos")]
        public List<Photo> PhotoLoadSaveList { get; set; }

        [XmlIgnore]
        public BindingList<Photo> Photos { get; set; }

        public List<GroupFilter> Filters { get; set; }

        /// <summary>
        /// Gets or sets the default filter.
        /// TODO: management of more than one filter
        /// </summary>
        [XmlIgnore]
        public GroupFilter DefaultFilter
        {
            get
            {
                return (Filters.Count > 0) ? Filters[0] : null;
            }
            set
            {
                Filters.Clear();
                if (value != null)
                {
                    Filters.Add(value);
                }
            }
        }

        #endregion

        #region Public Methods

        public static Project Load(string filename)
        {
            var project = Util.DeserializeFromXmlFile<Project>(filename);
            project.ProjectFile = filename;
            project.Name = project.Name ?? Path.GetFileNameWithoutExtension(filename);

            project.PhotoLoadSaveList.ForEach(
                p =>
                    {
                        project.Photos.Add(p);
                        p.Project = project;
                        p.AbsFile = Path.Combine(Path.GetDirectoryName(filename), p.RelFile);
                    });
            project.DetailLoadSaveList.ForEach(project.DetailAreas.Add);

            return project;
        }

        public void AddFile(string filename)
        {
            List<ExifTag> exif = new List<ExifTag>();
            //try
            {
                exif.AddRange(new ExifTagCollection(filename));
            }
            //catch (Exception)
            {
                // ignore
            }
            var camera = exif.Find(t => t.Id == 0x110);
            var iso = exif.Find(t => t.Id == 34855);
            var aperture = exif.Find(t => t.Id == 0x9202);
            var exposure = exif.Find(t => t.Id == 0x829A);
            var focalLength = exif.Find(t => t.Id == 0x920A);

            var keywords = string.Empty;

            // read the IPTC keywords
            // http://stackoverflow.com/questions/680654/reading-iptc-information-with-c-net-framework-2
            // we could do this with the .NET 3.5 WPF libs (see below), but I prefer the simple solution
            try
            {
                JpegParser parser = new JpegParser(filename);
                if (parser.ParseDocument())
                {
                    //Console.WriteLine("Parsed {0} {1}", System.IO.Path.GetFileName(filename), parser.Title);
                    //Console.WriteLine("Tags: {0}", parser.KeywordString);
                    //Console.WriteLine("Description: {0}", parser.Description);
                    //Console.WriteLine("Title: {0}", parser.Title);
                    //Console.WriteLine("Rating: {0}", parser.Rating);

                    keywords = parser.KeywordString;
                }
            }
            catch (Exception)
            {
                // ignore
            }

            // TODO EXIF data for TIFF files are incomplete
            // http://bitmiracle.com/libtiff/ ?
            // http://bitmiracle.com/libtiff/help/read-exif-metadata.aspx
            // http://www.awaresystems.be/imaging/tiff/tifftags/privateifd/exif.html

            //var er2 = new Goheer.EXIF.EXIFextractor(filename, string.Empty, string.Empty);
            //foreach (DictionaryEntry de in er2)
            //{
            //    Console.Out.WriteLine(de.Key +"=" + de.Value);
            //}

            // this is the WPF solution
            //var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            //var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
            //var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
            //string keywords = (metadata != null && metadata.Keywords != null)
            //                      ? metadata.Keywords.Aggregate((old, val) => old + "; " + val)
            //                      : null;

            //// property item solution
            //using (var img = Image.FromFile(filename))
            //{
            //    foreach (var pi in img.PropertyItems)
            //    {
            //        Console.Out.WriteLine(pi.Id +"=" + pi.Value);
            //    }
            //}

            var photo = new Photo()
                {
                    Project = this, 
                    RelFile = Util.RelativePath(filename, ProjectDir), 
                    AbsFile = Path.GetFullPath(filename),
                    Aperture = (aperture != null) ? aperture.Value : null, 
                    Camera = (camera != null) ? camera.Value : null, 
                    Exposure = (exposure != null) ? exposure.Value : null, 
                    Iso = (iso != null) ? iso.Value : null, 
                    FocalLength = (focalLength != null) ? focalLength.Value : null, 
                    Keywords = keywords
                };
            Photos.Add(photo);
        }

        /// <summary>
        /// Creates all details (cross product of photos and detail areas)
        /// </summary>
        /// <returns></returns>
        public PhotoDetailCollection CreateAllDetails()
        {
            var l = new PhotoDetailCollection();
            foreach (var photo in Photos)
            {
                foreach (var detailArea in DetailAreas)
                {
                    l.Add(new PhotoDetail(photo, detailArea));
                }
            }
            return l;
        }

        public Size MaxCropSize()
        {
            var result = new Rectangle();
            foreach (var area in DetailAreas)
            {
                var x = new Rectangle(new Point(0, 0), area.Crop.Size);
                result = Rectangle.Union(result, x);
            }
            return result.Size;
        }

        public void Save(string filename)
        {
            if (!string.Equals(filename, ProjectFile))
            {
                foreach (var photo in Photos)
                {
                    // adjust photos' relative path to new project location
                    photo.RelFile = Util.RelativePath(photo.AbsFile, Path.GetDirectoryName(filename));
                }
            }
            ProjectFile = filename;
            PhotoLoadSaveList = new List<Photo>(Photos);
            DetailLoadSaveList = new List<DetailArea>(DetailAreas);
            Name = Path.GetFileNameWithoutExtension(filename);
            Util.SerializeToXmlFile(this, filename);
        }

        #endregion

        public void AddFiles(string[] fileNames)
        {
            MainWindow.Instance.ScaleProgress("loading file(s)", fileNames.Length);
            foreach (var fileName in fileNames)
            {
                AddFile(fileName);
                MainWindow.Instance.Progress++;
            }
            MainWindow.Instance.ClearProgress();
        }
    }
}