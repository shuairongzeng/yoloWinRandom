// See https://aka.ms/new-console-template for more information
using HidSharp;

Console.WriteLine("Hello, World! 开始读取USB信息");

// 创建一个HID设备列表 获取电脑所有HID设备列表

// 获取所有的HID设备列表
var devices = DeviceList.Local.GetHidDevices();

foreach (var device in devices)
{
    Console.WriteLine($"Device: {device.GetProductName()}");
    Console.WriteLine($"Vendor ID (VID): {device.VendorID:X4}"); // 16进制格式
    Console.WriteLine($"Product ID (PID): {device.ProductID:X4}"); // 16进制格式
    Console.WriteLine($"Manufacturer: {device.GetManufacturer()}");
    // 更多属性可以从 device 对象获取或通过打开设备获取
    Console.WriteLine("---");
}
