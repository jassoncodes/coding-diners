using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiesgoPichinchaQuoteParser.Models
{
    internal class RiesgoFiles
    {

        public string name;
        public ArrayList textContent;
        //public string[] textContent;
        public string outPath;
        public string outFileName;
        public string fullOutPathName;

        //public FileTarget(string name, string[] textContent, string path) {
        public RiesgoFiles(string name, ArrayList textContent, string path)
        {
            this.name = name;
            this.textContent = textContent;
            this.outPath = path;
            this.outFileName = name + ".txt";
            this.fullOutPathName = this.outPath + this.outFileName;
        }

        public RiesgoFiles()
        {
            this.name = "";
            this.textContent = new ArrayList();
            //this.textContent = Array.Empty<string>();
            this.outPath = "";
            this.outFileName = "";
            this.fullOutPathName = "";
        }


    }
}
