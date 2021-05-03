using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Badgr;
using OpenCredentialPublisher.Shared.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{
    public class ClrViewModel
    {
        public ClrModel Clr { get; set; }
        /// <summary>
        /// The raw ClrDType hydrated from JSON .
        /// </summary>
        public ClrDType RawClrDType { get; set; }
       
        public bool HasPdf => Pdfs.Any(e => e.IsPdf);
       
        public const string PdfType = "data:application/pdf;";
       
        public CredentialPackageModel AncestorCredentialPackage { get; set; }
       
        public List<AssertionViewModel> AllAssertions { get; set; }
       
        public List<AssociatedAssertionViewModel> ParentAssertions { get; set; }
       
        public List<PdfShareViewModel> Pdfs { get; set; } = new List<PdfShareViewModel>();
        public static ClrViewModel FromBackpack(BadgrBackpackModel backpack)
        {
            var clrVM = new ClrViewModel() { Clr = new ClrModel(), AllAssertions = new List<AssertionViewModel>() };
            foreach (var assertion in backpack.BadgrAssertions)
            {
                clrVM.AllAssertions.Add(AssertionViewModel.FromBadgrAssertion(assertion, false));
            }

            return clrVM;
        }
        public static ClrViewModel FromClrModel(ClrModel clr)
        {
            var clrVM = new ClrViewModel() { Clr = clr, AllAssertions = new List<AssertionViewModel>()};
            if (!string.IsNullOrEmpty(clrVM.Clr.SignedClr))
            {
                clrVM.RawClrDType = clrVM.Clr.SignedClr.DeserializePayload<ClrDType>();
            }
            else
            {
                clrVM.RawClrDType = JsonSerializer.Deserialize<ClrDType>(clrVM.Clr.Json);
            }
            clrVM.SetAncestorCredentialPackage();
            clrVM.BuildAssertionsTree();

            return clrVM;
        }
        private void SetAncestorCredentialPackage()
        {
            if (Clr.CredentialPackage != null)
            {
                AncestorCredentialPackage = Clr.CredentialPackage;
            }
            else if (Clr.ClrSet != null)
            {
                AncestorCredentialPackage = Clr.ClrSet.CredentialPackage;
            }
            else if (Clr.VerifiableCredential != null)
            {
                AncestorCredentialPackage = Clr.VerifiableCredential.CredentialPackage;
            }
        }
        private void BuildAssertionsTree()
        {

            AllAssertions = new List<AssertionViewModel>();

            if (RawClrDType.SignedAssertions != null)
            {
                AllAssertions.AddRange(RawClrDType.SignedAssertions.Select(a => AssertionViewModel.FromAssertionDType(a.DeserializePayload<AssertionDType>(), true, a)));
            }

            if (RawClrDType.Assertions != null)
            {
                AllAssertions.AddRange(RawClrDType.Assertions.ConvertAll(a => AssertionViewModel.FromAssertionDType(a, true)));
            }
            foreach (var assertionVM in AllAssertions)
            {
                if (assertionVM.Assertion.Endorsements != null)
                {
                    assertionVM.AllEndorsements.AddRange(assertionVM.Assertion.Endorsements);
                }

                if (assertionVM.Assertion.Achievement != null)
                {
                    assertionVM.AchievementVM = AchievementViewModel.FromAchievementDType(assertionVM.Assertion.Achievement);

                    if (assertionVM.AchievementVM.Achievement.Endorsements != null)
                    {
                        assertionVM.AchievementVM.AllEndorsements.AddRange(assertionVM.AchievementVM.Achievement.Endorsements);
                    }

                    var issuer = assertionVM.AchievementVM.Achievement.Issuer;

                    if (issuer.Endorsements != null)
                    {
                        assertionVM.AchievementVM.AllEndorsements.AddRange(issuer.Endorsements);
                    }
                }
            }
            // Sort the assertions by issue date (oldest to newest)

            // AllAssertions = AllAssertions.OrderBy(a => a.IssuedOn).ToList();

            // Add associations to each assertion

            var associatedAssertions = new List<AssociatedAssertionViewModel>();

            foreach (var assertionVM in AllAssertions)
            {
                //var associatedAssertion =
                //    JsonSerializer.Deserialize<AssociatedAssertion>(assertion.ToJson()); // Performance Issue
                AssociatedAssertionViewModel associatedAssertion = assertionVM.Assertion;
                associatedAssertion.ChildAssertions = new List<AssociatedAssertionViewModel>();
                associatedAssertion.ClrVM = this;
                associatedAssertions.Add(associatedAssertion);
                if (associatedAssertion.Evidence != null &&
                    associatedAssertion.Evidence
                        .Any(aa => aa.Artifacts != null &&
                                aa.Artifacts.Any(artifact => artifact.Url != null)))
                {

                    foreach (var evidence in associatedAssertion.Evidence)
                    {
                        foreach (var artifact in evidence.Artifacts)
                        {
                            if (artifact.Url != null)
                            {
                                var model = new PdfShareViewModel
                                {
                                    ClrVM = this,
                                    ClrId = Clr.Id,
                                    ArtifactId = artifact.ArtifactKey,
                                    AssertionId = associatedAssertion.Id,
                                    EvidenceName = evidence.Name,
                                    ArtifactName = artifact.Name ?? artifact.Description,
                                    IsPdf = artifact.Url.StartsWith(PdfType)
                                };

                                if (!model.IsPdf)
                                {
                                    model.IsUrl = !artifact.Url.StartsWith("data:");
                                    model.ArtifactUrl = artifact.Url;
                                    if (!model.IsUrl)
                                    {
                                        model.MediaType = DataUrlUtility.GetMediaType(artifact.Url);
                                    }
                                }

                                Pdfs.Add(model);
                            }
                        }
                    }
                }
            }
            // Add child assertions

            foreach (var assertion in associatedAssertions)
            {
                if (assertion.Achievement?.Associations == null) continue;

                foreach (var association in assertion.Achievement.Associations)
                {
                    if (association.AssociationType ==
                        AssociationDType.AssociationTypeEnum.IsChildOfEnum
                        || association.AssociationType ==
                        AssociationDType.AssociationTypeEnum.IsPartOfEnum)
                    {
                        if (association.TargetId == assertion.Achievement.Id) continue;

                        var parentAssertion = associatedAssertions
                            .SingleOrDefault(a => a.Achievement.Id == association.TargetId);

                        if (parentAssertion != null)
                        {
                            assertion.ParentAssertion = parentAssertion;
                            if (!parentAssertion.ChildAssertions.Contains(assertion))
                            {
                                parentAssertion.ChildAssertions.Add(assertion);
                            }
                        }
                    }

                    if (association.AssociationType ==
                        AssociationDType.AssociationTypeEnum.IsParentOfEnum)
                    {
                        foreach (var childAssertion in associatedAssertions.Where(x => x.Achievement.Id == association.TargetId))
                        {

                            if (!assertion.ChildAssertions.Contains(childAssertion))
                            {
                                childAssertion.ParentAssertion = assertion;
                                assertion.ChildAssertions.Add(childAssertion);
                            }
                        }
                    }
                }
            }

            // Create the list of parent assertions

            ParentAssertions = associatedAssertions
                .Where(a => (a.ChildAssertions.Any() && a.ParentAssertion == null) || a.ParentAssertion == null)
                .ToList();
        }
    }
}
