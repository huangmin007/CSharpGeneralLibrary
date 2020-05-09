## General



* ### BoyerMoore 
    [Boyer-Moore (BM) 算法实现](https://baike.baidu.com/item/Boyer-%20Moore%E7%AE%97%E6%B3%95/16548374?fr=aladdin)  
抽象理解：<b>该算法适合 集合数据 与 集合数据 的查找，在 A 集合元素中查找与 B 集合所有元素位置相同、数据相同、值相同的索引；注意：这是重点概念！！！</b>  
简单理解：例如：字符串的查找  

    该算法应用最多的是在字符查找方面，一般操作系统底层字符查找都是基于该算法，正则表达式查找算法也是使用该算法之一；
在用于查找子字符串的算法当中，BM（Boyer-Moore）算法被认为最高效的字符串搜索算法。

    BoyerMoore 和 BoyerMoore&lt;T&gt; 类扩展了 BM 算法的应用，<b>支持多语言(例如：英文、中文等其它语言)查找，支持多数据类型(编程语言基类型/值类型，例如：int,uint,ushort,struct等)查找</b>

```C#
//BoyerMoore IReadOnlyList<byte>
byte[] p1 = new byte[] { 0x04, 0x04, 0x05 };
byte[] s1 = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x04, 0x05, 0x04, 0x02, 0x01, 0x04, 0x04, 0x05, 0x04, 0x03 };
List<byte> s2 = new List<byte>() { 0x03, 0x02, 0x03, 0x04, 0x04, 0x05, 0x03, 0x04, 0x04, 0x05, 0x01, 0x04, 0x04, 0x05, 0x04, 0x03 };

int index;
int[] indexs;

BoyerMoore bm1 = new BoyerMoore(p1);
index = bm1.Search(s1);
indexs = bm1.SearchAll(s1);
//index = BoyerMoore.Search(s1, p1);
//indexs = BoyerMoore.SearchAll(s1, p1);
Console.WriteLine("1::{0}, {1}", index, string.Join(",,", indexs));

index = bm1.Search(s2);
indexs = bm1.SearchAll(s2);
//index = BoyerMoore.Search(s1, p2);
//indexs = BoyerMoore.SearchAll(s1, p2);
Console.WriteLine("2::{0}, {1}", index, string.Join(",,", indexs));

//BoyerMoore string
string en_p1 = "ello";
string en_s1 = "Hello World, Hello World, Hello World, Hello World";

index = BoyerMoore.Search(ref en_s1, ref en_p1);
indexs = BoyerMoore.SearchAll(ref en_s1, ref en_p1);
Console.WriteLine("3::{0}, {1}", index, string.Join(",,", indexs));

string cn_p1 = "深圳";
string cn_p2 = "深圳圳";
string cn_s1 = "广，东省深圳深圳深深圳圳temp市福田,区深圳圳街，hello";
BoyerMoore bm2 = new BoyerMoore(ref cn_p1, 0xFFFF);
index = bm2.Search(ref cn_s1);
Console.WriteLine("4::{0}, {1}", index, string.Join(",,", indexs));
index = bm2.SearchAt(ref cn_s1, 2);
Console.WriteLine("4::{0}, {1}", index, string.Join(",,", indexs));

index = BoyerMoore.Search(ref cn_s1, ref cn_p1, 0xFFFF);
indexs = BoyerMoore.SearchAll(ref cn_s1, ref cn_p2, 0xFFFF);
Console.WriteLine("5::{0}, {1}", index, string.Join(",,", indexs));

/*
1::3, 3,,9
2::3, 3,,7,,11
3::1, 1,,14,,27,,40
4::4, 1,,14,,27,,40
4::6, 1,,14,,27,,40
5::4, 9,,21
*/
```

* ### BoyerMoore&lt;T&gt; where T: struct
    [Boyer-Moore (BM) 算法实现](https://baike.baidu.com/item/Boyer-%20Moore%E7%AE%97%E6%B3%95/16548374?fr=aladdin) 扩展应用，
[BoyerMoore](#BoyerMoore) 的泛型版本，只支持基类型/值类型，未开放引用类型，理论上一样是可以使用引用类型，只需要对象实现重写 <b>Object.Equals(object obj)</b> 函数
```C#
List<int> p1 = new List<int>() { 789, 987, 879 };
int[] s1 = new int[] { 478, 586, 789, 987, 879, 852, 147, 963, 456, 654, 478, 586, 789, 987, 879, 852, 147, 963, 456, 654, 789, 987, 879 };
List<int> s2 = new List<int>() {478, 586, 789, 987, 879, 852, 147, 963, 456, 654, 478, 586, 789, 987, 879, 852, 147, 963, 456, 654, 789, 987, 879 };

int index;
int[] indexs;

BoyerMoore<int> bm1 = new BoyerMoore<int>(p1);
index = bm1.Search(s1);
indexs = bm1.SearchAll(s2);
Console.WriteLine("1::{0}, {1}", index, string.Join(",,", indexs));

//这只是一个示例
struct ST
{
    public int a;
    public uint b;
    public string c;

    // ...
    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(ST)) return false;
        return c == ((ST)obj).c;
    }

    public override string ToString()
    {
        return c;
    }
}

List<ST> st_s1 = new List<ST>() {
    new ST(){ a = 1, b = 234 , c = "aaa"},
    new ST(){ a = 3, b = 234 , c = "bbb"},
    new ST(){ a = 2, b = 25 , c = "ccc"},
    new ST(){ a = 6, b = 25 , c = "ddd"},
    new ST(){ a = 19, b = 29 , c = "eee"},
    new ST(){ a = 22, b = 25 , c = "ccc"},
    new ST(){ a = 62, b = 25 , c = "ddd"},
    new ST(){ a = 19, b = 29 , c = "eee"},
};
List<ST> st_p1 = new List<ST>()
{
    new ST(){ a = 22, b = 252 , c = "ccc"},
    new ST(){ a = 63, b = 252 , c = "ddd"},
};

BoyerMoore<ST> bm2 = new BoyerMoore<ST>(st_p1);
index = bm2.Search(st_s1);
indexs = bm2.SearchAll(st_s1);
Console.WriteLine("2::{0}, {1}", index, string.Join(",,", indexs));

/*
1::2, 2,,12,,20
2::2, 2,,5
*/
```
  
* ### AbstractDataAnalyseAdapter 
    参考 [HP-Socket.Net](https://gitee.com/int2e/HPSocket.Net/tree/master) 库，单独分离数据分析适配器，用途更多。  
    抽象类，多通道数据分析适配器(支持多线程)，有四个基本分析模式抽象子类，也可以自行继承 AbstractDataAnalyseAdapter 抽象类，或是它的四个抽象子类，每个类都表示一种数据分析模式算法；
    * FixedSizeDataAnalysePattern 固定字节大小的分析模式(抽象基类)
    * FixedHeadDataAnalysePattern 固定头部大小的分析模式(抽象基类)
    * TerminatorDataAnalysePattern 固定终结数据块分析模式(抽象基类，以数据块为分割符，对数据进行分割）
    * BetweenAndDataAnalysePattern 固定数据头端，和固定数据尾端的分析模式(抽象基类，在数据端 A 和数据端 B 之间的数据块）
    * 自定义实现 AbstractDataAnalyseAdapter 抽象类
```C#
//这只是个示例，可以继承 AbstractDataAnalyseAdapter 抽象类实现数据分析
public class TextDataAnalyse<TChannelKey> : TerminatorDataAnalysePattern<TChannelKey, string>
{
    public TextDataAnalyse() : base(terminator: Encoding.Default.GetBytes("\r\n")) // 指定结束符为\r\n
    {
    }

    /// <inheritdoc/>
    protected override string ConvertResultType(List<byte> data)
    {
        return Encoding.Default.GetString(data.ToArray());
    }
}

public class Data
{
    public int a = 0;
    public int b = 0;
    public string c = "";
}
public class TestDataAnalyse : FixedSizeDataAnalysePattern<HPSocket.IClient, Data>
{
    public TestDataAnalyse() : base(32)
    {
    }

    /// <inheritdoc/>
    protected sealed override Data ConvertResultType(List<byte> packet)
    {
        byte[] value = packet.ToArray();
        return new Data()
        {
            a = BitConverter.ToInt32(value, 0),
            b = BitConverter.ToInt32(value, 4),
            c = Encoding.Default.GetString(value, 8, value.Length - 8),
        };
    }
}

//AbstractDataAnalyseAdapter<HPSocket.IClient, byte[]> dataAnalyse = new FixedSizeDataAnalyse<HPSocket.IClient>(32);
//AbstractDataAnalyseAdapter<HPSocket.IClient, Data> dataAnalyse = new TestDataAnalyse();
//var dataAnalyse = new FixedSizeDataAnalyse<HPSocket.IClient>(32);
var dataAnalyse = new TestDataAnalyse();
var tcpClient = HPSocketExtension.CreateClient<TcpClient>("127.0.0.1", 9999, (HPSocket.IClient client, byte[] data) =>
{
    dataAnalyse.AnalyseChannel(client, data, (c, d) =>
    {
        //StackTrace st = new StackTrace(new StackFrame(true));
        //StackFrame sf = st.GetFrame(0);
        //Console.WriteLine("File:{0} Method:{1} Line:{2} Column:{3}", sf.GetFileName(), sf.GetMethod().Name, sf.GetFileLineNumber(), sf.GetFileColumnNumber());

        //Console.WriteLine("Length:{0} {1}", d.Length, Encoding.Default.GetString(d));
        Console.WriteLine("Value:{0}", d);
        return true;
    });
}, true, App.Log);
dataAnalyse.AddChannel(tcpClient);

var udpClient = HPSocketExtension.CreateClient<UdpClient>("127.0.0.1", 9999, (client, data) =>
{
    dataAnalyse.AnalyseChannel(client, data, (c, d) =>
    {
        Console.WriteLine("Value:{0}", d);
        return true;
    });
}, false, App.Log);
dataAnalyse.AddChannel(udpClient);
```


 