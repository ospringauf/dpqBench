// -----------------------------------------------------------------------------------------
// DpBench - YxTextTableRenderer.cs
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

namespace Paguru.DpBench.Renderer
{
    using System.Collections.Generic;

    using Paguru.DpBench.Model;

    /// <summary>
    /// Row/column renderer, only for preview/testing of the layout logic.
    /// Will only output the tile names.
    /// </summary>
    public class YxTextTableRenderer : IRenderer
    {
        public object Render(GroupFilter f)
        {
            string s = string.Empty;
            var rows = RenderAsText(f, f.Input);
            foreach (var row in rows)
            {
                s += row + "\r\n";
            }
            return s;
        }

        private List<string> RenderAsText(GroupFilter gl, PhotoDetailCollection input = null)
        {
            var result = new List<string>();
            if (gl.IsLast)
            {
                result.Add(RenderRow(gl, input));
            }
            else
            {
                foreach (var pv in gl.ParameterValues.SelectedValues)
                {
                    var renderedRows = RenderAsText(gl.NextGroupFilter, gl.Filter(pv as string, input)) as List<string>;
                    foreach (var row in renderedRows)
                    {
                        result.Add(string.Format("{0} | {1}", (pv as string).PadLeft(20), row));
                    }
                }

                // parallel execution might destroy the sort order defined by the user
                //Parallel.ForEach(
                //    gf.ParameterValues.SelectedValues,
                //    x =>
                //        {
                //            var pv = x;
                //            var renderedRows = RenderAsText(gf.NextGroupFilter, gf.Filter(pv as string, input));
                //            foreach (var row in renderedRows)
                //            {
                //                result.Add(string.Format("{0} | {1}", (pv as string).PadLeft(30), row));
                //            }
                //        });
            }

            return result;
        }

        private string RenderRow(GroupFilter gl, PhotoDetailCollection input)
        {
            string s = string.Empty;
            foreach (var pv in gl.ParameterValues.SelectedValues)
            {
                string tileText = string.Empty;
                var tiles = gl.Filter(pv as string, input);
                if (tiles.Count == 0)
                {
                    tileText = "-";
                }
                else if (tiles.Count > 1)
                {
                    tileText = pv + ": " + tiles.Count + " matches";
                }
                else
                {
                    tileText = tiles[0].ToString();
                }
                s += string.Format("[{0}] ", tileText.PadLeft(20));
            }
            return s;
        }
    }
}