using Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System;
using System.Collections.Generic;
using Data.Dao;
using System.Data.SqlClient;

namespace Tests.Dao
{
    [TestClass]
    public class AccountDaoTests
    {
        private BlockusEntities _context; 
        private Mock<BlockusEntities> _mockContext;
        private Mock<DbSet<Account>> _mockSet;

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
                _context.Account.Add(new Account { Id_Account = 1, Username = "Jax", Email = "jax@gmail.com", AccountPassword = "ABCD1234" });
                _context.Account.Add(new Account { Id_Account = 2, Username = "Abraham", Email = "ejemplo@gmail.com", AccountPassword = "1234" });
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
            _mockSet = new Mock<DbSet<Account>>();

            var data = new List<Account>
            {
                new Account { Id_Account = 1, Username = "Jax", Email = "jax@gmail.com", AccountPassword = "ABCD1234" },
                new Account { Id_Account = 2, Username = "Abraham", Email = "ejemplo@gmail.com", AccountPassword = "1234" }
            }.AsQueryable();

            _mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(c => c.Account).Returns(_mockSet.Object);
        }

        [TestMethod]
        public void CreateAccount_ShouldReturnSuccess()
        {
            var account = new Account { Username = "Cinthia", Email = "Ejemplo@gmail.com", AccountPassword = "h21saf54" };
            var dao = new AccountDao(_context);
            
            var result = dao.CreateAccount(account);
            var createdAccount = _context.Account.FirstOrDefault(a => a.Username == account.Username);
            var createdConfig = _context.ProfileConfiguration.FirstOrDefault(c => c.Id_Account == createdAccount.Id_Account);
            var createdResults = _context.Results.FirstOrDefault(r => r.Id_Account == createdAccount.Id_Account);

            Assert.AreEqual(1, result);
            Assert.IsNotNull(createdAccount);
            Assert.IsNotNull(createdConfig);
            Assert.IsNotNull(createdResults);
        }

        [TestMethod]
        public void CreateAccount_ShouldReturnValidationError()
        {
            var account = new Account { Username = null, Email = null, AccountPassword = null };
            var dao = new AccountDao(_context);

            var result = dao.CreateAccount(account);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CreateAccount_ShouldHandleException()
        {
            var account = new Account { Username = "Cinthia", Email = "Ejemplo@gmail.com", AccountPassword = "h21saf54" };
            var dao = new AccountDao(_mockContext.Object);

            _mockContext.Setup(c => c.Account.Add(It.IsAny<Account>())).Throws(new Exception("Simulated Exception"));

            var result = dao.CreateAccount(account);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CreateAccount_ShouldRollbackTransactionOnValidationError()
        {
            var account = new Account { Username = "Cinthia", Email = "Ejemplo@gmail.com", AccountPassword = "h21saf54" };
            var dao = new AccountDao(_mockContext.Object);

            _mockContext.Setup(c => c.SaveChanges()).Throws(new DbEntityValidationException()); 

            var result = dao.CreateAccount(account);

            Assert.AreEqual(0, result);
            _mockSet.Verify(m => m.Add(It.IsAny<Account>()), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once); 
        }

        //Login Method
        [TestMethod]
        public void Login_ShouldReturnAccountWhenValidCredentials()
        {
            var dao = new AccountDao(_context);

            var result = dao.Login("Jax", "ABCD1234");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id_Account);
            Assert.AreEqual("Jax", result.Username);
            Assert.AreEqual("jax@gmail.com", result.Email); 
        }

        [TestMethod]
        public void Login_ShouldReturnDefaultWhenInvalidCredentials()
        {
            var dao = new AccountDao(_context);

            var result = dao.Login("Jax", "invalidPassword"); 

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id_Account); 
        }

        [TestMethod]
        public void Login_ShouldReturnAccountWhenUsingEmail()
        {
            var dao = new AccountDao(_context);

            var result = dao.Login("jax@gmail.com", "ABCD1234"); 

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id_Account);
            Assert.AreEqual("Jax", result.Username);
            Assert.AreEqual("jax@gmail.com", result.Email);
        }

        [TestMethod]
        public void Login_ShouldHandleGeneralException()
        {
            var dao = new AccountDao(_mockContext.Object);

            _mockContext.Setup(x => x.Account).Throws(new Exception("Simulated Exception"));

            var result = dao.Login("Jax", "ABCD1234");

            Assert.IsNotNull(result);
            Assert.AreEqual(-1, result.Id_Account); 
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Dispose(); 
        }
    }
}
