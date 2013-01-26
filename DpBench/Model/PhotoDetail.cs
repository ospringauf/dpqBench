// -----------------------------------------------------------------------------------------
// DpBench - PhotoDetail.cs
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
    using System.Drawing;

    using ImageConverter = Paguru.DpBench.ImageConverter;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PhotoDetail
    {
        #region Constructors and Destructors

        public PhotoDetail(Photo p, DetailArea a)
        {
            Photo = p;
            Area = a;
            Parameters = Photo.GetParameters();
            Parameters["Detail"] = Area.Name;
        }

        #endregion

        #region Public Properties

        public DetailArea Area { get; private set; }

        public Image Image
        {
            get
            {
                var img = Photo.Image;
                img = ImageConverter.Crop(img, Area.Crop);
                return img;
            }
        }

        public IDictionary<string, string> Parameters { get; private set; }

        public Photo Photo { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Photo.BaseFilename + ":" + Area.Name;
        }

        #endregion
    }
}