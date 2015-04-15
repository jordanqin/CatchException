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
using System.Threading.Tasks;

namespace CatchException
{
    //处理这些异常的时候，应用程序已经崩溃且无法恢复，这时android系统已经杀死了应用程序，
    //唯一能做的就是：记录异常、场景等重要信息，在合适的时候发送到服务器，以供错误分析

    [Application(Label = "MyApplication")]
    public class MyApplication : Application
    {
        public MyApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            //注册未处理异常事件
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

            //CrashHandler crashHandler = CrashHandler.GetInstance();
            //crashHandler.Init(ApplicationContext);
        }

        protected override void Dispose(bool disposing)
        {
            AndroidEnvironment.UnhandledExceptionRaiser -= AndroidEnvironment_UnhandledExceptionRaiser;
            base.Dispose(disposing);
        }

        void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            UnhandledExceptionHandler(e.Exception, e);
        }

        /// <summary>
        /// 处理未处理异常
        /// </summary>
        /// <param name="e"></param>
        private void UnhandledExceptionHandler(Exception ex, RaiseThrowableEventArgs e)
        {
            //处理程序（记录 异常、设备信息、时间等重要信息）
            //**************

            //提示
            Task.Run(() =>
                {
                    Looper.Prepare();
                    //可以换成更友好的提示
                    Toast.MakeText(this, "很抱歉,程序出现异常,即将退出.", ToastLength.Long).Show();
                    Looper.Loop();
                });

            //停一会，让前面的操作做完
            System.Threading.Thread.Sleep(2000);

            e.Handled = true;
        }
    }
}