using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranslateResourcesFromStrings
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            var result2 = folderBrowserDialog2.ShowDialog();

            DirectoryInfo di = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
            DirectoryInfo diResult = new DirectoryInfo(folderBrowserDialog2.SelectedPath);

            DirectoryInfo[] values = di.GetDirectories("value*", SearchOption.TopDirectoryOnly);
            foreach (DirectoryInfo v in values)
            {
                List<Dictionary<string, string>> allSettings = new List<Dictionary<string, string>>();
                string newFileName = String.Format("AppResources.{0}.resx", v.Name.Replace("values", "")).Replace("..", ".").Replace("-", "");
                FileInfo[] files = v.GetFiles("string*.xml", SearchOption.TopDirectoryOnly);
                foreach (FileInfo f in files)
                {
                    Dictionary<string, string> oSettings = new Dictionary<string, string>();

                    // Open the stream and read it back.
                    using (StreamReader sr = f.OpenText())
                    {
                        String data = sr.ReadToEnd();

                        string[] lines = data.Split(new string[] { "<string name=\"" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (String a in lines)
                        {
                            if (!a.Contains("<resource"))
                            {
                                String key = a.Substring(0, a.IndexOf("\""));
                                String value = a.Substring(a.IndexOf(">") + 1, a.IndexOf("</") - (a.IndexOf(">") + 1));
                                oSettings.Add(key, value);
                            }
                        }
                    }

                    allSettings.Add(oSettings);
                }

                String allSettingsLines = "<?xml version=\"1.0\" encoding=\"utf - 8\"?> <root> ";
                allSettingsLines += "<resheader name=\"resmimetype\">		<value>text/microsoft-resx</value>	</resheader>	<resheader name=\"version\">		<value>2.0</value>	</resheader>	<resheader name=\"reader\">		<value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>	</resheader>	<resheader name=\"writer\">		<value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>	</resheader>";
                String linePlaceHolder = "<data name=\"{0}\" xml:space=\"preserve\"><value>{1}</value></data>";
                foreach (Dictionary<string, string> list in allSettings)
                {
                    foreach (string k in list.Keys)
                    {
                        string value = list[k];
                        allSettingsLines += Environment.NewLine + String.Format(linePlaceHolder, k, value);
                    }
                }

                allSettingsLines += Environment.NewLine + "</root>";
                System.IO.File.WriteAllText(diResult.FullName + "\\" + newFileName, allSettingsLines);

            }
        }
    }
}
