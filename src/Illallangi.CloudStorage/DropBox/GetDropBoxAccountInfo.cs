using System.Collections.Generic;
using System.Management.Automation;
using DropNet;

namespace Illallangi.CloudStorage.DropBox
{
    [Cmdlet(VerbsCommon.Get, "DropBoxAccountInfo")]
    public sealed class GetDropBoxAccountInfo : DropBoxPsCmdlet
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