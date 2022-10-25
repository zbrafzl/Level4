using Prp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prp.Sln.Areas.nadmin.Controllers
{
    public class SubjectAdminController : BaseAdminController
    {
        // GET: nadmin/SubjectAdmin
        public ActionResult SubjectSetup()
        {
            SpecialityModelAdmin model = new SpecialityModelAdmin();
            int subjectId = Request.QueryString["specialityId"].TooInt();
            int supersSubjectId = Request.QueryString["superSpecialityId"].TooInt();
            if (subjectId > 0)
                model.superSpeciality = new SpecialityDAL().SuperSpecialityGetById(subjectId);
            model.superSpeciality = new SpecialityDAL().SuperSpecialityGetById(subjectId);

            DDLSpecialitys dDLSpeciality = new DDLSpecialitys();
            dDLSpeciality.condition = "GetAll";
            model.listSpeciality = new SpecialityDAL().GetSpecialityDDL(dDLSpeciality);
            return View(model);
        }


       

        public ActionResult SubjectManage()
        {
            SpecialityModelAdmin model = new SpecialityModelAdmin();

            model.inductionId = Request.QueryString["inductionId"].TooInt();
            model.listInduction = DDLInduction.GetAll("GetAllActive");

            if (model.inductionId == 0)
            {
                if (model.listInduction != null && model.listInduction.Count == 1)
                    model.inductionId = model.listInduction.FirstOrDefault().id.TooInt();
            }
            if (model.inductionId > 0)
                model.list = new SpecialityDAL().GetWithJobByInduction(model.inductionId);
            else
                model.superSpecialities = new SpecialityDAL().GetAdminSuperSpecialities();

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult SaveSubjectData(SpecialityModelAdmin ModelSave, HttpPostedFileBase files)
        {
            Speciality obj = ModelSave.speciality;

            obj.name = obj.name.TooString();
            obj.isActive = obj.isActive.TooBoolean();
            obj.sortOrder = obj.sortOrder.TooInt();
            obj.dated = DateTime.Now;
            obj.adminId = loggedInUser.userId;

            Message m = new SpecialityDAL().AddUpdate(obj);

            return Redirect("/admin/speciality-manage");
        }


        [ValidateInput(false)]
        public ActionResult SaveSubSpecialitytData(SpecialityModelAdmin ModelSave, HttpPostedFileBase files)
        {
            SuperSpeciality obj = ModelSave.superSpeciality;
            obj.specialityId = obj.specialityId.TooInt();
            obj.name = obj.name.TooString();
            obj.isActive = obj.isActive.TooBoolean();
            obj.sortOrder = obj.sortOrder.TooInt();
            obj.dated = DateTime.Now;
            obj.adminId = loggedInUser.userId;

            Message m = new SpecialityDAL().AddUpdateSuperSpeciality(obj);

            return Redirect("/admin/speciality-manage");
        }
    }
}