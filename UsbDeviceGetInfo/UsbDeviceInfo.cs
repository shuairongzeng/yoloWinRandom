using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbDeviceGetInfo
{
    using System;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;

    public class UsbDeviceInfo
    {
        public string Vid { get; set; }
        public string Pid { get; set; }
        // 你可以添加更多属性，如制造商、产品名称等
        public string Manufacturer { get; set; }
        public string ProductName { get; set; }
    }

    public class UsbDeviceReader
    {
        // 定义 GUID for USB devices
        public static Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");

        // 定义 SetupAPI 常量
        public const int DIGCF_PRESENT = 0x00000002;
        public const int DIGCF_DEVICEINTERFACE = 0x00000010;

        // 定义 SP_DEVINFO_DATA 结构
        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        // 定义 SP_DEVICE_INTERFACE_DATA 结构 (如果需要获取设备接口信息)
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            private IntPtr Reserved;
        }

        // 定义 SP_DEVICE_INTERFACE_DETAIL_DATA 结构 (如果需要获取设备路径)
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public uint cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        // 导入 SetupAPI 函数
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(
            ref Guid ClassGuid,
            [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
            IntPtr hwndParent,
            uint Flags
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInfo(
            IntPtr DevInfoSet,
            uint MemberIndex,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetupDiGetDeviceRegistryProperty(
            IntPtr DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            uint Property,
            out uint PropertyRegDataType,
            IntPtr PropertyBuffer,
            uint PropertyBufferSize,
            out uint RequiredSize
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(
            IntPtr DevInfoSet
        );

        // 定义设备注册表属性常量 (你需要查找更多常量来获取其他属性)
        public const uint SPDRP_HARDWAREID = 0x00000004; // 获取硬件 ID (包含 VID和PID)
        public const uint SPDRP_MFG = 0x0000000B; // 获取制造商
        public const uint SPDRP_DEVICEDESC = 0x00000000; // 获取设备描述 (可能包含产品名称)


        public static List<UsbDeviceInfo> GetUsbDevices()
        {
            List<UsbDeviceInfo> usbDevices = new List<UsbDeviceInfo>();
            IntPtr devInfoSet = IntPtr.Zero;

            try
            {
                // 获取设备信息集合
                Guid usbDeviceGuid = GUID_DEVINTERFACE_USB_DEVICE;
                devInfoSet = SetupDiGetClassDevs(ref usbDeviceGuid, null, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);

                if (devInfoSet == IntPtr.Zero || devInfoSet == new IntPtr(-1))
                {
                    // Handle error
                    return usbDevices;
                }

                SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
                deviceInfoData.cbSize = (uint)Marshal.SizeOf(deviceInfoData);

                uint memberIndex = 0;
                while (SetupDiEnumDeviceInfo(devInfoSet, memberIndex, ref deviceInfoData))
                {
                    UsbDeviceInfo usbDevice = new UsbDeviceInfo();

                    // 获取硬件 ID (包含 VID 和 PID)
                    string hardwareId = GetDeviceRegistryPropertyString(devInfoSet, ref deviceInfoData, SPDRP_HARDWAREID);
                    if (!string.IsNullOrEmpty(hardwareId))
                    {
                        // 解析硬件 ID 获取 VID 和 PID
                        // 硬件 ID 格式通常是 USB\VID_VVVV&PID_PPPP...
                        string[] ids = hardwareId.Split('&');
                        foreach (string id in ids)
                        {
                            if (id.StartsWith("VID_"))
                            {
                                usbDevice.Vid = id.Substring(4, 4);
                            }
                            else if (id.StartsWith("PID_"))
                            {
                                usbDevice.Pid = id.Substring(4, 4);
                            }
                        }
                    }

                    // 获取制造商
                    usbDevice.Manufacturer = GetDeviceRegistryPropertyString(devInfoSet, ref deviceInfoData, SPDRP_MFG);

                    // 获取产品名称 (可能在设备描述中)
                    usbDevice.ProductName = GetDeviceRegistryPropertyString(devInfoSet, ref deviceInfoData, SPDRP_DEVICEDESC);

                    usbDevices.Add(usbDevice);
                    memberIndex++;
                }
            }
            finally
            {
                // 释放设备信息集合句柄
                if (devInfoSet != IntPtr.Zero && devInfoSet != new IntPtr(-1))
                {
                    SetupDiDestroyDeviceInfoList(devInfoSet);
                }
            }

            return usbDevices;
        }

        // 辅助函数：获取设备注册表属性的字符串值
        private static string GetDeviceRegistryPropertyString(IntPtr devInfoSet, ref SP_DEVINFO_DATA deviceInfoData, uint property)
        {
            uint propertyRegDataType;
            uint requiredSize;

            // 第一次调用获取所需的缓冲区大小
            SetupDiGetDeviceRegistryProperty(devInfoSet, ref deviceInfoData, property, out propertyRegDataType, IntPtr.Zero, 0, out requiredSize);

            if (requiredSize > 0)
            {
                IntPtr buffer = Marshal.AllocHGlobal((int)requiredSize);
                try
                {
                    if (SetupDiGetDeviceRegistryProperty(devInfoSet, ref deviceInfoData, property, out propertyRegDataType, buffer, requiredSize, out requiredSize))
                    {
                        // 根据属性类型处理数据，这里假设是字符串
                        if (propertyRegDataType == 1) // REG_SZ (string)
                        {
                            return Marshal.PtrToStringAuto(buffer);
                        }
                        else if (propertyRegDataType == 7) // REG_MULTI_SZ (multiple strings)
                        {
                            // For hardware IDs, this might return multiple strings
                            // We'll just return the first one for simplicity here
                            return Marshal.PtrToStringAuto(buffer);
                        }
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
            return null;
        }
    }
}
