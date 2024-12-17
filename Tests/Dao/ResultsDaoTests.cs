using Data.Dao;
using Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;


namespace Tests.Dao
{
    [TestClass]
    public class ResultsDaoTests
    {
        private BlockusEntities _context;
        private Mock<BlockusEntities> _mockContext;
        private Mock<DbSet<Results>> _mockSet; 

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

                _context.Results.Add(new Results { Id_Result = 1, Victories = 3, Losses = 1, Id_Account = 1 });
                _context.Results.Add(new Results { Id_Result = 2, Victories = 5, Losses = 2, Id_Account = 2 });
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
            _mockSet = new Mock<DbSet<Results>>();

            var data = new List<Results>
            {
                new Results { Id_Result = 1, Victories = 3, Losses = 1, Id_Account = 1 }, 
                new Results { Id_Result = 2, Victories = 5, Losses = 2, Id_Account = 2 }
            }.AsQueryable();

            _mockSet.As<IQueryable<Results>>().Setup(m => m.Provider).Returns(data.Provider); 
            _mockSet.As<IQueryable<Results>>().Setup(m => m.Expression).Returns(data.Expression); 
            _mockSet.As<IQueryable<Results>>().Setup(m => m.ElementType).Returns(data.ElementType); 
            _mockSet.As<IQueryable<Results>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator()); 
            
            _mockContext.Setup(c => c.Results).Returns(_mockSet.Object);
        }

        [TestMethod]
        public void GetresultsByAccount_ShouldReturnCorrectResults()
        {
            var dao = new ResultsDao(_context); 
            var result = dao.GetResultsByAccount(1);

            Assert.AreEqual(1, result.Id_Result);
            Assert.AreEqual(3, result.Victories);
            Assert.AreEqual(1, result.Losses);
            Assert.AreEqual(1, result.Id_Account); 
        }

        [TestMethod]
        public void GetResultsByAccount_ShouldReturnDefaultWhenNotFound()
        {
            var dao = new ResultsDao(_context);
            var result = dao.GetResultsByAccount(99);

            Assert.AreEqual(0, result.Id_Result);
            Assert.AreEqual(0, result.Victories);
            Assert.AreEqual(0, result.Losses);
            Assert.AreEqual(0, result.Id_Account);
        }

        [TestMethod]
        public void GetResultsByAccount_ShouldHandleException()
        {
            var dao = new ResultsDao(_mockContext.Object);

            _mockContext.Setup(c => c.Results).Throws(new Exception("General Exception simulation"));

            var result = dao.GetResultsByAccount(1);

            Assert.AreEqual(-1, result.Id_Result); 
            Assert.AreEqual(0, result.Victories); 
            Assert.AreEqual(0, result.Losses); 
            Assert.AreEqual(0, result.Id_Account);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Dispose(); 
        }
    }
}
