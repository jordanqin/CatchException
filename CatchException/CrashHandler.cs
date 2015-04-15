using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Threading.Tasks;

namespace CatchException
{
    /// <summary>
    /// （IUncaughtExceptionHandler只能捕获到Dalvik runtime的异常，mono runtime中的C#异常，这个不起作用）
    /// IUncaughtException处理类,当程序发生Uncaught异常的时候,有该类来接管程序
    /// </summary>
    [Obsolete]
    public class CrashHandler:Thread.IUncaughtExceptionHandler
    {
        //系统默认的UncaughtException处理类 
        private Thread.IUncaughtExceptionHandler mDefaultHandler;
        //CrashHandler实例
        private static CrashHandler INSTANCE = new CrashHandler();
        //程序的Context对象
        private Context mContext;

        /// <summary>
        /// 保证只有一个CrashHandler实例
        /// </summary>
        private CrashHandler()
        {
        }

        /// <summary>
        /// 获取CrashHandler实例 ,单例模式
        /// </summary>
        /// <returns></returns>
        public static CrashHandler GetInstance()
        {
            return INSTANCE;
        }

        public IntPtr Handle
        {
            get { return Thread.CurrentThread().Handle; }
        }

        public void Dispose()
        {
            this.Dispose();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        public void Init(Context context)
        {
            mContext = context;
            //获取系统默认的UncaughtException处理器
            mDefaultHandler = Thread.DefaultUncaughtExceptionHandler;
            //设置该CrashHandler为程序的默认处理器
            Thread.DefaultUncaughtExceptionHandler = this;
        }

        ///当UncaughtException发生时会转入该函数来处理
        public void UncaughtException(Thread thread, Throwable ex)
        {
            if (!HandleException(ex) && mDefaultHandler != null)
            {
                //如果用户没有处理则让系统默认的异常处理器来处理
                mDefaultHandler.UncaughtException(thread, ex);
            }
            else
            {
            }
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>如果处理了该异常信息返回true; 否则返回false.</returns>
        private bool HandleException(Throwable ex)
        {
            if (ex == null)
            {
                return false;
            }

            //处理程序（记录 异常、设备信息、时间等重要信息）
            //************


            //提示
            Task.Run(() =>
            {
                Looper.Prepare();
                //可以换成更友好的提示
                Toast.MakeText(mContext, "很抱歉,程序出现异常,即将退出.", ToastLength.Long).Show();
                Looper.Loop();
            });

            //停一会，让前面的操作做完
            System.Threading.Thread.Sleep(2000);

            return true;
        }
    }
}