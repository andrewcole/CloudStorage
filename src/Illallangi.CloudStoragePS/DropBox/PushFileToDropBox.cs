using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.PowerShell
{
    [Cmdlet(VerbsCommon.Push, "FileToDropBox")]
    public sealed class PushFileToDropBox : DropBoxPSCmdlet
    {
        private string currentSource;

        [Parameter]
        public string Destination { get; set; }

        [Parameter]
        public string FileName { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Alias("FullName")]
        public string Source
        { 
            get
            {
                return this.currentSource;
            }
            set
            {
                this.currentSource = string.IsNullOrWhiteSpace(value) ?
                    null :
                    Path.GetFullPath(value);
            }
        }

        protected override System.Collections.Generic.IEnumerable<object> Process(DropNetClient client)
        {
            if (!File.Exists(this.Source))
            {
                throw new ArgumentException(string.Format("File {0} does not exist", this.Source));
            }

            var result = client.UploadFile(this.Destination ?? "/", this.FileName ?? Path.GetFileName(this.Source), File.ReadAllBytes(this.Source));

            yield return new
            {
                Path = result.Path,
            };
        }
    }
}
