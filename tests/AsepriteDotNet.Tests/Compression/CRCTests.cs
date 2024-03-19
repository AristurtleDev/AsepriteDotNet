// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Text;
using AsepriteDotNet.Compression;

namespace AsepriteDotNet.Tests.Compression;

public class CRCTests
{
    [Fact]
    public void CRC_Test()
    {
        CRC crc = new();
        Assert.Equal(0xC061813AU, crc.Update(Encoding.UTF8.GetBytes("It may be that the gulfs will wash us down, ")));
        Assert.Equal(0xAE0CCA93U, crc.Update(Encoding.UTF8.GetBytes("It may be we shall touch the Happy Isles, ")));
        Assert.Equal(0x00EA1BBDU, crc.Update(Encoding.UTF8.GetBytes("and though We are not now that strength ")));
        Assert.Equal(0x0436A1DFU, crc.Update(Encoding.UTF8.GetBytes("which in old days Moved earth and heaven, ")));
        Assert.Equal(0x5CC83E5DU, crc.Update(Encoding.UTF8.GetBytes("that which we are, we are.")));

        Assert.Equal(0x5CC83E5DU, crc.CurrentValue);
    }
}
