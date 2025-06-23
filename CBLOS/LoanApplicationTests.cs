using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoanApplication2._0.Controllers;
using LoanApplicationSystem2._0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CBLOS
{
    [TestFixture]
    public class ClientAuthControllerTests
    {
        private ApplicationDbContext _dbContext;
        private ClientAuthController _controller;
        private DefaultHttpContext _httpContext;

        [SetUp]
        public void SetUp()
        {
            // Use EF Core InMemory provider for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Seed test data
            _dbContext.Clients.Add(new Client
            {
                ApplicationId = 1,
                Name = "Test User",
                Email = "test@example.com",
                Password = "pass123"
            });
            _dbContext.SaveChanges();

            _controller = new ClientAuthController(_dbContext);

            // Setup HttpContext with Session and Connection
            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new TestSession();
            _httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            };
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
            if (_controller is IDisposable disposableController)
            {
                disposableController.Dispose();
            }
        }

        [Test]
        public void LoginC_WithValidCredentials_RedirectsToDashboard()
        {
            // Act
            var result = _controller.LoginC("test@example.com", "pass123", false);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirect = (RedirectToActionResult)result;
            Assert.That(redirect.ActionName, Is.EqualTo("DashboardWithApply"));
            Assert.That(redirect.ControllerName, Is.EqualTo("Loan"));
            Assert.That(_httpContext.Session.GetString("ClientId"), Is.EqualTo("1"));
        }

        [Test]
        public void LoginC_WithInvalidCredentials_ReturnsViewWithError()
        {
            // Act
            var result = _controller.LoginC("test@example.com", "wrongpass", false);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(!_controller.ModelState.IsValid, Is.True);
            Assert.That(_controller.ModelState.Values.Any(v => v.Errors.Any(e => e.ErrorMessage.Contains("Invalid"))), Is.True);
        }

        [Test]
        public void LoginC_WithEmptyFields_ReturnsViewWithError()
        {
            // Act
            var result = _controller.LoginC("", "", false);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(!_controller.ModelState.IsValid, Is.True);
            Assert.That(_controller.ModelState.Values.Any(v => v.Errors.Any(e => e.ErrorMessage.Contains("required"))), Is.True);
        }

        // Helper: Fake ISession implementation for unit testing
        private class TestSession : ISession
        {
            private readonly Dictionary<string, byte[]> _sessionStorage = new();

            public IEnumerable<string> Keys => _sessionStorage.Keys;
            public string Id => Guid.NewGuid().ToString();
            public bool IsAvailable => true;
            public void Clear() => _sessionStorage.Clear();
            public void Remove(string key) => _sessionStorage.Remove(key);
            public void Set(string key, byte[] value) => _sessionStorage[key] = value;
            public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);

            public void SetString(string key, string value) => Set(key, System.Text.Encoding.UTF8.GetBytes(value));
            public string GetString(string key) => TryGetValue(key, out var data) ? System.Text.Encoding.UTF8.GetString(data) : null;

            // Not used in this test, but required by interface
            public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public void Load() { }
            public void Commit() { }
        }
    }
}