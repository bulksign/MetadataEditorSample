using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bulksign.Api;
using Integration;

namespace WebSignRedirectIntegration.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(string draftId, string email)
		{
			if (string.IsNullOrWhiteSpace(draftId))
			{
				return Content("Invalid request, the identifier is missing");
			}

			ViewBag.DraftId = draftId;
			ViewBag.Email = email;

			return View();
		}


		[HttpPost]
		public ActionResult SendEnvelope(string continent, string country, string city, string draftId, string email)
		{
			if (string.IsNullOrWhiteSpace(draftId))
			{
				return Content("Invalid request, the identifier is missing");
			}

			try
			{
				BulksignResult<SendEnvelopeResultApiModel> result = new BulksignIntegration().UpdateMetadataAndSendForSigning(email, draftId, city, continent, country);

				if (result.IsSuccessful)
				{
					//we've sent the envelope, so now redirect the user back to envelope details page
					//for on-premise set your Bulksign instance url
					return RedirectPermanent($"https://bulksign.com/EnvelopeDetail/Index/{draftId}");
				}

				return Content("Error");
			}
			catch (Exception ex)
			{
				//log here
				return Content("Error");
			}
		}
	}
}