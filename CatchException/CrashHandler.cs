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
    /// ��IUncaughtExceptionHandlerֻ�ܲ���Dalvik runtime���쳣��mono runtime�е�C#�쳣������������ã�
    /// IUncaughtException������,��������Uncaught�쳣��ʱ��,�и������ӹܳ���
    /// </summary>
    [Obsolete]
    public class CrashHandler:Thread.IUncaughtExceptionHandler
    {
        //ϵͳĬ�ϵ�UncaughtException������ 
        private Thread.IUncaughtExceptionHandler mDefaultHandler;
        //CrashHandlerʵ��
        private static CrashHandler INSTANCE = new CrashHandler();
        //�����Context����
        private Context mContext;

        /// <summary>
        /// ��ֻ֤��һ��CrashHandlerʵ��
        /// </summary>
        private CrashHandler()
        {
        }

        /// <summary>
        /// ��ȡCrashHandlerʵ�� ,����ģʽ
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
        /// ��ʼ��
        /// </summary>
        /// <param name="context"></param>
        public void Init(Context context)
        {
            mContext = context;
            //��ȡϵͳĬ�ϵ�UncaughtException������
            mDefaultHandler = Thread.DefaultUncaughtExceptionHandler;
            //���ø�CrashHandlerΪ�����Ĭ�ϴ�����
            Thread.DefaultUncaughtExceptionHandler = this;
        }

        ///��UncaughtException����ʱ��ת��ú���������
        public void UncaughtException(Thread thread, Throwable ex)
        {
            if (!HandleException(ex) && mDefaultHandler != null)
            {
                //����û�û�д�������ϵͳĬ�ϵ��쳣������������
                mDefaultHandler.UncaughtException(thread, ex);
            }
            else
            {
            }
        }

        /// <summary>
        /// �쳣����
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>��������˸��쳣��Ϣ����true; ���򷵻�false.</returns>
        private bool HandleException(Throwable ex)
        {
            if (ex == null)
            {
                return false;
            }

            //������򣨼�¼ �쳣���豸��Ϣ��ʱ�����Ҫ��Ϣ��
            //************


            //��ʾ
            Task.Run(() =>
            {
                Looper.Prepare();
                //���Ի��ɸ��Ѻõ���ʾ
                Toast.MakeText(mContext, "�ܱ�Ǹ,��������쳣,�����˳�.", ToastLength.Long).Show();
                Looper.Loop();
            });

            //ͣһ�ᣬ��ǰ��Ĳ�������
            System.Threading.Thread.Sleep(2000);

            return true;
        }
    }
}