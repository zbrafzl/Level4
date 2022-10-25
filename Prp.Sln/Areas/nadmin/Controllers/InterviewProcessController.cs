using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Prp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prp.Sln.Areas.nadmin.Controllers
{
    public class InterviewProcessController : BaseAdminController
    {
        
        [CheckHasRight]
        public ActionResult ApplicantListForInterview()
        {

            if (loggedInUser.typeId == ProjConstant.Constant.UserType.applicant)
            {

            }

            ProofReadingAdminModel model = new ProofReadingAdminModel();
            try
            {
                int inductionId = AdminHelper.GetInductionId();
                int phaseId = AdminHelper.GetPhaseId();
                model.applicantId = Request.QueryString["applicantid"].TooInt();
                if (model.applicantId > 0)
                {
                    model = AdminFunctions.GenerateModelProofReading(inductionId, phaseId, model.applicantId);
                }
            }
            catch (Exception)
            {
                model.applicantId = 0;
            }
            return View(model);
        }

        public ActionResult InterviewTeam()
        {
            InterviewAdminModel model = new InterviewAdminModel();
            try
            {
                string key = Request.QueryString["key"].TooString();
                string value = Request.QueryString["value"].TooString();

                model.search.key = key;
                model.search.value = value;

                if (!String.IsNullOrEmpty(key) && !String.IsNullOrWhiteSpace(value))
                {

                    Message msg = new VerificationDAL().GetApplicantIdBySearchInterview(value, key);
                    int applicantId = msg.id.TooInt();
                    if (applicantId > 0)
                    {
                        int inductionId = AdminHelper.GetInductionId();
                        int phaseId = AdminHelper.GetPhaseId();
                        //model = AdminFunctions.GenerateModelProofReading(inductionId, phaseId, applicantId);

                        model = AdminFunctions.GenerateModeInterview(inductionId, phaseId, applicantId);


                        model.search.key = key;
                        model.search.value = value;

                    }

                    model.statusId = msg.statusId;

                    model.applicantId = applicantId;
                    model.requestType = 1;
                }
                else
                {
                    ViewBag.imgLoR = "";
                    model.imgLoR = "";
                    model.applicantId = 0;
                    model.requestType = 2;
                }

            }
            catch (Exception)
            {
                model.applicantId = 0;
                model.requestType = 3;
            }
            return View(model);
        }

        [CheckHasRight]
        public ActionResult InterviewView()
        {
            ProofReadingAdminModel model = new ProofReadingAdminModel();
            try
            {
                string key = Request.QueryString["key"].TooString();
                string value = Request.QueryString["value"].TooString();

                model.search.key = key;
                model.search.value = value;

                if (!String.IsNullOrEmpty(key) && !String.IsNullOrWhiteSpace(value))
                {
                    Message msg = new VerificationDAL().GetApplicantIdBySearchVerification(value, key);
                    int applicantId = msg.id.TooInt();
                    if (applicantId > 0)
                    {
                        int inductionId = AdminHelper.GetInductionId();
                        int phaseId = AdminHelper.GetPhaseId();
                        model = AdminFunctions.GenerateModelProofReading(inductionId, phaseId, applicantId);
                        model.search.key = key;
                        model.search.value = value;


                    }
                    model.statusId = msg.statusId;
                    model.applicantId = applicantId;
                    model.requestType = 1;
                }
                else
                {

                    model.applicantId = 0;
                    model.requestType = 2;
                }

            }
            catch (Exception)
            {
                model.applicantId = 0;
                model.requestType = 3;
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult AddUpdateInterviewStatus(applicantInterviewMarksList obj)
        {
            //obj.comments = obj.comments.TooString();
            //obj.picLetter = obj.picLetter.TooString();

            //bool voucherStatus = true;

            //try
            //{
            //    ApplicationStatus objStatus = new ApplicantDAL().GetApplicationStatus(ProjConstant.inductionId, ProjConstant.phaseId
            //    , obj.applicantId, ProjConstant.Constant.ApplicationStatusType.voucherPhf).FirstOrDefault();

            //    if (objStatus != null && (objStatus.statusId == 6 || objStatus.statusId == 9))
            //    {
            //        voucherStatus = false;
            //    }
            //}
            //catch (Exception)
            //{
            //}

            //if (voucherStatus == false)
            //{
            //    obj.approvalStatusId = 2;
            //    if (!obj.comments.Contains("Voucher/Payment"))
            //    {
            //        if (String.IsNullOrWhiteSpace(obj.comments))
            //            obj.comments = " Voucher/Payment Unverified";
            //        else
            //            obj.comments = obj.comments + ", Voucher/Payment Unverified";
            //    }

            //}

            Message msg = new VerificationDAL().AddUpdateInterviewStatus(obj);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}