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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Media.Imaging;
    using System.Xml.Serialization;

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
            var exif = new List<ExifTag>(new ExifTagCollection(filename));
            var camera = exif.Find(t => t.Id == 0x110);
            var iso = exif.Find(t => t.Id == 34855);
            var aperture = exif.Find(t => t.Id == 0x9202);
            var exposure = exif.Find(t => t.Id == 0x829A);
            var focalLength = exif.Find(t => t.Id == 0x920A);

            // var img1 = Image.FromFile(filename);
            // var pi = img1.PropertyItems;
            // var pil = img1.PropertyIdList;
            var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
            var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
            string keywords = (metadata != null && metadata.Keywords != null)
                                  ? metadata.Keywords.Aggregate((old, val) => old + "; " + val)
                                  : null;

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
            foreach (var photo in Photos)
            {
                // adjust relative path
                photo.RelFile = Util.RelativePath(photo.AbsFile, Path.GetDirectoryName(filename));
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