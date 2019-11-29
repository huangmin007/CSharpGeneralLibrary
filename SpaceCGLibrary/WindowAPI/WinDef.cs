using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCG.WindowAPI
{
    /// <summary>
    /// POINT 结构定义点的 x 和 y 坐标
    /// <para> POINT, POINTL, *PPOINT, *NPPOINT, *LPPOINT </para>
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-point </para>
    /// </summary>
    public struct POINT
    {
        /// <summary>
        /// 指定点的x坐标
        /// </summary>
        public int x;
        /// <summary>
        /// 指定点的y坐标
        /// </summary>
        public int y;
        /// <summary>
        /// POINT 结构体
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{x}, {y}";
        }
    }

    /// <summary>
    /// SIZE 结构定义矩形的宽度和高度
    /// <para>存储在此结构中的矩形尺寸可以对应于视口范围，窗口范围，文本范围，位图尺寸或某些扩展功能的长宽比过滤器。</para>
    /// <para> SIZE, *PSIZE, *LPSIZE </para>
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-size </para>
    /// </summary>
    public struct SIZE
    {
        /// <summary>
        /// 指定矩形的宽度。单位取决于使用此结构的功能。
        /// </summary>
        public int cx;
        /// <summary>
        /// 指定矩形的高度。单位取决于使用此结构的功能。
        /// </summary>
        public int cy;
        /// <summary>
        /// SIZE 结构体
        /// </summary>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{cx}, {cy}";
        }
    }

    /// <summary>
    /// RECT 结构通过其左上角和右下角的坐标定义一个矩形
    /// <para> RECT, RECTL, *PRECT, NEAR *NPRECT, FAR *LPRECT </para>
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rect </para>
    /// </summary>
    public struct RECT
    {
        /// <summary>
        /// 指定矩形左上角的x坐标
        /// </summary>
        public int left;
        /// <summary>
        /// 指定矩形左上角的y坐标
        /// </summary>
        public int top;
        /// <summary>
        /// 指定矩形右下角的x坐标。
        /// </summary>
        public int right;
        /// <summary>
        /// 指定矩形右下角的y坐标。
        /// </summary>
        public int buttom;
        /// <summary>
        /// RECT 结构体
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="buttom"></param>
        public RECT(int left, int top, int right, int buttom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.buttom = buttom;
        }
        /// <summary>
        /// @ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{left}, {top}, {right}, {buttom}";
        }
    }

    /// <summary>
    /// 标识线程，进程或窗口的每英寸点数（dpi）设置。
    /// <para>参考：https://docs.microsoft.com/en-us/windows/win32/api/windef/ne-windef-dpi_awareness </para>
    /// </summary>
    public enum DpiAwaeness
    {
        /// <summary>
        /// DPI意识无效。这是无效的DPI感知值。
        /// </summary>
        INVALID,
        /// <summary>
        /// DPI不知道。此过程无法适应DPI更改，并且始终假定比例因子为100％（96 DPI）。系统将在其他任何DPI设置上自动缩放它。
        /// </summary>
        UNAWARE,
        /// <summary>
        /// 系统DPI感知。此过程无法适应DPI更改。它将一次查询DPI，并在该过程的整个生命周期中使用该值。
        /// <para>如果DPI更改，则该过程将不会调整为新的DPI值。当DPI从系统值更改时，系统会自动按比例将其放大或缩小。</para>
        /// </summary>
        SYSTEM_AWARE,
        /// <summary>
        /// 每个监视器DPI感知。创建DPI时，此过程将对其进行检查，并在DPI更改时调整比例因子。这些过程不会被系统自动缩放
        /// </summary>
        PER_MONITOR_AWARE
    }

    /// <summary>
    /// 标识窗口的 DPI 托管行为。此行为允许在线程中创建的窗口托管具有不同 DPI_AWARENESS_CONTEXT 的子窗口 
    /// <para>https://docs.microsoft.com/en-us/windows/win32/api/windef/ne-windef-dpi_hosting_behavior </para>
    /// </summary>
    public enum DpiHostingBehavior
    {
        /// <summary>
        /// DPI托管行为无效。如果先前的 SetThreadDpiHostingBehavior 调用使用了无效的参数，通常会发生这种情况。
        /// </summary>
        INVALID,
        /// <summary>
        /// 默认的 DPI 托管行为。关联的窗口行为正常，无法使用不同的 DPI_AWARENESS_CONTEXT 创建或重新父级子窗口。
        /// </summary>
        DEFAULT,
        /// <summary>
        /// 混合的 DPI 托管行为。这样可以使用不同的 DPI_AWARENESS_CONTEXT 创建和重新创建父窗口。这些子窗口将由OS独立缩放。
        /// </summary>
        MIXED
    }

    /// <summary>
    /// WinDef.h
    /// </summary>
    public static partial class WinDef
    {
    }
}
