namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<string> _proxy = new Option<string>("--proxy");
        private Option<int?> _socketTimeout = new Option<int?>("--socket-timeout");
        private Option<string> _sourceAddress = new Option<string>("--source-address");
        private Option<bool> _forceIpv4 = new Option<bool>("-4", "--force-ipv4");
        private Option<bool> _forceIpv6 = new Option<bool>("-6", "--force-ipv6");

        /// <summary>
        /// Use the specified HTTP/HTTPS/SOCKS
        /// proxy. To enable SOCKS proxy, specify a
        /// proper scheme. For example
        /// socks5://127.0.0.1:1080/. Pass in an
        /// empty string (--proxy "") for direct
        /// connection
        /// </summary>
        public string Proxy { get => _proxy.Value; set => _proxy.Value = value; }
        /// <summary>
        /// Time to wait before giving up, in
        /// seconds
        /// </summary>
        public int? SocketTimeout { get => _socketTimeout.Value; set => _socketTimeout.Value = value; }
        /// <summary>
        /// Client-side IP address to bind to
        /// </summary>
        public string SourceAddress { get => _sourceAddress.Value; set => _sourceAddress.Value = value; }
        /// <summary>
        /// Make all connections via IPv4
        /// </summary>
        public bool ForceIPv4 { get => _forceIpv4.Value; set => _forceIpv4.Value = value; }
        /// <summary>
        /// Make all connections via IPv6
        /// </summary>
        public bool ForceIPv6 { get => _forceIpv6.Value; set => _forceIpv6.Value = value; }
    }
}
