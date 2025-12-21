using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Core.Helpers
{
    /// <summary>
    /// 文件帮助 类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 以字节方式读取文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static byte[] Load(string path)
        {
            if(File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }

            throw new Exception($"{path}不存在");
        }

        public static bool SaveFileDialog(string title,string filter,string defaultExt,string defaultFullName,out string fullname)
        {
            fullname = null;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Title = title;
            dlg.Filter = filter;
            string defaultDir = null;
            if (defaultFullName != null)
            {
                defaultDir = Path.GetDirectoryName(defaultFullName);
                dlg.InitialDirectory= defaultDir;
                dlg.FileName= Path.GetFileName(defaultFullName);
            }
            dlg.DefaultExt= defaultExt; 
            if(dlg.ShowDialog()==true)
            {
                fullname=dlg.FileName;
                return true;
            }

            return false;   
        }
    }
}
