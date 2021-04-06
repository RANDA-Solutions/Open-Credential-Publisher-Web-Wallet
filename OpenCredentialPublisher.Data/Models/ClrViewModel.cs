//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using OpenCredentialPublisher.ClrLibrary.Extensions;
//using OpenCredentialPublisher.ClrLibrary.Models;
//using Microsoft.AspNetCore.Mvc;
//using OpenCredentialPublisher.Data.ViewModels.Credentials;
//using System;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;

//namespace OpenCredentialPublisher.Data.Models
//{

//    public class ClrViewModel : ClrDType
//    {
//        public const string PdfType = "data:application/pdf;";

//        [JsonIgnore]
//        public List<AssertionDType> AllAssertions { get; set; }

//        [JsonIgnore]
//        [BindProperty]
//        public int ClrId { get; set; }

//        [JsonIgnore]
//        public bool HasPdf => Pdfs.Any();
//        [JsonIgnore]
//        public List<PdfShareViewModel> Pdfs { get; set; } = new List<PdfShareViewModel>();

//        [JsonIgnore]
//        public List<AssociatedAssertion> ParentAssertions { get; set; }

//        public void BuildAssertionsTree()
//        {
//            // First combine all the assertions

//            AllAssertions = new List<AssertionDType>();

//            if (SignedAssertions != null)
//            {
//                AllAssertions.AddRange(SignedAssertions.Select(a =>
//                {
//                    var assertion = a.DeserializePayload<AssertionDType>();
//                    assertion.IsSigned = true;
//                    return assertion;
//                }));
//            }

//            if (Assertions != null)
//            {
//                AllAssertions.AddRange(Assertions);
//            }
            

//            // Sort the assertions by issue date (oldest to newest)

//            // AllAssertions = AllAssertions.OrderBy(a => a.IssuedOn).ToList();

//            // Add associations to each assertion

//            var associatedAssertions = new List<AssociatedAssertion>();

//            foreach (var assertion in AllAssertions)
//            {
//                //var associatedAssertion =
//                //    JsonSerializer.Deserialize<AssociatedAssertion>(assertion.ToJson()); // Performance Issue
//                AssociatedAssertion associatedAssertion = assertion;
//                associatedAssertion.ChildAssertions = new List<AssociatedAssertion>();
//                associatedAssertion.Clr = this;
//                associatedAssertions.Add(associatedAssertion);
//                if (associatedAssertion.Evidence!= null &&
//                    associatedAssertion.Evidence
//                        .Any(aa => aa.Artifacts != null &&
//                                aa.Artifacts.Any(artifact => artifact.Url != null && artifact.Url.StartsWith("data:application/pdf;")))) {

//                    foreach (var evidence in associatedAssertion.Evidence) {
//                        foreach (var artifact in evidence.Artifacts) {
//                            if (artifact.Url != null && artifact.Url.StartsWith(PdfType))
//                            {
//                                var model = new PdfShareViewModel
//                                {
//                                    ClrModel = this,
//                                    ClrId = ClrId,
//                                    ArtifactId = artifact.ArtifactKey,
//                                    AssertionId = associatedAssertion.Id,
//                                    EvidenceName = evidence.Name,
//                                    ArtifactName = artifact.Name ?? artifact.Description
//                                };
//                                Pdfs.Add(model);
//                            }
//                        }
//                    }
//                }
//            }

//            // Add child assertions

//            foreach (var assertion in associatedAssertions)
//            {
//                if (assertion.Achievement?.Associations == null) continue;

//                foreach (var association in assertion.Achievement.Associations)
//                {
//                    if (association.AssociationType ==
//                        AssociationDType.AssociationTypeEnum.IsChildOfEnum
//                        || association.AssociationType ==
//                        AssociationDType.AssociationTypeEnum.IsPartOfEnum)
//                    {
//                        if (association.TargetId == assertion.Achievement.Id) continue;

//                        var parentAssertion = associatedAssertions
//                            .SingleOrDefault(a => a.Achievement.Id == association.TargetId);

//                        if (parentAssertion != null)
//                        {
//                            assertion.ParentAssertion = parentAssertion;
//                            if (!parentAssertion.ChildAssertions.Contains(assertion))
//                            {
//                                parentAssertion.ChildAssertions.Add(assertion);
//                            }
//                        }
//                    }

//                    if (association.AssociationType ==
//                        AssociationDType.AssociationTypeEnum.IsParentOfEnum)
//                    {
//                        foreach (var childAssertion in associatedAssertions.Where(x => x.Achievement.Id == association.TargetId))
//                        {

//                            if (!assertion.ChildAssertions.Contains(childAssertion))
//                            {
//                                childAssertion.ParentAssertion = assertion;
//                                assertion.ChildAssertions.Add(childAssertion);
//                            }
//                        }
//                    }
//                }
//            }

//            // Create the list of parent assertions

//            ParentAssertions = associatedAssertions
//                .Where(a => (a.ChildAssertions.Any() && a.ParentAssertion == null) || a.ParentAssertion == null)
//                .ToList();
//        }

        
//    }
//}
