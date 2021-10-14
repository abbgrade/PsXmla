using System;
using System.Management.Automation;
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

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            switch (ParameterSetName)
            {
                case "PowerBi":
                    DataSource = $"powerbi://api.powerbi.com/v1.0/{PowerBiTenant}/{PowerBiWorkspace}";
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
                    break;

                case "Properties":
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
                    break;

                case "ConnectionString":
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

                        WriteVerbose("Connect by connection string.");
                    }
                    break;

                default:
                    throw new NotImplementedException($"ParameterSetName {ParameterSetName}");
            }

            {
                string protocol = null;
                var protocolRegex = new Regex(@"(\w+)://");
                var match = protocolRegex.Match(DataSource);
                if (match.Success)
                {
                    protocol = match.Groups[1].Value;
                }

                switch (protocol)
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
                        }
                        break;

                    default:
                        throw new NotSupportedException($"Protocol {protocol} is not supported");
                }
            }

            var connection = new Server();
            WriteVerbose($"Connect to { ConnectionString }");
            connection.Connect(ConnectionString);
            WriteObject(connection);
        }
    }
}
