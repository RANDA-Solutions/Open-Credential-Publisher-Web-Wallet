using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
   public  class AssertionResultsVM
    {
        public List<ResultVM> Results { get; set; }
        public List<ResultDescriptionVM> ResultDescriptions { get; set; }

        public AssertionResultsVM(List<ResultVM> results, List<ResultDescriptionVM> resultDescriptions)
        {
            Results = results;
            ResultDescriptions = resultDescriptions;
        }
    }
}
