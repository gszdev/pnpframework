﻿using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PnP.Framework.Provisioning.Model;
using PnP.Framework.Provisioning.ObjectHandlers.TokenDefinitions;
using PnP.Framework.Test.Framework.Functional.Validators;

namespace PnP.Framework.Test.Framework.Functional.Implementation
{
    internal class CustomActionImplementation : ImplementationBase
    {

        internal void SiteCollectionCustomActionAdding(string url)
        {
            using (var cc = TestCommon.CreateClientContext(url))
            {
                // Ensure we can test clean
                DeleteCustomActions(cc);

                // Add custom actions
                var result = TestProvisioningTemplate(cc, "customaction_add.xml", Handlers.CustomActions);

                // Ensure the needed tokens are added to the target token parser, this is needed due to the tokenparser perf optimalizations
                result.TargetTokenParser.Tokens.Add(new SiteToken(cc.Web));
                result.TargetTokenParser.Tokens.Add(new SiteTitleToken(cc.Web));
                result.TargetTokenParser.Tokens.Add(new GroupSiteTitleToken(cc.Web));

                Assert.IsTrue(CustomActionValidator.Validate(result.SourceTemplate.CustomActions, result.TargetTemplate.CustomActions, result.TargetTokenParser, cc.Web));

                // Update custom actions
                var result2 = TestProvisioningTemplate(cc, "customaction_delta_1.xml", Handlers.CustomActions);
                // Ensure the needed tokens are added to the target token parser, this is needed due to the tokenparser perf optimalizations
                result2.TargetTokenParser.Tokens.Add(new SiteToken(cc.Web));
                result2.TargetTokenParser.Tokens.Add(new SiteTitleToken(cc.Web));
                result2.TargetTokenParser.Tokens.Add(new GroupSiteTitleToken(cc.Web));

                Assert.IsTrue(CustomActionValidator.Validate(result2.SourceTemplate.CustomActions, result2.TargetTemplate.CustomActions, result2.TargetTokenParser, cc.Web));

                // Update custom actions
                var result3 = TestProvisioningTemplate(cc, "customaction_1605_delta_2.xml", Handlers.CustomActions);
                // Ensure the needed tokens are added to the target token parser, this is needed due to the tokenparser perf optimalizations
                result3.TargetTokenParser.Tokens.Add(new SiteToken(cc.Web));
                result3.TargetTokenParser.Tokens.Add(new SiteTitleToken(cc.Web));
                result3.TargetTokenParser.Tokens.Add(new GroupSiteTitleToken(cc.Web));

                Assert.IsTrue(CustomActionValidator.Validate(result3.SourceTemplate.CustomActions, result3.TargetTemplate.CustomActions, result3.TargetTokenParser, cc.Web));
            }
        }

        internal void WebCustomActionAdding(string url)
        {
            using (var cc = TestCommon.CreateClientContext(url))
            {
                // Ensure we can test clean
                DeleteCustomActions(cc);

                // Add custom actions
                var result = TestProvisioningTemplate(cc, "customaction_add.xml", Handlers.CustomActions);
                // Ensure the needed tokens are added to the target token parser, this is needed due to the tokenparser perf optimalizations
                result.TargetTokenParser.Tokens.Add(new SiteToken(cc.Web));
                result.TargetTokenParser.Tokens.Add(new SiteTitleToken(cc.Web));
                result.TargetTokenParser.Tokens.Add(new GroupSiteTitleToken(cc.Web));

                Assert.IsTrue(CustomActionValidator.ValidateCustomActions(result.SourceTemplate.CustomActions.WebCustomActions, result.TargetTemplate.CustomActions.WebCustomActions, result.TargetTokenParser, cc.Web));

                // Update custom actions
                var result2 = TestProvisioningTemplate(cc, "customaction_delta_1.xml", Handlers.CustomActions);
                // Ensure the needed tokens are added to the target token parser, this is needed due to the tokenparser perf optimalizations
                result2.TargetTokenParser.Tokens.Add(new SiteToken(cc.Web));
                result2.TargetTokenParser.Tokens.Add(new SiteTitleToken(cc.Web));
                result2.TargetTokenParser.Tokens.Add(new GroupSiteTitleToken(cc.Web));

                Assert.IsTrue(CustomActionValidator.ValidateCustomActions(result2.SourceTemplate.CustomActions.WebCustomActions, result2.TargetTemplate.CustomActions.WebCustomActions, result2.TargetTokenParser, cc.Web));
            }
        }

        #region Helper methods
        private void DeleteCustomActions(ClientContext cc)
        {
            var siteActions = cc.Site.GetCustomActions();
            foreach (var action in siteActions)
            {
                if (action.Name.StartsWith("CA_"))
                {
                    cc.Site.DeleteCustomAction(action.Id);
                }
            }

            var webActions = cc.Web.GetCustomActions();
            foreach (var action in webActions)
            {
                if (action.Name.StartsWith("CA_"))
                {
                    cc.Web.DeleteCustomAction(action.Id);
                }
            }
        }
        #endregion

    }
}