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
    //������Щ�쳣��ʱ��Ӧ�ó����Ѿ��������޷��ָ�����ʱandroidϵͳ�Ѿ�ɱ����Ӧ�ó���
    //Ψһ�����ľ��ǣ���¼�쳣����������Ҫ��Ϣ���ں��ʵ�ʱ���͵����������Թ��������

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

            //ע��δ�����쳣�¼�
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
        /// ����δ�����쳣
        /// </summary>
        /// <param name="e"></param>
        private void UnhandledExceptionHandler(Exception ex, RaiseThrowableEventArgs e)
        {
            //������򣨼�¼ �쳣���豸��Ϣ��ʱ�����Ҫ��Ϣ��
            //**************

            //��ʾ
            Task.Run(() =>
                {
                    Looper.Prepare();
                    //���Ի��ɸ��Ѻõ���ʾ
                    Toast.MakeText(this, "�ܱ�Ǹ,��������쳣,�����˳�.", ToastLength.Long).Show();
                    Looper.Loop();
                });

            //ͣһ�ᣬ��ǰ��Ĳ�������
            System.Threading.Thread.Sleep(2000);

            e.Handled = true;
        }
    }
}