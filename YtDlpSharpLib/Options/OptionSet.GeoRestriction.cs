namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<string> _geoVerificationProxy = new Option<string>("--geo-verification-proxy");
        private Option<bool> _geoBypass = new Option<bool>("--geo-bypass");
        private Option<bool> _noGeoBypass = new Option<bool>("--no-geo-bypass");
        private Option<string> _geoBypassCountry = new Option<string>("--geo-bypass-country");
        private Option<string> _geoBypassIpBlock = new Option<string>("--geo-bypass-ip-block");

        /// <summary>
        /// Use this proxy to verify the IP address
        /// for some geo-restricted sites. The
        /// default proxy specified by --proxy (or
        /// none, if the option is not present) is
        /// used for the actual downloading.
        /// </summary>
        public string GeoVerificationProxy { get => _geoVerificationProxy.Value; set => _geoVerificationProxy.Value = value; }
        /// <summary>
        /// Bypass geographic restriction via
        /// faking X-Forwarded-For HTTP header (default)
        /// </summary>
        public bool GeoBypass { get => _geoBypass.Value; set => _geoBypass.Value = value; }
        /// <summary>
        /// Do not bypass geographic restriction
        /// via faking X-Forwarded-For HTTP header
        /// </summary>
        public bool NoGeoBypass { get => _noGeoBypass.Value; set => _noGeoBypass.Value = value; }
        /// <summary>
        /// Force bypass geographic restriction
        /// with explicitly provided two-letter ISO
        /// 3166-2 country code
        /// </summary>
        public string GeoBypassCountry { get => _geoBypassCountry.Value; set => _geoBypassCountry.Value = value; }
        /// <summary>
        /// Force bypass geographic restriction
        /// with explicitly provided IP block in
        /// CIDR notation
        /// </summary>
        public string GeoBypassIpBlock { get => _geoBypassIpBlock.Value; set => _geoBypassIpBlock.Value = value; }
    }
}
