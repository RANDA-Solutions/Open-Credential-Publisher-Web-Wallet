﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace OpenCredentialPublisher.Data.Xsds
{
}

namespace OpenCredentialPublisher.Data.Xsds
{
}

namespace OpenCredentialPublisher.Data.Xsds
{
}

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class DIHistoryReport {
    
    private DIHistoryReportCandidate[] candidateField;
    
    private string testyearField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("candidate", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public DIHistoryReportCandidate[] candidate {
        get {
            return this.candidateField;
        }
        set {
            this.candidateField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string testyear {
        get {
            return this.testyearField;
        }
        set {
            this.testyearField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class DIHistoryReportCandidate {
    
    private string lastnameField;
    
    private string firstnameField;
    
    private string dobField;
    
    private string ssnField;
    
    private string addressField;
    
    private string cityField;
    
    private string stateField;
    
    private string zipcodeField;
    
    private DIHistoryReportCandidateTest[] testField;
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string lastname {
        get {
            return this.lastnameField;
        }
        set {
            this.lastnameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string firstname {
        get {
            return this.firstnameField;
        }
        set {
            this.firstnameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string dob {
        get {
            return this.dobField;
        }
        set {
            this.dobField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string ssn {
        get {
            return this.ssnField;
        }
        set {
            this.ssnField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string address {
        get {
            return this.addressField;
        }
        set {
            this.addressField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string city {
        get {
            return this.cityField;
        }
        set {
            this.cityField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string state {
        get {
            return this.stateField;
        }
        set {
            this.stateField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string zipcode {
        get {
            return this.zipcodeField;
        }
        set {
            this.zipcodeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("test", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public DIHistoryReportCandidateTest[] test {
        get {
            return this.testField;
        }
        set {
            this.testField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class DIHistoryReportCandidateTest {
    
    private string testdateField;
    
    private string testnameField;
    
    private string scoreField;
    
    private string reportdateField;
    
    private DIHistoryReportCandidateTestCategory[] categoryField;
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string testdate {
        get {
            return this.testdateField;
        }
        set {
            this.testdateField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string testname {
        get {
            return this.testnameField;
        }
        set {
            this.testnameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string score {
        get {
            return this.scoreField;
        }
        set {
            this.scoreField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string reportdate {
        get {
            return this.reportdateField;
        }
        set {
            this.reportdateField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("category", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public DIHistoryReportCandidateTestCategory[] category {
        get {
            return this.categoryField;
        }
        set {
            this.categoryField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class DIHistoryReportCandidateTestCategory {
    
    private string category1pointsearnedField;
    
    private string category2pointsearnedField;
    
    private string category3pointsearnedField;
    
    private string category4pointsearnedField;
    
    private string category5pointsearnedField;
    
    private string category6pointsearnedField;
    
    private string category7pointsearnedField;
    
    private string category8pointsearnedField;
    
    private string category9pointsearnedField;
    
    private string category10pointsearnedField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string category1pointsearned {
        get {
            return this.category1pointsearnedField;
        }
        set {
            this.category1pointsearnedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string category2pointsearned {
        get {
            return this.category2pointsearnedField;
        }
        set {
            this.category2pointsearnedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string category3pointsearned {
        get {
            return this.category3pointsearnedField;
        }
        set {
            this.category3pointsearnedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string category4pointsearned {
        get {
            return this.category4pointsearnedField;
        }
        set {
            this.category4pointsearnedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string category5pointsearned {
        get {
            return this.category5pointsearnedField;
        }
        set {
            this.category5pointsearnedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string category6pointsearned {
        get {
            return this.category6pointsearnedField;
        }
        set {
            this.category6pointsearnedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string category7pointsearned {
        get {
            return this.category7pointsearnedField;
        }
        set {
            this.category7pointsearnedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string category8pointsearned {
        get {
            return this.category8pointsearnedField;
        }
        set {
            this.category8pointsearnedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string category9pointsearned {
        get {
            return this.category9pointsearnedField;
        }
        set {
            this.category9pointsearnedField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string category10pointsearned {
        get {
            return this.category10pointsearnedField;
        }
        set {
            this.category10pointsearnedField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class NewDataSet {
    
    private DIHistoryReport[] itemsField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("DIHistoryReport")]
    public DIHistoryReport[] Items {
        get {
            return this.itemsField;
        }
        set {
            this.itemsField = value;
        }
    }
}