﻿using System.Linq;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoRhinoMock;

namespace Medidata.Cloud.Thermometer.RaveCommon.UnitTests.ExpendoState
{
    [TestClass]
    public class ExpendoStateConcurrentStorageTests
    {
        private IFixture _fixture;
        private ExpendoStateConcurrentStorage _sut;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoRhinoMockCustomization());
            _sut = new ExpendoStateConcurrentStorage();
        }

        [TestMethod]
        public void GetStorage_ReturnNewIfNotExisting()
        {
            // Arrange
            var identity = _fixture.Create<int>();

            // Act
            _sut.GetStorage(identity);

            // Assert
            Assert.AreEqual(1, _sut.PropDic.Count);
            Assert.AreEqual(identity, _sut.PropDic.First().Key);
        }

        [TestMethod]
        public void GetStorage_ReturnIfExisting()
        {
            // Arrange
            var identity = _fixture.Create<int>();
            _sut.GetStorage(identity);
            var expectedStorage = _sut.PropDic.First().Value;

            // Act
            var result = _sut.GetStorage(identity);

            // Assert
            Assert.AreEqual(1, _sut.PropDic.Count);
            Assert.AreEqual(identity, _sut.PropDic.First().Key);
            Assert.AreSame(expectedStorage, result);
        }

        [TestMethod]
        public void ClearStorage_ExistingTarget()
        {
            // Arrange
            var identity = _fixture.Create<int>();

            // Act and Assert
            _sut.GetStorage(identity);
            Assert.AreEqual(1, _sut.PropDic.Count);

            _sut.ClearStorage(identity);
            Assert.AreEqual(0, _sut.PropDic.Count);
        }

        [TestMethod]
        public void ClearStorage_NonExistingTarget()
        {
            // Arrange
            var identity = _fixture.Create<int>();

            // Act and Assert
            _sut.GetStorage(identity);
            Assert.AreEqual(1, _sut.PropDic.Count);

            var deletingIdentity = _fixture.Create<int>();
            _sut.ClearStorage(deletingIdentity);
            Assert.AreEqual(1, _sut.PropDic.Count);
        }
    }
}