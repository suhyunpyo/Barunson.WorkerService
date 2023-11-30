namespace Barunson.WorkerService.CommonBatchJob.Models
{
    /// <summary>
    /// CJ API Model - Root
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CJModelRoot<T>
    {
        public T DATA { get; set; } = default(T);
    }

    /// <summary>
    /// CJ API - Token
    /// </summary>
    public class CJModelToken
    {
        public string CUST_ID { get; set; }
        public string BIZ_REG_NUM { get; set; }
    }

    public class CJModelCommon
    {
        public string TOKEN_NUM { get; set; }
        public string CUST_ID { get; set; }

    }
    /// <summary>
    /// CJ API RegBook
    /// </summary>
    public class CJModelRegBook : CJModelCommon
    {
        public string RCPT_YMD { get; set; }
        public string CUST_USE_NO { get; set; }
        public string RCPT_DV { get; set; }
        public string WORK_DV_CD { get; set; }
        public string CUST_WORK_CD { get; }
        public string REQ_DV_CD { get; set; }
        public string MPCK_KEY { get; set; }
        public string CAL_DV_CD { get; set; }
        public string FRT_DV_CD { get; set; }
        public string CNTR_ITEM_CD { get; set; }
        public string BOX_TYPE_CD { get; set; }
        public string BOX_QTY { get; set; }

        public string FRT { get; set; }
        public string CUST_MGMT_DLCM_CD { get; set; }
        public string SENDR_NM { get; set; }
        public string SENDR_TEL_NO1 { get; set; }
        public string SENDR_TEL_NO2 { get; set; }
        public string SENDR_TEL_NO3 { get; set; }
        public string SENDR_CELL_NO1 { get; set; }
        public string SENDR_CELL_NO2 { get; set; }
        public string SENDR_CELL_NO3 { get; set; }
        public string SENDR_SAFE_NO1 { get; set; }
        public string SENDR_SAFE_NO2 { get; set; }
        public string SENDR_SAFE_NO3 { get; set; }
        public string SENDR_ZIP_NO { get; set; }
        public string SENDR_ADDR { get; set; }
        public string SENDR_DETAIL_ADDR { get; set; }
        public string RCVR_NM { get; set; }
        public string RCVR_TEL_NO1 { get; set; }
        public string RCVR_TEL_NO2 { get; set; }
        public string RCVR_TEL_NO3 { get; set; }
        public string RCVR_CELL_NO1 { get; set; }
        public string RCVR_CELL_NO2 { get; set; }
        public string RCVR_CELL_NO3 { get; set; }
        public string RCVR_SAFE_NO1 { get; set; }
        public string RCVR_SAFE_NO2 { get; set; }
        public string RCVR_SAFE_NO3 { get; set; }
        public string RCVR_ZIP_NO { get; set; }
        public string RCVR_ADDR { get; set; }
        public string RCVR_DETAIL_ADDR { get; set; }
        public string ORDRR_NM { get; set; }
        public string ORDRR_TEL_NO1 { get; set; }
        public string ORDRR_TEL_NO2 { get; set; }
        public string ORDRR_TEL_NO3 { get; set; }
        public string ORDRR_CELL_NO1 { get; set; }
        public string ORDRR_CELL_NO2 { get; set; }
        public string ORDRR_CELL_NO3 { get; set; }
        public string ORDRR_SAFE_NO1 { get; set; }
        public string ORDRR_SAFE_NO2 { get; set; }
        public string ORDRR_SAFE_NO3 { get; set; }
        public string ORDRR_ZIP_NO { get; set; }
        public string ORDRR_ADDR { get; set; }
        public string ORDRR_DETAIL_ADDR { get; set; }
        public string INVC_NO { get; set; }
        public string ORI_INVC_NO { get; set; }
        public string ORI_ORD_NO { get; set; }
        public string COLCT_EXPCT_YMD { get; set; }
        public string COLCT_EXPCT_HOUR { get; set; }
        public string SHIP_EXPCT_YMD { get; set; }
        public string SHIP_EXPCT_HOUR { get; set; }
        public string PRT_ST { get; set; }
        public string ARTICLE_AMT { get; set; }
        public string REMARK_1 { get; set; }
        public string REMARK_2 { get; set; }
        public string REMARK_3 { get; set; }
        public string COD_YN { get; set; }
        public string ETC_1 { get; set; }
        public string ETC_2 { get; set; }
        public string ETC_3 { get; set; }
        public string ETC_4 { get; set; }
        public string ETC_5 { get; set; }
        public string DLV_DV { get; set; }
        public string RCPT_SERIAL { get; set; }

        public List<CJModelMPCK> ARRAY { get; set; }
    }

    public class CJModelMPCK
    {
        public string MPCK_SEQ { get; set; }
        public string GDS_CD { get; set; }
        public string GDS_NM { get; set; }
        public string GDS_QTY { get; set; }
        public string UNIT_CD { get; set; }
        public string UNIT_NM { get; set; }
        public string GDS_AMT { get; set; }
    }


    public class CJResponse<T>
    {
        public string RESULT_CD { get; set; }
        public string RESULT_DETAIL { get; set; }
        public T DATA { get; set; } = default(T);
    }
    public class CJResonseEmpty
    { }

    public class CJResponseToken
    {
        public string TOKEN_NUM { get; set; }
        public string TOKEN_EXPRTN_DTM { get; set; }
        public string NOTICE { get; set; }
    }

    public class DelivertyData
    {
        public int ORDER_SEQ { get; set; }
        public string ORDER_TABLE_NAME { get; set; }
        public string DELIVERY_CODE { get; set; }
        public int DELIVERY_SEQ { get; set; }
        public string DELIVERY_MSG { get; set; }
        public string RECV_NAME { get; set; }
        public string RECV_ZIP { get; set; }
        public string RECV_ADDR { get; set; }

        public string RECV_ADDR_DETAIL { get; set; }
        public string RECV_PHONE { get; set; }
        public string RECV_HPHONE { get; set; }
        public DateTime? SEND_DATE { get; set; }
        public string ERP_PartCode { get; set; }
        public string SALES_GUBUN { get; set; }
    }
}
