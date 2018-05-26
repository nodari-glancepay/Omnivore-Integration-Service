using Newtonsoft.Json;

namespace GlancePay.OmnivoreIntegration.Dto
{
    public class Embedded
    {
        [JsonProperty("tender_type")]
        public TenderType TenderType { get; set; }
        [JsonProperty("ticket")]
        public Ticket Ticket { get; set; }
    }
}
