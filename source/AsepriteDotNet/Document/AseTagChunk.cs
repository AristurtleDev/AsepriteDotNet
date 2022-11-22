/* -----------------------------------------------------------------------------
Copyright 2022 Aristurtle

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.Collections;

using AsepriteDotNet.Document.Native;

namespace AsepriteDotNet.Document;

public class AseTagsChunk : AseChunk, IEnumerable<AseTag>
{
    private readonly IList<AseTag> _tags = new List<AseTag>();

    public int NumberOfTags { get; }

    public AseTagsChunk(RawChunkHeader header, RawTagsChunk raw)
        : base(header)
    {
        NumberOfTags = raw.NumberOfTags;

        for (int i = 0; i < raw.Tags.Length; i++)
        {
            _tags.Add(new AseTag(raw.Tags[i]));
        }
    }

    public IEnumerator<AseTag> GetEnumerator() => _tags.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _tags.GetEnumerator();
}