using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using DropNet;
using DropNet.Exceptions;
using Illallangi.CloudStoragePS.Config;

namespace Illallangi.CloudStoragePS.PowerShell
{
    [Cmdlet(VerbsCommon.Get, "DropBoxAccountInfo")]
    public sealed class GetDropBoxAccountInfo : DropBoxPSCmdlet
    {
        protected override IEnumerable<object> Process(DropNetClient client)
        {
            var accountInfo = client.AccountInfo();

            yield return new
            {
                DisplayName = accountInfo.display_name,
                EMail = accountInfo.email,
                UserID = accountInfo.uid,
                Country = accountInfo.country,
                ReferralLink = accountInfo.referral_link,
            };
        }
    }
}