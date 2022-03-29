using Microsoft.AspNetCore.Mvc;
using EmulatorsRequester.Models;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using EmulatorsRequester.Util;

namespace EmulatorsRequester.Controllers
{
    public class ScovilleController : Controller
    {

        public IActionResult Index(Logs logs)
        {
            return View(logs);
        }

        [HttpPost]
        public IActionResult Index(Logs logs, string restart)
        {
            return View(logs);
        }

        public IActionResult AddSensors()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSensors(AddSensor sensors)
        {
            var client = new HttpClient();
            string url = "http://" + Common.LocalhostString() + "/api/sensor/" + sensors.SensorPrefix;

            var httpResponseMessage = await client.GetAsync(url);
            if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                if (!responseContent.Contains("Not Found"))
                {
                    sensors.ResponseStatus = "400";
                    sensors.JsonResponse = "Duplicated prefix";
                    return View(sensors);
                } 
                else
                {
                    url = "http://" + Common.LocalhostString() + ":8090/envirovue/start";
                    string bridgeBody = "";
                    string mobileBody = "";
                    if (!string.IsNullOrEmpty(sensors.BridgePrefix))
                    {
                        bridgeBody = @",
                            ""bridges"": {
                            ""url"": ""https://zb-device.zpc-dev.zebra.com:443/weblink/bridge/"",
                                ""count"": 1,
                                ""personality"": {
                                ""model"": ""ZB200"",
                                ""weblink-protocol"": ""v4.weblink.zebra.com""
                                },
                                ""prefix"": """ + sensors.BridgePrefix + @""",
                                ""min-start-delay"": 0,
                                ""max-start-delay"": 1
                            }";
                    }

                    if (!string.IsNullOrEmpty(sensors.MobilePrefix))
                    {
                        mobileBody = @",
                            ""mobile"": {
                            ""id_prefix"": """ + sensors.MobilePrefix + @""",
                                ""count"": 1
                            }";
                    }
                    
                    string body = @"{
                        ""envirovue"": {
                            ""enabled"": true,
                            ""auto-start"": true,
                            ""tenant-id"": """ + sensors.TenantId + @""",
                            ""auth-id"": """ + sensors.AuthId + @""",
                            ""ttl"": 6000,
                            ""web"": {
                            ""bearer"": """ + sensors.BearToken + @""",
                                ""origin"": ""https://zprintvillez-dev.zebra.com""
                            },
                            ""grpc"": {
                            ""host"": ""scv.zpc-dev.zebra.com"",
                                ""port"": 443,
                                ""plaintext"": false
                            },
                            ""sensors"": {
                            ""mac_prefix"": """ + sensors.SensorPrefix + @""",
                                ""count"": " + sensors.SensorCount + @" 
                            }" + bridgeBody + mobileBody + @"
                        }
                    }";
                    try
                    {
                        httpResponseMessage = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                        if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            sensors.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                            sensors.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            sensors.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                            sensors.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                        }
                        return View(sensors);
                    }
                    catch (Exception ex)
                    {
                        sensors.ResponseStatus = "500";
                        sensors.JsonResponse = "Emulator service error";
                        return View(sensors);
                    }
                }      
            }    
            else
            {
                sensors.ResponseStatus = "400";
                sensors.JsonResponse = "Duplicated prefix";
                return View(sensors);
            }
        }

        public IActionResult AddSamples()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSamples(AddSample samples, string GenerateSamples, string AddSamples)
        {
            if (!string.IsNullOrEmpty(GenerateSamples))
            {
                Random rnd = new Random();
                string sampleString = "";
                for (int i = 1; i < samples.RandomAmount; i++)
                {
                    int num = rnd.Next(samples.RandomRangeLow, samples.RandomRangeHigh);
                    sampleString = string.Format("{0}\n{1},", sampleString, num);
                }
                samples.Sample = sampleString.Substring(0, sampleString.Length - 1);
                return View(samples);
            }

            if (!string.IsNullOrEmpty(AddSamples))
            {
                string url = "http://" + Common.LocalhostString() + ":8090/envirovue/samples";
                var client = new HttpClient();
                string body = @"{
                    ""macAddress"": """ + samples.MacAddress + @""",
                    ""samples"": [" + samples.Sample + @"
                    ]
                }";
                try
                {
                    var httpResponseMessage = await client.PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        samples.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                        samples.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        samples.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                        samples.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    }
                    return View(samples);
                }
                catch (Exception ex)
                {
                    samples.ResponseStatus = "500";
                    samples.JsonResponse = "Emulator service error";
                    return View(samples);
                }                
            }
            return View(samples);
        }

        public IActionResult SetBattery()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetBattery(Battery battery)
        {
            string url = "http://"+Common.LocalhostString()+":8090/envirovue/battery";
            var client = new HttpClient();
            string body = @"{
                ""macAddress"": """ + battery.MacAddress + @""",
                ""battery"": " + battery.BatteryLevel + @"                
            }";
            try
            {
                var httpResponseMessage = await client.PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    battery.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                    battery.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                else
                {
                    battery.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                    battery.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                return View(battery);
            }
            catch (Exception ex)
            {
                battery.ResponseStatus = "500";
                battery.JsonResponse = "Emulator service error";
                return View(battery);
            }
        }

        public IActionResult GetBridgeLogs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetBridgeLogs(BridgeLogs bridgeLogs)
        {
            string url = "http://"+Common.LocalhostString()+":8090/envirovue/bridge/logs";
            var client = new HttpClient();

            url = url + "?id=" + bridgeLogs.Id + "&count=" + bridgeLogs.Count + "&offset=" + bridgeLogs.Offset;

            try
            {
                var httpResponseMessage = await client.GetAsync(url);
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    bridgeLogs.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                    string response = await httpResponseMessage.Content.ReadAsStringAsync();
                    // Parse base64 encoded logs
                    var json = JObject.Parse(response);
                    if (json.ContainsKey("logs"))
                    {
                        var jsonLogs = json["logs"];
                        foreach (JObject jsonLog in jsonLogs)
                        {
                            if (jsonLog.ContainsKey("command"))
                            {
                                JObject command = jsonLog.Value<JObject>("command");
                                if (command.ContainsKey("method"))
                                {
                                    if ((command.Value<string>("method")) == "datalogger.control_mission")
                                    {
                                        JObject logParams = command.Value<JObject>("params");
                                        if (logParams.Value<string>("step") == "start")
                                        {
                                            string configDescriptorBase64 = logParams.Value<string>("config_descriptor");
                                            string controlCharacteristicBase64 = logParams.Value<string>("control_characteristic");

                                            string configDescriptorString = Common.ConfigDescriptorDecode(configDescriptorBase64);
                                            response = response.Replace(configDescriptorBase64, configDescriptorString);

                                            string controllCharacteristicString = Common.ControlCharacteristicDecode(controlCharacteristicBase64);
                                            response = response.Replace(controlCharacteristicBase64, controllCharacteristicString);
                                        }
                                        else if (logParams.Value<string>("step") == "read")
                                        {
                                            string temperatureCharacteristicBase64 = logParams.Value<string>("temperature_characteristic");

                                            string tempearatureCharacteristicString = Common.TemperatureCharacteristicDecode(temperatureCharacteristicBase64);
                                            response = response.Replace(temperatureCharacteristicBase64, tempearatureCharacteristicString);
                                        }
                                        else if (logParams.Value<string>("step") == "stop")
                                        {
                                            string temperatureCharacteristicBase64 = logParams.Value<string>("temperature_characteristic");
                                            string controlCharacteristicBase64 = logParams.Value<string>("control_characteristic");

                                            string tempearatureCharacteristicString = Common.TemperatureCharacteristicDecode(temperatureCharacteristicBase64);
                                            response = response.Replace(temperatureCharacteristicBase64, tempearatureCharacteristicString);

                                            string controllCharacteristicString = Common.ControlCharacteristicDecode(controlCharacteristicBase64);
                                            response = response.Replace(controlCharacteristicBase64, controllCharacteristicString);
                                        }
                                    }
                                }
                            }
                        }

                        bridgeLogs.JsonResponse = response;
                    }
                    else
                    {
                        bridgeLogs.JsonResponse = response;
                    }
                }
                else
                {
                    bridgeLogs.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                    bridgeLogs.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                return View(bridgeLogs);
            }
            catch (Exception ex)
            {
                bridgeLogs.ResponseStatus = "500";
                bridgeLogs.JsonResponse = "Emulator service error";
                return View(bridgeLogs);
            }           
        }
        public IActionResult StopSensors()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StopSensors(StopSensor sensors)
        {
            string url = "http://"+Common.LocalhostString()+":8090/envirovue/stop";
            var client = new HttpClient();
            string body = "{}";
            url = url + "?id=" + sensors.Id;
            try
            {
                var httpResponseMessage = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    sensors.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                    sensors.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                else
                {
                    sensors.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                    sensors.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                return View(sensors);
            }
            catch (Exception ex)
            {
                sensors.ResponseStatus = "500";
                sensors.JsonResponse = "Emulator service error";
                return View(sensors);
            }
        }

        public IActionResult StartTask()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StartTask(StartTaskModel starttask)
        {
            string url = "http://"+Common.LocalhostString()+":8090/envirovue/start/task";
            var client = new HttpClient();
            string body = @"{
                ""macAddress"": """ + starttask.MacAddress + @""",
                ""dateTime"": """ + starttask.DateTime + @"""
            }";
            try
            {
                var httpResponseMessage = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    starttask.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                    starttask.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                else
                {
                    starttask.ResponseStatus = ((int)httpResponseMessage.StatusCode).ToString();
                    starttask.JsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                return View(starttask);
            }
            catch (Exception ex)
            {
                starttask.ResponseStatus = "500";
                starttask.JsonResponse = "Emulator service error";
                return View(starttask);
            }
        }
    }
}
