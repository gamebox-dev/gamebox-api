using GameBox.Connectors.IGDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoxTests.Connectors.IGDB
{
    public class IGDBGameSourceTests
    {
        [Fact]
        public void ConformCoverURL_Test()
        {
            string result = IGDBGameSource.ConformCoverURL("//images.igdb.com/igdb/image/upload/t_thumb/co20ac.jpg");

            Assert.Equal("https://images.igdb.com/igdb/image/upload/t_cover_big/co20ac.jpg", result);
        }
    }
}
