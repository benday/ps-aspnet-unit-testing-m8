using Benday.Presidents.Api.Models;
using Benday.Presidents.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Benday.Presidents.Api;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Benday.Presidents.WebUi.TestData;
using Benday.Presidents.Api.DataAccess;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Benday.Presidents.WebUi;

namespace Benday.Presidents.WebUI.Controllers
{
    public class TestDataUtility : ITestDataUtility
    {
        private IPresidentService _Service;
        private PresidentsDbContext _DbContext;
        private UserManager<IdentityUser> _UserManager;
        private RoleManager<IdentityRole> _RoleManager;

        public TestDataUtility(IPresidentService service, PresidentsDbContext dbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            if (service == null)
                throw new ArgumentNullException("service", "service is null.");

            _Service = service;

            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext", "Argument cannot be null.");
            }

            _DbContext = dbContext;

            _UserManager = userManager;
            _RoleManager = roleManager;
        }

        public async Task CreatePresidentTestData()
        {
            var xml = TestDataResource.UsPresidentsXml;

            List<President> allPresidents = PopulatePresidentsFromXml(xml);

            DeleteAll();

            allPresidents.ForEach(x => _Service.Save(x));

            await InitializeSecurity();
        }

        public async Task VerifyDatabaseIsPopulated()
        {
            _DbContext.Database.EnsureCreated();

            var presidents = _Service.GetPresidents();

            if (presidents == null || presidents.Count == 0)
            {
                await CreatePresidentTestData();
            }
        }

        private List<President> PopulatePresidentsFromXml(string xml)
        {
            var returnValue = new List<President>();

            var root = XElement.Parse(xml);

            var presidents = root.ElementsByLocalName("president");

            President groverCleveland = null;

            foreach (var fromElement in presidents)
            {
                var currentPresident = GetPresidentFromXml(fromElement);

                if (currentPresident.LastName == "Cleveland")
                {
                    // grover cleveland had two non-consecutive terms
                    // only create one record for grover 
                    // with two terms
                    if (groverCleveland == null)
                    {
                        groverCleveland = currentPresident;
                        returnValue.Add(currentPresident);
                    }
                    else
                    {
                        groverCleveland.Terms.Add(currentPresident.Terms[0]);
                    }
                }
                else
                {
                    returnValue.Add(currentPresident);
                }
            }

            return returnValue;
        }

        private President GetPresidentFromXml(XElement fromValue)
        {
            President toValue = new President();

            toValue.BirthCity = fromValue.AttributeValue("birthcity");
            toValue.BirthState = fromValue.AttributeValue("birthstate");
            toValue.BirthDate = SafeToDateTime(fromValue.AttributeValue("birthdate"));

            toValue.DeathCity = fromValue.AttributeValue("deathcity");
            toValue.DeathState = fromValue.AttributeValue("deathstate");
            toValue.DeathDate = SafeToDateTime(fromValue.AttributeValue("deathdate"));

            toValue.FirstName = fromValue.AttributeValue("firstname");
            toValue.LastName = fromValue.AttributeValue("lastname");

            toValue.ImageFilename = fromValue.AttributeValue("image-filename");

            toValue.AddTerm(
                "President",
                SafeToDateTime(fromValue.AttributeValue("start")),
                SafeToDateTime(fromValue.AttributeValue("end")),
                SafeToInt32(fromValue.AttributeValue("id"))
                );

            return toValue;
        }

        private DateTime SafeToDateTime(string fromValue)
        {
            DateTime temp;

            if (DateTime.TryParse(fromValue, out temp) == true)
            {
                return temp;
            }
            else
            {
                return default(DateTime);
            }
        }

        private int SafeToInt32(string fromValue)
        {
            int temp;

            if (Int32.TryParse(fromValue, out temp) == true)
            {
                return temp;
            }
            else
            {
                return default(int);
            }
        }

        private void DeleteAll()
        {
            var allPresidents = _Service.GetPresidents();

            foreach (var item in allPresidents)
            {
                _Service.DeletePresidentById(item.Id);
            }
        }
        private async Task InitializeSecurity()
        {
            await DeleteAllRoles();
            await DeleteAllUsers();

            // create the roles
            await _RoleManager.CreateAsync(new IdentityRole(SecurityConstants.RoleName_Admin));
            await _RoleManager.CreateAsync(new IdentityRole(SecurityConstants.RoleName_User));

            // create users
            var admin = await CreateUser(SecurityConstants.Username_Admin);
            var user1 = await CreateUser(SecurityConstants.Username_User1);
            var user2 = await CreateUser(SecurityConstants.Username_User2);
            var user3 = await CreateUser(SecurityConstants.Username_Subscriber1);
            var user4 = await CreateUser(SecurityConstants.Username_Subscriber2);
        }

        private async Task<IdentityUser> CreateUser(string username)
        {
            var user = new IdentityUser();

            user.UserName = username;
            user.Email = username;
            user.EmailConfirmed = true;

            var result = await _UserManager.CreateAsync(user, SecurityConstants.DefaultPassword);

            if (result.Succeeded == false)
            {
                throw new InvalidOperationException("Error while creating user." + Environment.NewLine + result.Errors.ToString());
            }
            else
            {
                Console.WriteLine();
            }

            return user;
        }

        private async Task DeleteAllUsers()
        {
            var users = _UserManager.Users.ToList();

            foreach (var deleteThisUser in users)
            {
                await _UserManager.DeleteAsync(deleteThisUser);
            }
        }

        private async Task DeleteAllRoles()
        {
            var roles = _RoleManager.Roles.ToList();

            foreach (var deleteThis in roles)
            {
                await _RoleManager.DeleteAsync(deleteThis);
            }
        }
    }

}
