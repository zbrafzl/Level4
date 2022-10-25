using Prp.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Prp.Data
{
    public class VerificationDAL : PrpDBConnect
    {

        public ApplicantApprovalStatus GetApplicationApprovalStatusGetByTypeAndId(int inductionId, int phaseId, int statusTypeId, int applicantId)
        {
            ApplicantApprovalStatus obj = new ApplicantApprovalStatus();
            try
            {
                var objt = db.spApplicationApprovalStatusGetByTypeAndId(inductionId, phaseId, statusTypeId, applicantId).FirstOrDefault();
                if (objt != null && objt.applicationApprovalStatusId > 0)
                    obj = MapVerification.ToEntity(objt);
            }
            catch (Exception)
            {
                obj = new ApplicantApprovalStatus();
            }
            return obj;
        }

        public List<ApplicantApprovalStatus> GetApplicationApprovalStatusGetById(int inductionId, int phaseId, int applicantId)
        {
            List<ApplicantApprovalStatus> list = new List<ApplicantApprovalStatus>();
            try
            {
                var listt = db.spApplicationApprovalStatusGetById(inductionId, phaseId, applicantId).ToList();
                list = MapVerification.ToEntityList(listt);
            }
            catch (Exception)
            {
                list = new List<ApplicantApprovalStatus>();
            }
            return list;
        }


        public ApplicationVerificationStatus GetApplicationAmendmentsStatus(int applicantId)
        {
            ApplicationVerificationStatus obj = new ApplicationVerificationStatus();
            try
            {
                var objt = db.tvwApplicationAmendments.FirstOrDefault(x => x.applicantId == applicantId);
                obj = MapVerification.ToEntity(objt);
            }
            catch (Exception)
            {
                obj = new ApplicationVerificationStatus();
            }
            return obj;
        }

        public Message GetApplicantIdBySearchVerification(string search, string condition)
        {
            Message msg = new Message();
            try
            {

                var item = db.spApplicantGetBySearchVerification(search, condition);
                msg.id = item;
            }
            catch (Exception ex)
            {
                msg.status = false;
                msg.msg = ex.Message;
            }
            return msg;
        }

        public InterviewDetail GetApplicantInterview(int inductionId, int applicantId)
        {
            InterviewDetail interviewDetail = new InterviewDetail();
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[spApplicantGetInterviewDetail]"
            };
            try
            {
                con = new SqlConnection(PrpDbConnectADO.Conn);
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@inductionId", inductionId);
                cmd.Parameters.AddWithValue("@applicantId", applicantId);
                DataTable dt = PrpDbADO.FillDataTable(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    interviewDetail.applicantId = Convert.ToInt32(dr[0]);
                    interviewDetail.interviewMarks = Convert.ToInt32(dr[1]);
                    interviewDetail.interviewComments = dr[2].ToString();
                    interviewDetail.marksLoR = Convert.ToInt32(dr[3]);
                    interviewDetail.picLoR = dr[4].ToString();
                }
            }
            catch (Exception ex)
            {
                interviewDetail.interviewComments = "";
            }
            finally
            {
                con.Close();
            }
            return interviewDetail;
        }

        public Message GetApplicantIdBySearchInterview(string search, string condition)
        {
            Message msg = new Message();
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[spApplicantGetBySearchInterview]"
            };
            try
            {
                con = new SqlConnection(PrpDbConnectADO.Conn);
                con.Open();
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@search", search);
                cmd.Parameters.AddWithValue("@condition", condition);
                DataTable dt = PrpDbADO.FillDataTable(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    msg.id = Convert.ToInt32(dr[0]);
                }
            }
            catch (Exception ex)
            {
                msg.status = false;
                msg.msg = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return msg;
        }

        public Message AddUpdateVerficationStatus(VerificationEntity obj)
        {
            Message msg = new Message();
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "[dbo].[spApplicantApprovalStatusAddUpdate]"
                };
                cmd.Parameters.AddWithValue("@inductionId", obj.inductionId);
                cmd.Parameters.AddWithValue("@phaseId", obj.phaseId);
                cmd.Parameters.AddWithValue("@applicantId", obj.applicantId);
                cmd.Parameters.AddWithValue("@approvalStatusTypeId", obj.approvalStatusTypeId);
                cmd.Parameters.AddWithValue("@approvalStatusId", obj.approvalStatusId);
                cmd.Parameters.AddWithValue("@comments", obj.comments);
                cmd.Parameters.AddWithValue("@adminId", obj.adminId);
                DataTable dt = PrpDbADO.FillDataTable(cmd);

                msg = dt.ConvertToEnitityMessage();
            }
            catch (Exception ex)
            {
                msg.status = false;
                msg.msg = ex.Message;
            }
            return msg;
        }

        public Message AddUpdateInterviewStatus(applicantInterviewMarksList obj)
        {
            SqlConnection con = new SqlConnection();
            con = new SqlConnection(PrpDbConnectADO.Conn);
            
            Message msg = new Message();
            try
            {
                string query = "";
                foreach (var item in obj.interviewMarksListItem)
                {
                    query += " update tblApplicantPreferenceInterviewMarks set marks = " + item.marksInterviewPref + " where applicantId = " + obj.applicantId + " and inductionId = " + obj.inductionId + " and phaseId = " + obj.phaseId + " and preferenceNo = " + item.prefNo + "";
                }
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                cmd.ExecuteNonQuery();

                msg.status = true;
                msg.msg = "Ok";
            }
            catch (Exception ex)
            {
                msg.status = false;
                msg.msg = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return msg;
        }

        public DataTable ApplicantListVerifyView(VerificationEntity obj)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[spApplicantListVerify]"
            };
            cmd.Parameters.AddWithValue("@statusId", obj.statusId);
            cmd.Parameters.AddWithValue("@condition", obj.condition);

            cmd.Parameters.AddWithValue("@condition", obj.condition);
            return PrpDbADO.FillDataTable(cmd);
        }

        public DataTable ApplicantListVerifyExport(VerificationEntity obj)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[spApplicantListVerifyExport]"
            };

            cmd.Parameters.AddWithValue("@statusId", obj.statusId);
            return PrpDbADO.FillDataTable(cmd);
        }


        public DataTable GetApplicationHasAmedmentAndNotSentEmail()
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[spGetApplicationHasAmedmentAndNotSentEmail]"
            };

            return PrpDbADO.FillDataTable(cmd);
        }
    }
}
