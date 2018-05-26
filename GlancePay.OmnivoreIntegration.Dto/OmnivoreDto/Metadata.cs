﻿using Newtonsoft.Json;

namespace GlancePay.OmnivoreIntegration.Dto
{
    public class Metadata
    {
        [JsonProperty("error")]
        public int GiftCardBalance { get; set; }

        [JsonProperty("pos_error")]
        public string PosError { get; set; }
    }
}