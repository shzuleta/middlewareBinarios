using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Buffers.Text;
using FBapiService.DataDB;
using FBapiService.Models.Util;

namespace Models.logIng
{
    public class Logs
    {
        public int Id { get; set; }
        public DateTime dateSend { get; set; }
        public DateTime dateRequest { get; set; }
        public string level { get; set; }
        public string bank { get; set; }
        public string codeIntern { get; set; }

        //input
        public string currency { get; set; }
        public string gloss { get; set; }
        public decimal amount { get; set; }
        public DateTime expirationDate { get; set; }
        public bool singleUse { get; set; }
        public string additionalData { get; set; }
        public string destinationAccountId { get; set; }
        public string jsonInput { get; set; }

        //output
        public string idQR { get; set; }
        //public Base64 imageQR { get; set; }
        public string success { get; set; }
        public string messageOutput { get; set; }
        public string jsonOutput { get; set; }
    }

    class History
    {
        private readonly BanticanFintechContext _DBContext;

        public History()
        {
        }

        public History(BanticanFintechContext dbContext)
        {
            _DBContext = dbContext;
        }
        public string RegisterLog(Logs DataLog)
        {

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();


            using (SqlConnection conex = new SqlConnection(configuration["Conex"]))
            {
                try
                {
                    conex.Open();
                    string sql = "insert into historyLog ( datesend , daterequest , [level] , bank , currency , gloss , amount , expirationdate , singleuse, additionaldata , destinationaccountid , jsoninput , idQR , success , messageoutput , jsonoutput,codeInter) values ( @datesend, @daterequest, @level, @bank, @currency, @gloss, @amount, @expirationdate, @singleuse, @additionaldata, @destinationaccountid, @jsoninput, @idQR, @success, @messageoutput, @jsonoutput,@codeIntern )";

                    using (SqlCommand cmd = new SqlCommand(sql, conex))
                    {
                        //cmd.Parameters.AddWithValue("@id", 0);
                        cmd.Parameters.AddWithValue("@datesend", DataLog.dateSend);
                        cmd.Parameters.AddWithValue("@daterequest", DataLog.dateRequest);
                        cmd.Parameters.AddWithValue("@level", DataLog.level);
                        cmd.Parameters.AddWithValue("@bank", DataLog.bank);
                        cmd.Parameters.AddWithValue("@currency", DataLog.currency);
                        cmd.Parameters.AddWithValue("@gloss", DataLog.gloss);
                        cmd.Parameters.AddWithValue("@amount", DataLog.amount);
                        cmd.Parameters.AddWithValue("@expirationdate", DataLog.expirationDate);
                        cmd.Parameters.AddWithValue("@singleuse", DataLog.singleUse);
                        cmd.Parameters.AddWithValue("@additionaldata", DataLog.additionalData);
                        cmd.Parameters.AddWithValue("@destinationaccountid", DataLog.destinationAccountId);
                        cmd.Parameters.AddWithValue("@jsoninput", DataLog.jsonInput);
                        cmd.Parameters.AddWithValue("@idQR", DataLog.idQR);
                        cmd.Parameters.AddWithValue("@success", DataLog.success);
                        cmd.Parameters.AddWithValue("@messageoutput", DataLog.messageOutput);
                        cmd.Parameters.AddWithValue("@jsonoutput", "");// DataLog.jsonOutput);
                        cmd.Parameters.AddWithValue("@codeintern", DataLog.codeIntern);

                        int rows = cmd.ExecuteNonQuery();
                    }


                }
                catch (Exception ex)
                {
                    return ex.Message;
                    throw;
                }
                finally
                {
                    conex.Close();

                }
            }


            return "";
        }

        public string RegisterLog()
        {
            HistoryLog dataLog = new HistoryLog();

            dataLog.Id = 1;
            dataLog.Datesend = DateTime.Now;
            dataLog.Daterequest = DateTime.Now;
            dataLog.Level = "INFO";
            dataLog.Bank = "555"; //codigo de BNB 
            dataLog.CodeInter = "1111";

            dataLog.Currency = "BOB";
            dataLog.Gloss = "";
            dataLog.Amount = 1;
            dataLog.Expirationdate = DateTime.Parse("2023-07-31");
            dataLog.Singleuse = "1";
            dataLog.Additionaldata = "";
            dataLog.Destinationaccountid = "";
            dataLog.Jsoninput = "";

            dataLog.IdQr = "";
            dataLog.Messageoutput = "";
            dataLog.Jsonoutput = "";

            //if ((bool)objResobj.success)
            //{
            dataLog.Level = "WARNING";
            dataLog.Success = "1";
            //}
            //else
            //{
            //    dataLog.Success = "0";
            //}

            try
            {
                _DBContext.HistoryLogs.Add(dataLog);
                _DBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //objResobj.codError = ErrorType.er_NoRegistroLog.Id.ToString();
                //objResobj.descError = ErrorType.er_NoRegistroLog.Name.ToString() + "---" + ex.Message;
                throw;
            }
            finally
            {

            }

            return "OK";
        }
    }
    }

