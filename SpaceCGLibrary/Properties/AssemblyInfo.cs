﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的一般信息由以下
// 控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("SpaceCGLibrary for .NET Framework 4.6")]
[assembly: AssemblyDescription("huangmin@spacecg.cn")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SpaceCG.CN")]
[assembly: AssemblyProduct("SpaceCGLibrary")]
[assembly: AssemblyCopyright("Copyright ©  2019-2020")]
[assembly: AssemblyTrademark("SPACE")]
[assembly: AssemblyCulture("")]

//将 ComVisible 设置为 false 将使此程序集中的类型
//对 COM 组件不可见。  如果需要从 COM 访问此程序集中的类型，
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(true)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("b0bec753-29f1-4bac-9ba6-4a4df9f5462b")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
//可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值，
// 方法是按如下所示使用“*”: :
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.Config", Watch = true)]
