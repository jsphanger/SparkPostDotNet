namespace SparkPostDotNet
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Suppressions;
    using Transmissions;

    public class SparkPostClient
    {
        private const string DefaultUri = "https://api.sparkpost.com/api/v1";

        protected SparkPostOptions Options { get; }

        public SparkPostClient(IOptions<SparkPostOptions> options)
        {
            this.Options = options.Value;
        }

        public async Task CreateTransmission(Transmission transmission)
        {
            if (string.IsNullOrEmpty(this.Options.ApiKey))
            {
                throw new InvalidOperationException("Configuration variable SparkPost:ApiKey must be set.");
            }

            Uri SparkPostUri = new Uri(string.IsNullOrEmpty(this.Options.ApiHostUri) ? DefaultUri + "/transmissions" : this.Options.ApiHostUri + "/transmissions");

            using (var httpClient = new HttpClient())
            {
                transmission.Headers.Clear();
                transmission.Headers.Add("Content-Type", "application/json");
                httpClient.DefaultRequestHeaders.Add("Authorization", this.Options.ApiKey);
                var response = await httpClient.PostAsync(SparkPostUri, transmission);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
        }

        /// <summary>
        /// Creates or updates a suppression record in the general suppressions list.
        /// </summary>
        /// <param name="suppression">A Suppression object containing recipient email, type (transactional or non_transactional), and description.</param>
        public async Task UpdateSuppressionAsync(Suppression suppression)
        {
            if (string.IsNullOrEmpty(this.Options.ApiKey))
            {
                throw new InvalidOperationException("Configuration variable SparkPost:ApiKey must be set.");
            }

            if (string.IsNullOrEmpty(suppression.RecipientEmail))
            {
                throw new InvalidOperationException("Suppression variable RecipientEmail must be set.");
            }

            Uri SparkPostUri = new Uri(string.IsNullOrEmpty(this.Options.ApiHostUri) ? DefaultUri + "/suppression-list/" + suppression.RecipientEmail : this.Options.ApiHostUri + "/suppression-list/" + suppression.RecipientEmail);

            using (var httpClient = new HttpClient())
            {
                suppression.Headers.Clear();
                suppression.Headers.Add("Content-Type", "application/json");
                httpClient.DefaultRequestHeaders.Add("Authorization", this.Options.ApiKey);
                var response = await httpClient.PutAsync(SparkPostUri, suppression);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
        }

        /// <summary>
        /// Removes a suppression record from the general suppressions list.
        /// </summary>
        /// <param name="suppression">A Suppression object containing the recipient email.</param>
        public async Task RemoveSuppressionAsync(Suppression suppression)
        {
            if (string.IsNullOrEmpty(this.Options.ApiKey))
            {
                throw new InvalidOperationException("Configuration variable SparkPost:ApiKey must be set.");
            }

            if (string.IsNullOrEmpty(suppression.RecipientEmail))
            {
                throw new InvalidOperationException("Suppression variable RecipientEmail must be set.");
            }

            Uri SparkPostUri = new Uri(string.IsNullOrEmpty(this.Options.ApiHostUri) ? DefaultUri + "/suppression-list/" + suppression.RecipientEmail : this.Options.ApiHostUri + "/suppression-list/" + suppression.RecipientEmail);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", this.Options.ApiKey);
                var response = await httpClient.DeleteAsync(SparkPostUri);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
        }
    }
}