﻿namespace YtDlpSharpLib.Options
{
    public partial class OptionSet
    {
        private Option<string> _username = new Option<string>("-u", "--username");
        private Option<string> _password = new Option<string>("-p", "--password");
        private Option<string> _twofactor = new Option<string>("-2", "--twofactor");
        private Option<bool> _netrc = new Option<bool>("-n", "--netrc");
        private Option<string> _netrcLocation = new Option<string>("--netrc-location");
        private Option<string> _videoPassword = new Option<string>("--video-password");
        private Option<string> _apMso = new Option<string>("--ap-mso");
        private Option<string> _apUsername = new Option<string>("--ap-username");
        private Option<string> _apPassword = new Option<string>("--ap-password");
        private Option<bool> _apListMso = new Option<bool>("--ap-list-mso");
        private Option<string> _clientCertificate = new Option<string>("--client-certificate");
        private Option<string> _clientCertificateKey = new Option<string>("--client-certificate-key");
        private Option<string> _ClientCertificatePassword = new Option<string>("--client-certificate-password");

        /// <summary>
        /// Login with this account ID
        /// </summary>
        public string Username { get => _username.Value; set => _username.Value = value; }
        /// <summary>
        /// Account password. If this option is left
        /// out, yt-dlp will ask interactively
        /// </summary>
        public string Password { get => _password.Value; set => _password.Value = value; }
        /// <summary>
        /// Two-factor authentication code
        /// </summary>
        public string TwoFactor { get => _twofactor.Value; set => _twofactor.Value = value; }
        /// <summary>
        /// Use .netrc authentication data
        /// </summary>
        public bool Netrc { get => _netrc.Value; set => _netrc.Value = value; }
        /// <summary>
        /// Location of .netrc authentication data;
        /// either the path or its containing directory.
        /// Defaults to ~/.netrc
        /// </summary>
        public string NetrcLocation { get => _netrcLocation.Value; set => _netrcLocation.Value = value; }
        /// <summary>
        /// Video password (vimeo, youku)
        /// </summary>
        public string VideoPassword { get => _videoPassword.Value; set => _videoPassword.Value = value; }
        /// <summary>
        /// Adobe Pass multiple-system operator (TV
        /// provider) identifier, use --ap-list-mso
        /// for a list of available MSOs
        /// </summary>
        public string ApMso { get => _apMso.Value; set => _apMso.Value = value; }
        /// <summary>
        /// Multiple-system operator account login
        /// </summary>
        public string ApUsername { get => _apUsername.Value; set => _apUsername.Value = value; }
        /// <summary>
        /// Multiple-system operator account
        /// password. If this option is left out,
        /// yt-dlp will ask interactively.
        /// </summary>
        public string ApPassword { get => _apPassword.Value; set => _apPassword.Value = value; }
        /// <summary>
        /// List all supported multiple-system
        /// operators
        /// </summary>
        public bool ApListMso { get => _apListMso.Value; set => _apListMso.Value = value; }
        /// <summary>
        /// Path to client certificate file in PEM
        /// format. May include the private key
        /// </summary>
        public string ClientCertificate { get => _clientCertificate.Value; set => _clientCertificate.Value = value; }
        /// <summary>
        /// Path to private key file for client
        /// certificate
        /// </summary>
        public string ClientCertificateKey { get => _clientCertificateKey.Value; set => _clientCertificateKey.Value = value; }
        /// <summary>
        /// Password for client certificate private key,
        /// if encrypted. If not provided, and the key
        /// is encrypted, yt-dlp will ask interactively
        /// </summary>
        public string ClientCertificatePassword { get => _ClientCertificatePassword.Value;  set => _ClientCertificatePassword.Value = value; }
    }
}
