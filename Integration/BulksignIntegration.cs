using System;
using System.IO;
using Bulksign.Api;
using Newtonsoft.Json;

namespace Integration
{
	public class BulksignIntegration
	{
		private const string BulksignOrganizationToken = "";


		public BulksignResult<SendEnvelopeResultApiModel> UpdateMetadataAndSendForSigning(string senderEmail, string draftId, string city, string continent, string country)
		{
			//prepare metadata

			MetadataType mt = new MetadataType();
			mt.City = city;
			mt.Continent = continent;
			mt.Country = country;

			string metadata = JsonConvert.SerializeObject(mt);



			//specify the integration url for on-premise version of Bulksign, leave empty to target bulksign.com
			BulkSignApi api = new BulkSignApi();

			UpdateDraftApiModel draftSettings = new UpdateDraftApiModel();
			draftSettings.DraftId = draftId;
			draftSettings.Metadata = metadata;



			AuthenticationApiModel auth = new AuthenticationApiModel();
			auth.UserEmail = senderEmail;
			auth.Token = BulksignOrganizationToken;

			BulksignResult<string> updateResult = api.UpdateDraftSettings(auth, draftSettings);

			if (updateResult.IsSuccessful == false)
			{
				throw new Exception(updateResult.ErrorMessage);
			}

			return api.SendEnvelopeFromDraft(auth, draftId);
		}

	}
}