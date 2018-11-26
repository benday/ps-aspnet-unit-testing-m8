using Benday.Presidents.Common;
using Benday.Presidents.Api;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Benday.Presidents.WebUi;

namespace Benday.Presidents.WebUI.Controllers
{

    // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
    public class PresidentController : Controller
    {
        private const int ID_FOR_CREATE_NEW_PRESIDENT = 0;
        private IPresidentService _Service;
        private IValidatorStrategy<President> _Validator;
        private ITestDataUtility _TestDataUtility;

        public PresidentController(IPresidentService service,
            IValidatorStrategy<President> validator,
            ITestDataUtility testDataUtility
            )
        {
            if (service == null)
                throw new ArgumentNullException("service", "service is null.");

            if (validator == null)
            {
                throw new ArgumentNullException("validator", "Argument cannot be null.");
            }

            _Validator = validator;
            _Service = service;
            _TestDataUtility = testDataUtility;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var presidents = _Service.GetPresidents();

            return View(presidents);
        }

        [AllowAnonymous]
        [Route("/[controller]/[action]/{id}")]
        [Route("/president/{id}.aspx")]
        public ActionResult Details(int? id)
        {
            if (id == null || id.HasValue == false)
            {
                return new BadRequestResult();
            }

            var president = _Service.GetPresidentById(id.Value);

            if (president == null)
            {
                return NotFound();
            }

            return View(president);
        }

        [Route("/president/{last:alpha}/{first:alpha}")]
        public ActionResult Details(string last, string first)
        {
            if (String.IsNullOrWhiteSpace(last) == true ||
                String.IsNullOrWhiteSpace(first) == true)
            {
                return new BadRequestResult();
            }

            var president = _Service.Search(
                first, last).FirstOrDefault();

            if (president == null)
            {
                return NotFound();
            }

            return View("Details", president);
        }

        public ActionResult Create()
        {
            return RedirectToAction("Edit", new { id = ID_FOR_CREATE_NEW_PRESIDENT });
        }

        // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
        [Authorize(Policy = SecurityConstants.PolicyName_EditPresident)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            President president;

            if (id.Value == ID_FOR_CREATE_NEW_PRESIDENT)
            {
                // create new
                president = new President();
                president.AddTerm(PresidentsConstants.President,
                    default(DateTime),
                    default(DateTime), 0);
            }
            else
            {
                president = _Service.GetPresidentById(id.Value);
            }

            if (president == null)
            {
                return NotFound();
            }

            return View(president);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Roles = SecurityConstants.RoleName_Admin)]
        [Authorize(Policy = SecurityConstants.PolicyName_EditPresident)]
        public ActionResult Edit(President president)
        {
            if (_Validator.IsValid(president) == true)
            {
                bool isCreateNew = false;

                if (president.Id == ID_FOR_CREATE_NEW_PRESIDENT)
                {
                    isCreateNew = true;
                }
                else
                {
                    President toValue =
                        _Service.GetPresidentById(president.Id);

                    if (toValue == null)
                    {
                        return new BadRequestObjectResult(
                            String.Format("Unknown president id '{0}'.", president.Id));
                    }
                }

                _Service.Save(president);

                if (isCreateNew == true)
                {
                    RedirectToAction("Edit", new { id = president.Id });
                }
                else
                {
                    return RedirectToAction("Edit");
                }
            }

            return View(president);
        }

        //[AllowAnonymous]
        public async Task<ActionResult> ResetDatabase()
        {
            await _TestDataUtility.CreatePresidentTestData();

            return RedirectToAction("Index");
        }

        //[AllowAnonymous]
        public ActionResult VerifyDatabaseIsPopulated()
        {
            _TestDataUtility.VerifyDatabaseIsPopulated();

            return RedirectToAction("Index");
        }
    }
}