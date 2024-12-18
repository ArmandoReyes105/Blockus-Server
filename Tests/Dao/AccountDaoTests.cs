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
using System.Data.Entity.Core;
using Services.Dtos;

namespace Tests.Dao
{
    [TestClass]
    public class AccountDaoTests
    {
        private BlockusEntities _context; 
        private Mock<BlockusEntities> _mockContext;
        private Mock<DbSet<Account>> _mockSet;
        private Mock<DbSet<Results>> _mockSetResults;
        private Mock<DbSet<Friends>> _mockSetFriends; 

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
                _context.Account.Add(new Account { Id_Account = 3, Username = "Username3", Email = "ejemplo@gmail.com", AccountPassword = "1234" });
                _context.SaveChanges();

                _context.Results.Add(new Results { Id_Result = 1, Victories = 3, Losses = 1, Id_Account = 1 });
                _context.Results.Add(new Results { Id_Result = 2, Victories = 2, Losses = 2, Id_Account = 2 });
                _context.SaveChanges();

                _context.Friends.Add(new Friends { Id_Friend = 1, Id_Account = 1, Id_Account_Friend = 2 });
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
            _mockSetResults = new Mock<DbSet<Results>>();
            _mockSetFriends = new Mock<DbSet<Friends>>();

            var data = new List<Account>
            {
                new Account { Id_Account = 1, Username = "Jax", Email = "jax@gmail.com", AccountPassword = "ABCD1234" },
                new Account { Id_Account = 2, Username = "Abraham", Email = "ejemplo@gmail.com", AccountPassword = "1234" }
            }.AsQueryable();

            var resultsData = new List<Results>
            {
                new Results { Id_Result = 1, Victories = 3, Losses = 1, Id_Account = 1 },
                new Results { Id_Result = 2, Victories = 2, Losses = 2, Id_Account = 2 }

            }.AsQueryable();

            var dataFriends = new List<Friends>
            {
                new Friends { Id_Friend = 1, Id_Account = 1, Id_Account_Friend = 2 }
            }.AsQueryable();

            _mockSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockSetResults.As<IQueryable<Results>>().Setup(m => m.Provider).Returns(resultsData.Provider);
            _mockSetResults.As<IQueryable<Results>>().Setup(m => m.Expression).Returns(resultsData.Expression);
            _mockSetResults.As<IQueryable<Results>>().Setup(m => m.ElementType).Returns(resultsData.ElementType);
            _mockSetResults.As<IQueryable<Results>>().Setup(m => m.GetEnumerator()).Returns(resultsData.GetEnumerator());

            _mockSetResults.As<IQueryable<Friends>>().Setup(m => m.Provider).Returns(dataFriends.Provider);
            _mockSetResults.As<IQueryable<Friends>>().Setup(m => m.Expression).Returns(dataFriends.Expression);
            _mockSetResults.As<IQueryable<Friends>>().Setup(m => m.ElementType).Returns(dataFriends.ElementType);
            _mockSetResults.As<IQueryable<Friends>>().Setup(m => m.GetEnumerator()).Returns(dataFriends.GetEnumerator());

            _mockContext.Setup(c => c.Account).Returns(_mockSet.Object);
            _mockContext.Setup(c => c.Results).Returns(_mockSetResults.Object);
            _mockContext.Setup(c => c.Friends).Returns(_mockSetFriends.Object); 
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


        //IncreaseVictories
        [TestMethod]
        public void IncreaseVictories_ShouldIncreaseVictoriesSuccessfully() 
        { 
            var dao = new AccountDao(_context); 
            var initialVictories = _context.Results.First(r => r.Id_Account == 1).Victories;
            
            var result = dao.IncreaseVictories(1); 
            var updatedVictories = _context.Results.First(r => r.Id_Account == 1).Victories;
            
            Assert.AreEqual(1, result); 
            Assert.AreEqual(initialVictories + 1, updatedVictories); 
        }

        [TestMethod]
        public void IncreaseVictories_ShouldReturnMinusOneWhenAccountNotFound()
        {
            var dao = new AccountDao(_context); 
            var result = dao.IncreaseVictories(99); 
            
            Assert.AreEqual(-1, result); 
        }

        [TestMethod]
        public void IncreaseVictories_ShouldReturnMinusOneOnException()
        { 
            var dao = new AccountDao(_mockContext.Object); 
            
            _mockContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated Exception"));
            
            var result = dao.IncreaseVictories(1); 
            
            Assert.AreEqual(-1, result); 
        }

        [TestMethod]
        public void IncreaseVictories_ShouldReturnZeroWhenNoChange()
        {
            var dao = new AccountDao(_mockContext.Object);  
            
            _mockContext.Setup(c => c.SaveChanges()).Returns(0); 

            var result = dao.IncreaseVictories(1); 
            
            Assert.AreEqual(0, result);
        }


        //IncreaseLosses
        [TestMethod]
        public void IncreaseLosses_ShouldIncreaseLossesSuccesfully()
        {
            var dao = new AccountDao(_context); 
            var initialLosses = _context.Results.First(r => r.Id_Account == 1).Losses; 
            
            var result = dao.IncreaseLosses(1); 
            var updatedLosses = _context.Results.First(r => r.Id_Account == 1).Losses; 
            
            Assert.AreEqual(1, result); 
            Assert.AreEqual(initialLosses + 1, updatedLosses);
        }

        [TestMethod]
        public void IncreaseLosses_ShouldReturnMinusOneWhenAccountNotFound()
        {
            var dao = new AccountDao(_context); 
            
            var result = dao.IncreaseLosses(99); 
            
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void IncreaseLosses_ShouldReturnZeroWhenNoChange()
        {
            var dao = new AccountDao(_mockContext.Object); 

            _mockContext.Setup(c => c.SaveChanges()).Returns(0); 
            
            var result = dao.IncreaseLosses(1); 
            
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void IncreaseLosses_ShouldReturnMinusOneOnException()
        {
            var dao = new AccountDao(_mockContext.Object); 
            
            _mockContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated Exception")); 
            
            var result = dao.IncreaseLosses(1); 
            
            Assert.AreEqual(-1, result);
        }


        //UpdateAccount
        [TestMethod]
        public void UpdateAccount_ShouldReturnSuccessCode()
        {
            var account = new Account { Id_Account = 1, Username = "ArmandoUpdated", Email = "armando_updated@gmail.com", AccountPassword = "abcd1234", ProfileImage = 1 }; 
            var dao = new AccountDao(_context); 
            
            var result = dao.UpdateAccount(account); 
            var updatedAccount = _context.Account.FirstOrDefault(a => a.Id_Account == account.Id_Account); 
            
            Assert.AreEqual(1, result); 
            Assert.IsNotNull(updatedAccount); 
            Assert.AreEqual("ArmandoUpdated", updatedAccount.Username); 
            Assert.AreEqual("abcd1234", updatedAccount.AccountPassword); 
            Assert.AreEqual(1, updatedAccount.ProfileImage);
        }

        [TestMethod]
        public void UpdateAccount_ShouldReturnZeroWhenAccountNotFound()
        {
            var account = new Account { Id_Account = 99, Username = "NonExistent", Email = "nonexistent@gmail.com", AccountPassword = "password", ProfileImage = 0 }; 
            var dao = new AccountDao(_context); 
            
            var result = dao.UpdateAccount(account); 
            
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void UpdateAccount_ShouldHandleEntityException()
        {
            var account = new Account { Id_Account = 1, Username = "ArmandoUpdated", Email = "armando_updated@gmail.com", AccountPassword = "abcd1234", ProfileImage = 1 }; 
            var dao = new AccountDao(_mockContext.Object); 
            
            _mockContext.Setup(c => c.SaveChanges()).Throws(new EntityException("Simulated EntityException")); 
            
            var result = dao.UpdateAccount(account); 
            
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void UpdateAccount_ShouldHandleGeneralException()
        {
            var account = new Account { Id_Account = 1, Username = "ArmandoUpdated", Email = "armando_updated@gmail.com", AccountPassword = "abcd1234", ProfileImage = 1 }; 
            var dao = new AccountDao(_mockContext.Object); 
            
            _mockContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated General Exception")); 
            
            var result = dao.UpdateAccount(account); 
            
            Assert.AreEqual(-1, result);
        }


        //AddFriend
        [TestMethod]
        public void AddFriend_ShouldReturnSuccess()
        {
            var dao = new AccountDao(_context);

            var result = dao.AddFriend(1, 3); 
            var newFriendship = _context.Friends.FirstOrDefault(f => f.Id_Account == 1 && f.Id_Account_Friend == 3); 
            
            Assert.AreEqual(1, result); 
            Assert.IsNotNull(newFriendship);
        }

        [TestMethod]
        public void AddFriend_ShouldReturnZeroWhenAlreadyFriends()
        {
            var dao = new AccountDao(_context); 

            var result = dao.AddFriend(1, 2); 
            
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void AddFriend_ShouldReturnMinusTwoWhenSelfFriendRequest()
        {
            var dao = new AccountDao(_context); 
            
            var result = dao.AddFriend(1, 1); 
            
            Assert.AreEqual(-2, result);
        }

        [TestMethod]
        public void AddFriend_ShouldHandleEntityException()
        {
            var dao = new AccountDao(_mockContext.Object); 
            
            _mockContext.Setup(c => c.SaveChanges()).Throws(new EntityException("Simulated EntityException")); 
            
            var result = dao.AddFriend(1, 3); 
            
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void AddFriend_ShouldHandleGeneralException()
        {
            var dao = new AccountDao(_mockContext.Object); 
            
            _mockContext.Setup(c => c.SaveChanges()).Throws(new Exception("Simulated General Exception")); 
            
            var result = dao.AddFriend(1, 3); 
            
            Assert.AreEqual(-1, result);
        }


        //GetAccountFriends
        [TestMethod]
        public void GetAccountFriends_ShouldReturnFriendsList()
        {
            var dao = new AccountDao(_context); 
            
            var result = dao.GetAccountFriends(1); 
            
            Assert.IsNotNull(result); 
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetAccountFriends_ShouldReturnEmptyListWhenNoFriends()
        {
            var dao = new AccountDao(_context); 
            
            var result = dao.GetAccountFriends(99); 
            
            Assert.IsNotNull(result); 
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAccountFriends_ShouldHandleEntityException()
        {
            var dao = new AccountDao(_mockContext.Object); 
            
            _mockContext.Setup(c => c.Friends).Throws(new EntityException("Simulated EntityException")); 
            
            var friends = dao.GetAccountFriends(1); 
            
            Assert.IsNull(friends);
        }

        [TestMethod]
        public void GetAccountFriends_ShouldHandleGeneralException()
        {
            var dao = new AccountDao(_mockContext.Object);

            _mockContext.Setup(c => c.Friends).Throws(new Exception("Simulated General Exception"));

            var friends = dao.GetAccountFriends(1); 
            
            Assert.IsNull(friends);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Dispose(); 
        }
    }
}
