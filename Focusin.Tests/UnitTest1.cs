using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Focusin.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Focusin.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var vm = new MainViewModel();
            vm.UpdateSession();
            Assert.Equals(vm.CurrentSession.IsFreeTime, true);
        }
    }
}
