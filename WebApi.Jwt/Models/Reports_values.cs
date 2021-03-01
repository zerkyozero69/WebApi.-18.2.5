using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models
{
    /// <summary>
    /// โมเดล หน้ากราฟและหน้าอนุมัติ
    /// </summary>
    public class Reports_values
    {
        public string Organization { get; set; }
        public DateTime DateTime { get; set; }
        public string Title { get; set; }
        public string detail { get; set; }
    }
    public class approve_values
    {
        public string Approve_Title { get; set; }
        public string Approve_Reports { get; set; }
    }
    public class approve_Detail
    {
        public int No { get; set; }
        public string Number_Send { get; set; }
        public string Receiver_Station { get; set; }
        public string Send_Station { get; set; }
        public string weight_All { get; set; }
    }
    public class approve_Detail_Distribute
    {
        public string Number_Send { get; set; }
        public string Send_Station { get; set; }
        public string Subscriber_Info { get; set; }
        public string weight_All { get; set; }
    }

    public class approve_ReportsDetail
        {
        public string Number_Send { get; set; }
        public string Number_Receiver { get; set; }
        public string Receiver_Station { get; set; }
        public string Send_Station { get; set; }
        public string budget_year {get;set;}
        public DateTime Date_Send { get; set; }
        public DateTime Date_Receiver { get; set; }
        public string budget_Type { get; set; }
        public string Supplies_Info { get; set; }
        public int Weight { get; set; }
        public int Weight_all { get; set; }

    }

    public class approve_ReportsDetail_Dispense
    {
        public string Number_Send { get; set; }
        public string Receiver_Station { get; set; }
        public string Send_Station { get; set; }
        public string Subscriber_Info { get; set; }    
        public string Name_Subscriber { get; set; }
        public string Address { get; set; }
        public string Subscriber_Count { get; set; }
        public string budget_Type { get; set; }
        public string Supplies_Info { get; set; }
        public int Weight { get; set; }
        public int Weight_all { get; set; }
        public string Remarks { get; set; }

    }
    public class approve_Detail_Sales
    {
        public int No { get; set; }
        public string Number_Sales { get; set; }
        public string Number_receipt { get; set; }
        public string Subscriber_Info { get; set; }
        public string Weight_All { get; set; }
        public List<approve_ReportsDetail_sales> sales { get; set; }
    }
    public class approve_ReportsDetail_sales
    {
        public string budget_source { get; set; }

        public string Seed_Type { get; set; }
        public string Supplies_Info { get; set; }
        public int Weight { get; set; }
        public int Price_Unit { get; set; }
        public int Price_all { get; set; }
        public string Quantity { get; set; }
        public string Capacity { get; set; }
        public int Money_all { get; set; }
        public int Weight_all { get; set; }
     
    }
    public class approve_Reports_Receiver_animal_supplies ///รับเสบียงสัตว์
    {  public string Number_Send { get; set; }
        public string Number_Receiver { get; set; }
        public string Receiver_Station { get; set; }
        public string Send_Station { get; set; }
        public string budget_Type { get; set; }
        public string Objective_Info { get; set; }
        public string Supplies_Info { get; set; }

        public List<approve_ReportsDetail_Receiver_animal_supplies> Receiver { get; set; }

        public string Remarks { get; set; }


        
    }
    public class approve_ReportsDetail_Receiver_animal_supplies  ///รายละเอียดการรับเสบียงสัตว์
    {
        public string Detail_animal_supplies{ get; set; }

        public int Weight_Unti { get; set; }
        public string Quantity { get; set; }
        public string Capacity { get; set; }
        public int Weight_all { get; set; }

    }
    public class approve_ReportsDetail_Send_animal_supplies
    {
        public string Number_Send { get; set; }
       
        public string Receiver_Station { get; set; }
        public string Send_Station { get; set; }
        public string budget_Type { get; set; }
        public string Objective_Info { get; set; }
        public string Supplies_Info { get; set; }

        public string Remarks { get; set; }
        public List<approve_ReportsDetail_Distribute> sendlist { get; set; }
    }
    public class approve_ReportsDetail_Distribute  
    {
        public string budget_source { get; set; }
   
   
        public string Subscriber_Count { get; set; }
        public string activities_info { get; set; }
        public string Supplies_Info { get; set; }
        public int Weight { get; set; }
        public string Quantity { get; set; }
        public string Capacity { get; set; }
        public int Weight_all { get; set; }
      

    }
}