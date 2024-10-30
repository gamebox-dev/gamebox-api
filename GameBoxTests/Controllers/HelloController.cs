using GameBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoxTests.Controllers
{
    public class HelloController
    {
        [Fact]
        public void Hello_Test()
        {
            var controller = Moq.Mock.Of<GameBox.Controllers.HelloController>();

            HealthStatus status = controller.Get();

            Assert.Equal("ok", status.Status);
        }
    }
}
