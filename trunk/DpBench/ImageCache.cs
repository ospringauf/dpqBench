// -----------------------------------------------------------------------------------------
// DpBench - ImageCache.cs
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

namespace Paguru.DpBench
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Cache images to avoid multiple load of the same file
    /// TODO does not work yet
    /// </summary>
    public sealed class ImageCache : IDisposable
    {
        private static ImageCache instance;

        private Dictionary<string, Image> cache = new Dictionary<string, Image>();

        public static ImageCache CreateCache()
        {
            return instance = new ImageCache();
        }

        public static Image LoadImage(string filename)
        {
            Image img = null;

            // TODO does not work, images will be disposed early by ImageConverter
            //if (instance != null)
            //{
            //    if (!instance.cache.TryGetValue(filename, out img))
            //    {
            //        instance.cache[filename] = img = Image.FromFile(filename);
            //    }
            //    return img;
            //}
            //else
            {
                return Image.FromFile(filename);
            }
        }

        public void Dispose()
        {
            foreach (var img in cache.Values)
            {
                img.Dispose();
            }
            cache = null;
            instance = null;
        }
    }
}