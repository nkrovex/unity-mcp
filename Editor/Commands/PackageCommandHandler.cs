using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UnityMCP.Editor.Commands
{
    public class PackageCommandHandler
    {
        private const string PACKAGE_NAME = "com.justinpbarnett.unity-mcp";

        [CommandHandler("GET_PACKAGE_VERSION")]
        public static async Task<string> GetPackageVersion(string parameters)
        {
            try
            {
                var request = Client.List();
                while (!request.IsCompleted)
                {
                    await Task.Delay(100);
                }

                if (request.Status == StatusCode.Success)
                {
                    foreach (var package in request.Result)
                    {
                        if (package.name == PACKAGE_NAME)
                        {
                            var response = new Dictionary<string, string>
                            {
                                { "version", package.version },
                                { "packageName", package.name }
                            };
                            return JsonConvert.SerializeObject(response);
                        }
                    }
                    return JsonConvert.SerializeObject(new Dictionary<string, string>
                    {
                        { "error", $"Package {PACKAGE_NAME} not found" }
                    });
                }
                else
                {
                    return JsonConvert.SerializeObject(new Dictionary<string, string>
                    {
                        { "error", $"Failed to get package list: {request.Error.message}" }
                    });
                }
            }
            catch (System.Exception e)
            {
                return JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    { "error", $"Exception while getting package version: {e.Message}" }
                });
            }
        }
    }
} 