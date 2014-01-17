using System;
using System.IO;
using System.Management.Automation;
using DropNet;

namespace Illallangi.CloudStoragePS.DropBox
{
    [Cmdlet(VerbsCommon.Push, "FileToDropBox")]
    public sealed class PushFileToDropBox : DropBoxPsCmdlet
    {
        private string currentSource;

        [Parameter]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string Destination { get; set; }

        [Parameter]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string FileName { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Alias("FullName")]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
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
                result.Path,
            };
        }
    }
}
