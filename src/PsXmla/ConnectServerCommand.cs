using System;
using System.Management.Automation;
using System.Net;
using System.Security;
using System.Text.RegularExpressions;
using Microsoft.AnalysisServices.Tabular;
using Microsoft.Azure.Services.AppAuthentication;

namespace PsXmla
{
    [Cmdlet(VerbsCommunications.Connect, "Server")]
    [OutputType(typeof(Server))]
    public class ConnectServerCommand : PSCmdlet
    {
        [Parameter(
            ParameterSetName = "ConnectionString",
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string ConnectionString { get; set; }

        [Parameter(
            ParameterSetName = "Properties",
            Position = 0,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true
        )]
        [ValidateNotNullOrEmpty()]
        [Alias("Server", "ServerName", "ServerInstance")]
        public string DataSource { get; set; }

        [Parameter(
            ParameterSetName = "Properties",
            Position = 1,
            Mandatory = false,
            ValueFromPipelineByPropertyName = true
        )]
        [ValidateNotNullOrEmpty()]
        [Alias("Database", "DatabaseName")]
        public string InitialCatalog { get; set; }

        [Parameter(
            ParameterSetName = "PowerBi",
            Position = 0,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true
        )]
        [ValidateNotNullOrEmpty()]
        public string PowerBiWorkspace { get; set; }

        [Parameter(
            ParameterSetName = "PowerBi",
            Position = 1,
            Mandatory = false,
            ValueFromPipelineByPropertyName = true
        )]
        [ValidateNotNullOrEmpty()]
        public string PowerBiDataset { get; set; }

        [Parameter(
            ParameterSetName = "PowerBi",
            Position = 2,
            Mandatory = false,
            ValueFromPipelineByPropertyName = true
        )]
        [ValidateNotNullOrEmpty()]
        public string PowerBiTenant { get; set; } = "MyOrg";

        [Parameter()]
        public string AccessToken { get; set; }

        [Parameter()]
        public SwitchParameter InteractiveAuthentication;

        [Parameter()]
        public string Username {
            get { return Credential.UserName; } 
            set { Credential.UserName = value; } 
        }

        [Parameter()]
        public SecureString Password
        {
            get { return Credential.SecurePassword; }
            set { Credential.SecurePassword = value; }
        }

        [Parameter()]
        public NetworkCredential Credential { get; set; }

        string Protocol { get; set; }

        private Server Connection { get; set; }  = new Server();

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            switch (ParameterSetName)
            {
                case "PowerBi":
                    SetDataSourceForPowerBiParameters();
                    SetConnectionStringByPowerBiParameters();
                    break;

                case "Properties":
                    SetConnectionStringByServerParameters();
                    break;

                case "ConnectionString":
                    SetDataSourceByConnectionString();
                    break;

                default:
                    throw new NotImplementedException($"ParameterSetName {ParameterSetName}");
            }

            SetProtocolByDataSource();
            AddAuthenticationToConnectionString();
            ConnectByConnectionString();
            WriteObject(Connection);
        }

        private void ConnectByConnectionString()
        {
            Connection.Connect(ConnectionString);
        }

        private void AddAuthenticationToConnectionString()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                ConnectionString += $";Username={Username}";

                if (Password.Length > 0)
                {
                    ConnectionString += $";Password={new NetworkCredential(string.Empty, Password).Password}";
                    WriteVerbose("Authenticate via Username and Password");
                } else
                {
                    WriteVerbose("Authenticate via Username");
                }
            }
            else
            {
                switch (Protocol)
                {
                    case "powerbi":
                        if (!InteractiveAuthentication.IsPresent)
                        {
                            if (AccessToken == null)
                            {
                                WriteVerbose("Try to get AccessToken");
                                AccessToken = new AzureServiceTokenProvider().GetAccessTokenAsync("https://analysis.windows.net/powerbi/api.default").Result;
                            }
                            ConnectionString += $";Password={AccessToken}";
                            WriteVerbose("Authenticate via AccessToken");
                        }
                        break;

                    default:
                        throw new NotSupportedException($"Protocol {Protocol} is not supported");
                }
            }
        }

        private void SetProtocolByDataSource()
        {
            var protocolRegex = new Regex(@"(\w+)://");
            var match = protocolRegex.Match(DataSource);
            if (match.Success)
            {
                Protocol = match.Groups[1].Value;
            }
        }

        private void SetDataSourceByConnectionString()
        {
            var dataSourceRegex = new Regex(@"Data.?Source=""?(.+)""?;?");
            var match = dataSourceRegex.Match(ConnectionString);
            if (match.Success)
            {
                DataSource = match.Groups[1].Value;
                WriteVerbose($"ConnectionString contains DataSource {DataSource}");
            }
            else
            {
                WriteWarning($"ConnectionString lacks a DataSource");
            }
        }

        private void SetConnectionStringByServerParameters()
        {
            ConnectionString = $"DataSource={DataSource}";
            if (!string.IsNullOrEmpty(InitialCatalog))
            {
                ConnectionString += $";initial catalog={InitialCatalog}";
                WriteVerbose("Connect by DataSource and InitialCatalog.");
            }
            else
            {

                WriteVerbose("Connect by DataSource.");
            }
        }

        private void SetConnectionStringByPowerBiParameters()
        {
            ConnectionString = $"DataSource={DataSource}";
            if (!string.IsNullOrEmpty(PowerBiDataset))
            {
                ConnectionString += $";initial catalog={PowerBiDataset}";
                WriteVerbose("Connect by PowerBiWorkspace and PowerBiDataset.");
            }
            else
            {

                WriteVerbose("Connect by PowerBiWorkspace.");
            }
        }

        private void SetDataSourceForPowerBiParameters()
        {
            DataSource = $"powerbi://api.powerbi.com/v1.0/{PowerBiTenant}/{PowerBiWorkspace}";
        }
    }
}
