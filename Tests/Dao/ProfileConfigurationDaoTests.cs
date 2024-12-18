using Data.Dao;
using Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Linq;

namespace Tests.Dao
{
    [TestClass]
    public class ProfileConfigurationDaoTests
    {

        private BlockusEntities _context;
        private Mock<BlockusEntities> _mockContext;
        private Mock<DbSet<ProfileConfiguration>> _mockSet;

        [TestInitialize]
        public void Initialize()
        {
            InitializeEffortDatabase();
            InitializeMoq();
        }

        private void InitializeEffortDatabase()
        {
            DbConnection connection = Effort.EntityConnectionFactory.CreateTransient("name=BlockusEntities");
            _context = new BlockusEntities(connection);

            try
            {
                _context.Account.Add(new Account { Id_Account = 1, Username = "Test Account 1", Email = "ejemplo@gmail.com", AccountPassword = "1234" });
                _context.Account.Add(new Account { Id_Account = 2, Username = "Test Account 2", Email = "ejemplo@gmail.com", AccountPassword = "1234" });
                _context.SaveChanges();

                _context.ProfileConfiguration.Add(new ProfileConfiguration { Id_Configuration = 1, BoardStyle = 1, TilesStyle = 1, Id_Account = 1 });
                _context.ProfileConfiguration.Add(new ProfileConfiguration { Id_Configuration = 2, BoardStyle = 2, TilesStyle = 2, Id_Account = 2 });
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                ex.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors).ToList()
                    .ForEach(validationError => Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}"));

                throw;
            }
        }

        private void InitializeMoq()
        {
            _mockContext = new Mock<BlockusEntities>();
            _mockSet = new Mock<DbSet<ProfileConfiguration>>();

            var data = new List<ProfileConfiguration>
            {
                new ProfileConfiguration { Id_Configuration = 1, BoardStyle = 1, TilesStyle = 1, Id_Account = 1 },
                new ProfileConfiguration { Id_Configuration = 2, BoardStyle = 2, TilesStyle = 2, Id_Account = 2 }
            }.AsQueryable();

            _mockSet.As<IQueryable<ProfileConfiguration>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<ProfileConfiguration>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<ProfileConfiguration>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<ProfileConfiguration>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(c => c.ProfileConfiguration).Returns(_mockSet.Object);
        }

        [TestMethod]
        public void GetProfileConfiguration_ShouldReturnCorrectProfileConfiguration()
        {
            var dao = new ProfileConfigurationDao(_context);
            var result = dao.GetProfileConfiguration(2);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id_Configuration);
            Assert.AreEqual(2, result.BoardStyle);
            Assert.AreEqual(2, result.TilesStyle);
            Assert.AreEqual(2, result.Id_Account);
        }

        [TestMethod]
        public void GetProfileConfiguration_ShouldReturnDefaultWhenNotFound()
        {
            var dao = new ProfileConfigurationDao(_context);
            var result = dao.GetProfileConfiguration(99);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id_Configuration);
            Assert.AreEqual(0, result.BoardStyle);
            Assert.AreEqual(0, result.TilesStyle);
            Assert.AreEqual(0, result.Id_Account);
        }

        [TestMethod]
        public void GetProfileConfiguration_ShouldHandleGeneralException()
        {
            var dao = new ProfileConfigurationDao(_mockContext.Object);

            _mockContext.Setup(c => c.ProfileConfiguration).Throws(new Exception("Simulated General Exception"));

            var result = dao.GetProfileConfiguration(4);

            Assert.IsNotNull(result);
            Assert.AreEqual(-1, result.Id_Configuration);
            Assert.AreEqual(0, result.BoardStyle);
            Assert.AreEqual(0, result.TilesStyle);
            Assert.AreEqual(0, result.Id_Account);
        }

        [TestMethod]
        public void GetProfileConfiguration_ShouldHandleEntityException()
        {
            var dao = new ProfileConfigurationDao(_mockContext.Object);

            _mockContext.Setup(c => c.ProfileConfiguration).Throws(new EntityException("Simulated Entity Exception"));

            var result = dao.GetProfileConfiguration(2);

            Assert.IsNotNull(result);
            Assert.AreEqual(-1, result.Id_Configuration);
            Assert.AreEqual(0, result.BoardStyle);
            Assert.AreEqual(0, result.TilesStyle);
            Assert.AreEqual(0, result.Id_Account);
        }

    }
}
