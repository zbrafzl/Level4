
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Prp.Model
{

using System;
    using System.Collections.Generic;
    
public partial class tblHospitalDiscipline
{

    public int id { get; set; }

    public int hospitalId { get; set; }

    public int typeId { get; set; }

    public bool isApproved { get; set; }

    public int disciplineId { get; set; }

    public int specialityId { get; set; }

    public System.DateTime dateStart { get; set; }

    public System.DateTime dateEnd { get; set; }

    public string remarks { get; set; }

    public int adminId { get; set; }

    public System.DateTime dated { get; set; }

}

}
