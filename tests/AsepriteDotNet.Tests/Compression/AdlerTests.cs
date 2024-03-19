// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Text;
using AsepriteDotNet.Compression;

namespace AsepriteDotNet.Tests.Compression;

public class AdlerTests
{
    [Fact]
    public void Adler_Test()
    {
        Adler32 adler = new();
        Assert.Equal(1U, adler.CurrentValue);

        Assert.Equal(0x59E60F56U, adler.Update(Encoding.UTF8.GetBytes("It may be that the gulfs will wash us down, ")));
        Assert.Equal(0x15B41DC3U, adler.Update(Encoding.UTF8.GetBytes("It may be we shall touch the Happy Isles, ")));
        Assert.Equal(0xE30E2C3EU, adler.Update(Encoding.UTF8.GetBytes("and though We are not now that strength ")));
        Assert.Equal(0x69863AFDU, adler.Update(Encoding.UTF8.GetBytes("which in old days Moved earth and heaven, ")));
        Assert.Equal(0xE57243E3U, adler.Update(Encoding.UTF8.GetBytes("that which we are, we are.")));

        Assert.Equal(0xE57243E3U, adler.CurrentValue);
        adler.Reset();
        Assert.Equal(1U, adler.CurrentValue);
    }
}
