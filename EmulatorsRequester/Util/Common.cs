using EmulatorsRequester.Models;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EmulatorsRequester.Util
{
    public static class Common
    {
        public static string ConfigDescriptorDecode(string base64String)
        {
            // base64 decode
            var configDescriptorEncodedByte = Convert.FromBase64String(base64String);
            // parse object
            DateTime taskConfiguredTime = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            uint taskConfiguredTimeSecond = (uint)((configDescriptorEncodedByte[7]) << 24
                | (configDescriptorEncodedByte[6] << 16)
                | (configDescriptorEncodedByte[5] << 8)
                | (configDescriptorEncodedByte[4]));
            taskConfiguredTime = taskConfiguredTime.AddSeconds(taskConfiguredTimeSecond);
            DateTime delayStartTime = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            uint delayStartTimeSecond = (uint)((configDescriptorEncodedByte[11]) << 24
                | (configDescriptorEncodedByte[10] << 16)
                | (configDescriptorEncodedByte[9] << 8)
                | (configDescriptorEncodedByte[8]));
            delayStartTime = delayStartTime.AddSeconds(delayStartTimeSecond);

            ConfigDescriptor configDescriptor = new ConfigDescriptor
            {
                ReadInterval = (uint)((configDescriptorEncodedByte[1] << 8) | configDescriptorEncodedByte[0]),
                IsLoopOverwrite = (configDescriptorEncodedByte[2] & 0x01) == 1,
                IsStartOnHighTemp = (configDescriptorEncodedByte[2] & 0x02) == 1,
                IsStartOnLowTemp = (configDescriptorEncodedByte[2] & 0x04) == 1,
                IsStartOnButtonPress = (configDescriptorEncodedByte[2] & 0x08) == 1,
                IsAlarmOnHighLimit = (configDescriptorEncodedByte[2] & 0x10) == 1,
                IsAlarmOnLowLimit = (configDescriptorEncodedByte[2] & 0x20) == 1,
                TaskConfiguredTime = taskConfiguredTime.ToString("yyyy-MM-dd HH:mm:ss z"),
                DelayStartTime = (delayStartTime == ((new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(0))) ?"Not set":delayStartTime.ToString("yyyy-MM-dd HH:mm:ss z"),
                StartHighTemp = ((configDescriptorEncodedByte[13] << 8) | configDescriptorEncodedByte[12]),
                StartLowTemp = ((configDescriptorEncodedByte[15] << 8) | configDescriptorEncodedByte[14]),
                AlarmHighLimit = ((configDescriptorEncodedByte[23] << 8) | configDescriptorEncodedByte[22]),
                AlarmHighDelay = (uint)((configDescriptorEncodedByte[25] << 8) | configDescriptorEncodedByte[24]),
                AlarmLowLimit = ((configDescriptorEncodedByte[27] << 8) | configDescriptorEncodedByte[26]),
                AlarmLowDelay = (uint)((configDescriptorEncodedByte[29] << 8) | configDescriptorEncodedByte[28])
            };
            string configDescriptorString = JsonConvert.SerializeObject(configDescriptor);
            return configDescriptorString;
        }
        public static string ControlCharacteristicDecode(string base64String)
        {
            // base64 decode
            var base64EncodedBytes = Convert.FromBase64String(base64String);
            string controlCharacteristicEncoded = Encoding.UTF8.GetString(base64EncodedBytes);
            // string to bytes
            byte[] controlCharacteristicEncodedByte = Encoding.ASCII.GetBytes(controlCharacteristicEncoded);
            // parse object

            string SensorType = "";
            if ((controlCharacteristicEncodedByte[0] & 0x01) == 1)
            {
                SensorType = SensorType + string.Format("{0},", "temperature sensor");
            }
            if ((controlCharacteristicEncodedByte[0] & 0x02) == 1)
            {
                SensorType = SensorType + string.Format("{0},", "humidity sensor");
            }
            if ((controlCharacteristicEncodedByte[0] & 0x04) == 1)
            {
                SensorType = SensorType + string.Format("{0},", "pressure sensor");
            }
            if ((controlCharacteristicEncodedByte[0] & 0x08) == 1)
            {
                SensorType = SensorType + string.Format("{0},", "light sensor");
            }
            if ((controlCharacteristicEncodedByte[0] & 0x10) == 1)
            {
                SensorType = SensorType + string.Format("{0},", "inertial sensor");
            }
            if (SensorType.Length > 0)
            {
                SensorType = SensorType.Substring(0, SensorType.Length - 1);
            }

            string ControlEvent = "";
            switch (controlCharacteristicEncodedByte[1])
            {
                case 0:
                    ControlEvent = "stop task";
                    break;
                case 1:
                    ControlEvent = "enable task";
                    break;
                case 2:
                    ControlEvent = "reset";
                    break;
                case 3:
                    ControlEvent = "reserved for future use";
                    break;
                case 4:
                    ControlEvent = "data uploaded";
                    break;
                case 5:
                    ControlEvent = "accept new external probe serial number";
                    break;
                case 6:
                    ControlEvent = "test";
                    break;
                case 7:
                    ControlEvent = "store current timestamp to indicate task deliverd time";
                    break;
            }
            ControlCharacteristic controlCharacteristic = new ControlCharacteristic
            {
                SensorType = SensorType,
                ControlEvent = ControlEvent
            };


            string controlCharacteristicString = JsonConvert.SerializeObject(controlCharacteristic);
            return controlCharacteristicString;
        }

        public static string TemperatureCharacteristicDecode(string base64String)
        {
            // base64 decode
            var base64EncodedBytes = Convert.FromBase64String(base64String);
            string temperatureCharacteristicEncoded = Encoding.UTF8.GetString(base64EncodedBytes);
            // string to bytes
            byte[] temperatureCharacteristicEncodedByte = Encoding.ASCII.GetBytes(temperatureCharacteristicEncoded);
            // parse object
            TemperatureCharacteristic temperatureCharacteristic = new TemperatureCharacteristic
            {
                Count = (temperatureCharacteristicEncodedByte[1] << 8) | temperatureCharacteristicEncodedByte[0],
                OffsetTemp = (temperatureCharacteristicEncodedByte[3] << 8) | temperatureCharacteristicEncodedByte[2]
            };
            string temperatureCharacteristicString = JsonConvert.SerializeObject(temperatureCharacteristic);
            return temperatureCharacteristicString;
        }

        public static string LocalhostString()
        {
            using (StreamReader reader = new StreamReader("ip.properties"))
            {
                string line;
                line = reader.ReadLine();
                
                string[] lineValues = line.Split('=');
                return lineValues[1];               
            }
        }
    }
}
